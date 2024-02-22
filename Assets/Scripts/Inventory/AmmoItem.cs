
using UnityEngine;

[CreateAssetMenu(fileName = "newAmmo", menuName = "newItem/Ammo", order = 51)]

public class AmmoItem : Item
{
    [SerializeField] private AmmoType _ammoType;

    public AmmoType AmmoType => _ammoType;
}