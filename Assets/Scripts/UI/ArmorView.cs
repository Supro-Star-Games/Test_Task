using TMPro;
using UnityEngine;

public class ArmorView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _headArmorValue;
    [SerializeField] private TextMeshProUGUI _bodyArmorValue;
    [SerializeField] private PlayerCharacter _player;

    private void OnEnable()
    {
        _player.HeadArmorChanged += SetHeadArmor;
        _player.BodyArmorChanged += SetBodyArmor;
        _player.HeadArmorRemoved += OnRemovedHeadArmor;
        _player.BodyArmorRemoved += OnRemovedBodyArmor;
    }

    private void OnDisable()
    {
        _player.HeadArmorChanged -= SetHeadArmor;
        _player.BodyArmorChanged -= SetBodyArmor;
    }

    private void OnRemovedHeadArmor()
    {
        _headArmorValue.text = 0.ToString();
    }

    private void OnRemovedBodyArmor()
    {
        _bodyArmorValue.text = 0.ToString();
    }

    private void SetHeadArmor(int value)
    {
        _headArmorValue.text = value.ToString();
    }

    private void SetBodyArmor(int value)
    {
        _bodyArmorValue.text = value.ToString();
    }
}