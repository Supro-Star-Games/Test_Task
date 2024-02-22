using UnityEngine;

[CreateAssetMenu(fileName = "newMedkit", menuName = "newItem/Medkit", order = 51)]
public class MedKitItem : Item
{
    [SerializeField] private int _hpRestore;
    
    public int HpRestore => _hpRestore;
}