using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpView : MonoBehaviour
{
    [SerializeField] private Image _hpImage;
    [SerializeField] private TextMeshProUGUI _hpValue;
    [SerializeField] private Character _character;

    private void OnEnable()
    {
        _character.HpChanged += SetHp;
    }

    private void OnDisable()
    {
        _character.HpChanged -= SetHp;
    }

    private void SetHp(int hp, float fill)
    {
        _hpImage.fillAmount = fill;
        _hpValue.text = hp.ToString();
    }
}