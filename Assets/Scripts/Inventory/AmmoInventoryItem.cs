public class AmmoInventoryItem : InventoryItem
{
    private AmmoItem _ammoSO;
    private AmmoType _ammoType;

    public AmmoType AmmoType => _ammoType;

    public override void Initialize()
    {
        _sprite = _ammoSO.Sprite;
        _name = _ammoSO.Name;
        _weight = _ammoSO.Weight;
        _maxStack = _ammoSO.MaxStack;
        _ammoType = _ammoSO.AmmoType;
        base.Initialize();
    }

    protected override void Selected()
    {
        ItemSelected.Invoke(this);
    }


    public override void SetSO<T>(T itemSo)
    {
        base.SetSO(itemSo);
        _ammoSO = itemSo as AmmoItem;
    }
}