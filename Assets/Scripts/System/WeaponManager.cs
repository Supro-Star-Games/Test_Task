using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private Character _enemy;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Button _pistolButton;
    [SerializeField] private Button _rifleButton;
    [SerializeField] private Button _fireButton;

    [SerializeField] private int _pistolDamage;
    [SerializeField] private int _rifleDamage;

    [SerializeField] private AmmoItem _pistolAmmo;
    [SerializeField] private int _pistolBulletCost;
    [SerializeField] private GameObject _pistolFrame;

    [SerializeField] private AmmoItem _rifleAmmo;
    [SerializeField] private int _rifleBulletCost;
    [SerializeField] private GameObject _rifleFrame;


    private WeaponType _currentWeapon;

    private void Awake()
    {
        _currentWeapon = WeaponType.Pistol;
    }

    private void OnEnable()
    {
        _pistolButton.onClick.AddListener(SelectPistol);
        _rifleButton.onClick.AddListener(SelectRifle);
        _fireButton.onClick.AddListener(Shoot);
    }

    private void OnDisable()
    {
        _pistolButton.onClick.RemoveListener(SelectPistol);
        _rifleButton.onClick.RemoveListener(SelectRifle);
        _fireButton.onClick.RemoveListener(Shoot);
    }

    private void SelectPistol()
    {
        _currentWeapon = WeaponType.Pistol;
        _pistolFrame.SetActive(true);
        _rifleFrame.SetActive(false);
    }

    private void SelectRifle()
    {
        _currentWeapon = WeaponType.Rifle;
        _rifleFrame.SetActive(true);
        _pistolFrame.SetActive(false);
    }

    private void Shoot()
    {
        switch (_currentWeapon)
        {
            case WeaponType.Pistol:
                if (_inventory.ReduceItemAmount(_pistolAmmo, _pistolBulletCost))
                {
                    _enemy.TakeDamage(_pistolDamage);
                    if (_enemy.CurrentHP <= 0)
                    {
                        break;
                    }

                    _player.TakeDamage(_enemy.GetCharacterDamage());
                }

                break;
            case WeaponType.Rifle:
                if (_inventory.ReduceItemAmount(_rifleAmmo, _rifleBulletCost))
                {
                    _enemy.TakeDamage(_rifleDamage);
                    if (_enemy.CurrentHP <= 0)
                    {
                        break;
                    }

                    _player.TakeDamage(_enemy.GetCharacterDamage());
                }

                break;
        }
    }
}

public enum WeaponType
{
    Pistol,
    Rifle
}