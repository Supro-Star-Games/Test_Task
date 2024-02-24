public class MedKitInventoryItem : InventoryItem
{
    private MedKitItem _medKitSO;
    private int _hpRestore;
    public int HpRestore => _hpRestore;

    public override void Initialize()
    {
        _sprite = _medKitSO.Sprite;
        _name = _medKitSO.Name;
        _weight = _medKitSO.Weight;
        _maxStack = _medKitSO.MaxStack;
        _hpRestore = _medKitSO.HpRestore;
        base.Initialize();
    }

    protected override void Selected()
    {
        ItemSelected.Invoke(this);
    }
    
    public override void SetSO<T>(T itemSo)
    {
        base.SetSO(itemSo);
        _medKitSO = itemSo as MedKitItem;
    }
}