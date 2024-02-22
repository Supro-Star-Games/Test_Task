using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemViewPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _closeButton;

    [SerializeField] private GameObject _helmetIcon;
    [SerializeField] private GameObject _armorIcon;
    [SerializeField] private GameObject _healIcon;
    [SerializeField] private GameObject _ammoIcon;

    [SerializeField] private TextMeshProUGUI _useButtonText;
    [SerializeField] private TextMeshProUGUI _useValue;
    [SerializeField] private TextMeshProUGUI _weghtValue;

    private InventoryItem _currentItem;

    public event Action<InventoryItem> DeleteItem;

    public event Action<InventoryItem> UseItem;


    private void OnEnable()
    {
        _closeButton.onClick.AddListener(HidePopUp);
        _deleteButton.onClick.AddListener(OnDelete);
        _useButton.onClick.AddListener(OnUse);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(HidePopUp);
        _deleteButton.onClick.RemoveListener(OnDelete);
        _useButton.onClick.AddListener(OnUse);
    }

    public void SetView(InventoryItem item)
    {
        _currentItem = item;
        _itemName.text = _currentItem.Name;
        _itemImage.sprite = _currentItem.Sprite;
        _weghtValue.text = $"{_currentItem.Weight} Kg";
        switch (item)
        {
            case ClothInventoryItem _clothItem:
                ShowClothView(_clothItem);
                break;
            case MedKitInventoryItem _medItem:
                ShowMedView(_medItem);
                break;
            case AmmoInventoryItem _ammoItem:
                ShowAmmoView(_ammoItem);
                break;
        }

        gameObject.SetActive(true);
    }

    private void ShowClothView(ClothInventoryItem _item)
    {
        _useButtonText.text = "Equip";
        _useValue.text = $"+ {_item.Armor}";

        if (_item.DefendType == DefendType.Head)
        {
            _helmetIcon.SetActive(true);
        }
        else
        {
            _armorIcon.SetActive(true);
        }
    }

    private void OnDelete()
    {
        DeleteItem?.Invoke(_currentItem);
        HidePopUp();
    }

    private void OnUse()
    {
        UseItem?.Invoke(_currentItem);
        HidePopUp();
    }

    private void ShowMedView(MedKitInventoryItem _item)
    {
        _useButtonText.text = "Use";
        _useValue.text = $"+ {_item.HpRestore}";
        _healIcon.SetActive(true);
    }

    private void ShowAmmoView(AmmoInventoryItem _item)
    {
        _useButtonText.text = "Buy";
        _useValue.text = $"+ {_item.MaxStack - _item.Count}";
        _ammoIcon.SetActive(true);
    }

    private void HidePopUp()
    {
        switch (_currentItem)
        {
            case ClothInventoryItem cloth:
                _armorIcon.SetActive(false);
                _helmetIcon.SetActive(false);
                break;
            case MedKitInventoryItem med:
                _healIcon.SetActive(false);
                break;
            case AmmoInventoryItem ammo:
                _ammoIcon.SetActive(false);
                break;
        }

        _currentItem = null;
        gameObject.SetActive(false);
    }
}