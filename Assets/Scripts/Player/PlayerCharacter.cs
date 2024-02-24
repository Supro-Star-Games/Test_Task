using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerCharacter : Character
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private EquipmentSlot _bodySlot;
    [SerializeField] private EquipmentSlot _headSlot;
    [SerializeField] private ItemViewPopUp _itemViewPopUp;

    private int _headArmor;
    private int _bodyArmor;

    public event Action<int> HeadArmorChanged;
    public event Action<int> BodyArmorChanged;
    public event Action HeadArmorRemoved;
    public event Action BodyArmorRemoved;
    public event Action PlayerDefeated;

    private bool _isHeadDamage;

    public override void LoadData(int value)
    {
        base.LoadData(value);
        OnUseItem(_inventory.GetItem(SaveManager.Instance.HeadItem));
        OnUseItem(_inventory.GetItem(SaveManager.Instance.BodyItem));
    }

    public override void Initialize()
    {
        currentHP = maxHP;
        SaveManager.Instance.PlayerHp = currentHP;
        HpChange(currentHP, (float)currentHP / maxHP);
    }

    private void OnEnable()
    {
        _bodySlot.ItemChanged += SetArmor;
        _bodySlot.ItemRemoved += RemoveArmor;
        _headSlot.ItemRemoved += RemoveArmor;
        _headSlot.ItemChanged += SetArmor;
        _itemViewPopUp.UseItem += OnUseItem;
    }

    private void OnDisable()
    {
        _bodySlot.ItemRemoved -= RemoveArmor;
        _headSlot.ItemRemoved -= RemoveArmor;
        _bodySlot.ItemChanged -= SetArmor;
        _headSlot.ItemChanged -= SetArmor;
        _itemViewPopUp.UseItem -= OnUseItem;
    }


    private void RemoveArmor(ClothInventoryItem item)
    {
        if (item.DefendType == DefendType.Head)
        {
            _headArmor = 0;
            HeadArmorRemoved?.Invoke();
        }
        else
        {
            _bodyArmor = 0;
            BodyArmorRemoved?.Invoke();
        }
    }

    private void SetArmor(ClothInventoryItem armorItem)
    {
        if (armorItem.DefendType == DefendType.Head)
        {
            _headArmor = armorItem.Armor;
            HeadArmorChanged?.Invoke(_headArmor);
        }
        else
        {
            _bodyArmor = armorItem.Armor;
            BodyArmorChanged?.Invoke(_bodyArmor);
        }
    }


    private void OnUseItem(InventoryItem item)
    {
        switch (item)
        {
            case MedKitInventoryItem medKitItem:
                SetCurrentHp(medKitItem.HpRestore);
                medKitItem.SetCurrentCount(medKitItem.Count - 1);
                if (medKitItem.Count <= 0)
                {
                    _inventory.DeleteItem(medKitItem);
                }

                break;
            case ClothInventoryItem clothItem:
                if (clothItem.DefendType == DefendType.Head)
                {
                    clothItem.SwapPlaces(_headSlot, clothItem);
                    _headSlot.ChangeItem(clothItem);
                }
                else
                {
                    clothItem.SwapPlaces(_bodySlot, clothItem);
                    _bodySlot.ChangeItem(clothItem);
                }

                break;
            case AmmoInventoryItem ammoItem:
                ammoItem.SetCurrentCount(0, true);
                break;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (_isHeadDamage)
        {
            int currentDamage = damage - _headArmor;
            if (currentDamage < 0)
            {
                currentDamage = 0;
            }

            _isHeadDamage = false;
            currentHP -= currentDamage;
        }
        else
        {
            int currentDamage = damage - _bodyArmor;
            if (currentDamage < 0)
            {
                currentDamage = 0;
            }

            _isHeadDamage = true;
            currentHP -= currentDamage;
        }

        if (currentHP <= 0)
        {
            PlayerDefeated?.Invoke();
        }

        SaveManager.Instance.PlayerHp = currentHP;
        HpChange(currentHP, (float)currentHP / maxHP);
    }

    private void SetCurrentHp(int hpValue)
    {
        currentHP += hpValue;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        SaveManager.Instance.PlayerHp = currentHP;
        HpChange(currentHP, (float)currentHP / maxHP);
    }
}