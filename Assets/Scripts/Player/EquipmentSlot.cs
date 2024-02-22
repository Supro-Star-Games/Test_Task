using System;
using UnityEngine;


public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private DefendType _slotType;

    public ClothInventoryItem CurrentItem { get; private set; }
    public RectTransform SRectTransform { get; private set; }
    public DefendType SlotType => _slotType;

    public event Action<ClothInventoryItem> ItemChanged;

    public event Action<ClothInventoryItem> ItemRemoved;

    private void Awake()
    {
        SRectTransform = GetComponent<RectTransform>();
    }

    public void ChangeItem(ClothInventoryItem cloth)
    {
        CurrentItem = cloth;
        if (_slotType == DefendType.Head)
        {
            SaveManager.Instance.SaveEquippedItem(CurrentItem.ItemSO, true);
        }
        else
        {
            SaveManager.Instance.SaveEquippedItem(CurrentItem.ItemSO, false);
        }

        ItemChanged?.Invoke(cloth);
    }

    public void RemoveItem()
    {
        ItemRemoved?.Invoke(CurrentItem);
        CurrentItem = null;
    }
}