using UnityEngine;
using UnityEngine.UI;

public class DefeatPopUp : MonoBehaviour
{
    [SerializeField] private Button _playAgain;

    private void OnEnable()
    {
        _playAgain.onClick.AddListener(HidePopUp);
    }

    private void OnDisable()
    {
        _playAgain.onClick.RemoveListener(HidePopUp);
    }

    public void ShowPopUp()
    {
        gameObject.SetActive(true);
    }

    private void HidePopUp()
    {
        BootStrapper.Instance.RestartGame();
        gameObject.SetActive(false);
    }
}