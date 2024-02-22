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
    [SerializeField] private ItemViewPopUp itemViewPopUp;

    private GridLayoutGroup _grid;
    private List<InventorySlot> _slots;
    private List<InventoryItem> _items = new();
    
    private void OnEnable()
    {
        itemViewPopUp.DeleteItem += DeleteItem;
    }

    private void OnDisable()
    {
        itemViewPopUp.DeleteItem -= DeleteItem;
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
            var _slotRect = slot.GetComponent<RectTransform>();
            var newItem = Instantiate(_itemTemplate, _contentTransform);
            newItem.GetComponent<RectTransform>().anchoredPosition = _slotRect.anchoredPosition;

            switch (data)
            {
                case ClothItem _clothItem:
                    var _clothInventoryItem = newItem.AddComponent<ClothInventoryItem>();
                    _clothInventoryItem.SetSo(_clothItem);
                    _clothInventoryItem.InventorySlot = slot;
                    slot.IsOccupied = true;
                    _clothInventoryItem.Initialize();
                    _items.Add(_clothInventoryItem);
                    SaveManager.Instance.InventoryItems.Add(_clothInventoryItem);
                    _clothInventoryItem.SetCurrentCount(count);
                    _clothInventoryItem.ItemSelected += itemViewPopUp.SetView;
                    break;
                case AmmoItem _ammoItem:
                    var _ammoInventoryItem = newItem.AddComponent<AmmoInventoryItem>();
                    _ammoInventoryItem.SetSo(_ammoItem);
                    _ammoInventoryItem.InventorySlot = slot;
                    slot.IsOccupied = true;
                    _ammoInventoryItem.Initialize();
                    _items.Add(_ammoInventoryItem);
                    SaveManager.Instance.InventoryItems.Add(_ammoInventoryItem);
                    _ammoInventoryItem.SetCurrentCount(count);
                    _ammoInventoryItem.ItemSelected += itemViewPopUp.SetView;
                    break;
                case MedKitItem _medKitItem:
                    var _medKitInventoryItem = newItem.AddComponent<MedKitInventoryItem>();
                    _medKitInventoryItem.SetSo(_medKitItem);
                    slot.IsOccupied = true;
                    _medKitInventoryItem.InventorySlot = slot;
                    _medKitInventoryItem.Initialize();
                    _items.Add(_medKitInventoryItem);
                    SaveManager.Instance.InventoryItems.Add(_medKitInventoryItem);
                    _medKitInventoryItem.SetCurrentCount(count);
                    _medKitInventoryItem.ItemSelected += itemViewPopUp.SetView;
                    break;
            }

            slot.IsOccupied = true;
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }
    
    public void DeleteItem(InventoryItem item)
    {
        _items.Remove(item);
        SaveManager.Instance.InventoryItems.Remove(item);
        item.ItemSelected -= itemViewPopUp.SetView;
        _slots.Find(slot => slot == item.InventorySlot).IsOccupied = false;

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