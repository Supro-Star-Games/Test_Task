
using UnityEngine;


[CreateAssetMenu(fileName = "Cloth",menuName = "newItem/Cloth",order = 51)]

public class ClothItem : Item
{
    [SerializeField] private DefendType _defendType;
    [SerializeField] private int _armor;
    

    public DefendType DefendType => _defendType;
    public int Armor => _armor;
}
