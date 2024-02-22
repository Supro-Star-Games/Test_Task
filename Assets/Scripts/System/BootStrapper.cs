using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    public static BootStrapper Instance;

    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private Character _character;
    [SerializeField] private WinnerPopUp _winnerPop;
    [SerializeField] private DefeatPopUp _defeatPop;
    [SerializeField] private Inventory _inventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SaveManager.Instance.Initialize();
        if (SaveManager.Instance.isDataLoaded)
        {
            _inventory.LoadData(SaveManager.Instance.DataItems);
            _player.LoadData(SaveManager.Instance.PlayerHp);
            _character.LoadData(SaveManager.Instance.EnemyHp);
        }
        else
        {
            _inventory.Initialize();
            _player.Initialize();
            _character.Initialize();
        }
    }

    private void OnEnable()
    {
        _character.PlayerWon += _winnerPop.ShowPopUp;
        _player.PlayerDefeated += _defeatPop.ShowPopUp;
    }

    private void OnDisable()
    {
        _character.PlayerWon -= _winnerPop.ShowPopUp;
        _player.PlayerDefeated -= _defeatPop.ShowPopUp;
        SaveManager.Instance.SaveToFile();
    }

    public void RestartGame()
    {
        SaveManager.Instance.ClearSaveData();
        _player.Initialize();
        _character.Initialize();
        _inventory.Initialize();
    }

    public void ContinueGame()
    {
        _character.Initialize();
    }
}