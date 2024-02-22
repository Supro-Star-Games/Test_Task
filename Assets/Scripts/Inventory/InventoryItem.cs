using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryItem : DragInventoryItem
{
    private Button _itemButton;
    private TextMeshProUGUI _itemCount;

    protected Sprite _sprite;
    protected string _name;
    protected float _weight;
    protected int _maxStack;
    protected int _currenCount;

    public Action<InventoryItem> ItemSelected;
    public ScriptableObject ItemSO { get; protected set; }
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public float Weight => _weight;
    public int Count => _currenCount;

    public int MaxStack => _maxStack;

    public override void Initialize()
    {
        base.Initialize();
        var button = GetComponentInChildren<Button>();
        button.image.sprite = _sprite;
        _itemCount = button.GetComponentInChildren<TextMeshProUGUI>();
        _itemButton = button;
        button.onClick.AddListener(Selected);
    }

    private void OnDestroy()
    {
        _itemButton.onClick.RemoveListener(Selected);
    }


    protected virtual void Selected()
    {
        ItemSelected.Invoke(this);
    }

    public void SetCurrentCount(int count, bool setMax = false)
    {
        if (count <= _maxStack)
        {
            _currenCount = count;
        }
        else
        {
            _currenCount = _maxStack;
        }

        if (setMax)
        {
            _currenCount = _maxStack;
        }

        if (_currenCount == 1)
        {
            _itemCount.text = "";
        }
        else
        {
            _itemCount.text = _currenCount.ToString();
        }

        SaveManager.Instance.InventoryItems.Find(i => i == this)._currenCount = _currenCount;
    }
}