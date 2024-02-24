using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _itemTemplate;
    [SerializeField] private List<InventoryData> _startItems;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private ItemViewPopUp _itemViewPopUp;

    private GridLayoutGroup _grid;
    private List<InventorySlot> _slots;
    private List<InventoryItem> _items = new();

    private void OnEnable()
    {
        _itemViewPopUp.DeleteItem += DeleteItem;
    }

    private void OnDisable()
    {
        _itemViewPopUp.DeleteItem -= DeleteItem;
    }

    public void Initialize()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _slots = GetComponentsInChildren<InventorySlot>().ToList();

        foreach (var slot in _slots)
        {
            slot.IsOccupied = false;
        }

        if (_items.Count > 0)
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }

            _items.Clear();
        }

        foreach (var data in _startItems)
        {
            AddItem(data.ItemSO, data.Amount);
        }
    }

    public void LoadData(List<InventoryData> items)
    {
        _grid = GetComponent<GridLayoutGroup>();
        _slots = GetComponentsInChildren<InventorySlot>().ToList();

        foreach (var item in items)
        {
            AddItem(item.ItemSO, item.Amount);
        }
    }

    public void AddItem(ScriptableObject data, int count = 1)
    {
        var slot = _slots.First(s => !s.IsOccupied);
        if (slot)
        {
            UpdateGrid(_grid);
            var slotRect = slot.GetComponent<RectTransform>();
            var newItem = Instantiate(_itemTemplate, _contentTransform);
            newItem.GetComponent<RectTransform>().anchoredPosition = slotRect.anchoredPosition;

            switch (data)
            {
                case ClothItem clothItem:
                    var clothInventoryItem = newItem.AddComponent<ClothInventoryItem>();
                    SetItem(clothInventoryItem, clothItem, slot, count);
                    break;
                case AmmoItem ammoItem:
                    var ammoInventoryItem = newItem.AddComponent<AmmoInventoryItem>();
                    SetItem(ammoInventoryItem, ammoItem, slot, count);
                    break;
                case MedKitItem medKitItem:
                    var medKitInventoryItem = newItem.AddComponent<MedKitInventoryItem>();
                    SetItem(medKitInventoryItem, medKitItem, slot, count);
                    break;
            }
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    private void SetItem(InventoryItem item, Item itemSo, InventorySlot slot, int amount)
    {
        item.SetSO(itemSo);
        item.InventorySlot = slot;
        item.Initialize();
        _items.Add(item);
        SaveManager.Instance.InventoryItems.Add(item);
        item.SetCurrentCount(amount);
        item.ItemSelected += _itemViewPopUp.SetView;
        slot.IsOccupied = true;
    }


    public void DeleteItem(InventoryItem item)
    {
        _items.Remove(item);
        SaveManager.Instance.InventoryItems.Remove(item);
        item.InventorySlot.IsOccupied = false;
        item.ItemSelected -= _itemViewPopUp.SetView;

        Destroy(item.gameObject);
    }

    public InventoryItem GetItem(ScriptableObject so)
    {
        return _items.FirstOrDefault(i => i.ItemSO == so);
    }

    private void UpdateGrid(LayoutGroup gridLayoutGroup)
    {
        gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
    }

    public bool ReduceItemAmount(ScriptableObject itemSo, int reduceValue)
    {
        var inventoryItem = _items.Find(i => i.ItemSO == itemSo);
        if (inventoryItem && inventoryItem.Count >= reduceValue)
        {
            inventoryItem.SetCurrentCount(inventoryItem.Count - reduceValue);
            if (inventoryItem.Count <= 0)
            {
                DeleteItem(inventoryItem);
            }

            return true;
        }

        return false;
    }
}


[Serializable]
public class InventoryData
{
    [SerializeField] private Item _itemSO;
    [SerializeField] private int _amount;


    public InventoryData(ScriptableObject itemSO, int amount)
    {
        _itemSO = itemSO as Item;
        _amount = amount;
    }

    public Item ItemSO => _itemSO;
    public int Amount => _amount;
}