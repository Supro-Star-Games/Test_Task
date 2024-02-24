public class ClothInventoryItem : InventoryItem
{
    private ClothItem _clothSO;
    private DefendType _defendType;
    private int _armor;
    public DefendType DefendType => _defendType;
    public int Armor => _armor;

    public override void Initialize()
    {
        _sprite = _clothSO.Sprite;
        _name = _clothSO.Name;
        _weight = _clothSO.Weight;
        _maxStack = _clothSO.MaxStack;
        _defendType = _clothSO.DefendType;
        _armor = _clothSO.Armor;
        base.Initialize();
    }

    protected override void Selected()
    {
        ItemSelected.Invoke(this);
    }

    public override void SetSO<T>(T itemSo)
    {
        base.SetSO(itemSo);
        _clothSO = itemSo as ClothItem;
    }
}