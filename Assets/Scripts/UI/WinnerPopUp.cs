using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WinnerPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Inventory _inventory;

    [SerializeField] private List<InventoryData> _rewards;

    private Item _rewardItem;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(HidePopUp);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(HidePopUp);
    }

    public void ShowPopUp()
    {
        var currentReward = RandomizeReward();
        _itemName.text = currentReward.ItemSO.Name;
        _itemIcon.sprite = currentReward.ItemSO.Sprite;
        if (currentReward.Amount == 1)
        {
            _amount.text = "";
        }
        else
        {
            _amount.text = currentReward.Amount.ToString();
        }

        _inventory.AddItem(currentReward.ItemSO, currentReward.Amount);
        gameObject.SetActive(true);
    }

    private void HidePopUp()
    {
        BootStrapper.Instance.ContinueGame();
        gameObject.SetActive(false);
    }

    private InventoryData RandomizeReward()
    {
        var randomIndex = Random.Range(0, _rewards.Count);
        return _rewards[randomIndex];
    }
}