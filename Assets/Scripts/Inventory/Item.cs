using UnityEngine;


public class Item : ScriptableObject
{
    [SerializeField] protected Sprite itemSprite;
    [SerializeField] protected string _itemName;
    [SerializeField] protected float _weight;
    [SerializeField] protected int _maxStack;

    public float Weight => _weight;
    public int MaxStack => _maxStack;
    public string Name => _itemName;
    public Sprite Sprite => itemSprite;
}