using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string _saveFileName;
    [SerializeField] private string _savePath;

    [SerializeField] public List<InventoryData> DataItems = new();
    public List<InventoryItem> InventoryItems = new();

    public ScriptableObject HeadItem;
    public ScriptableObject BodyItem;

    public int PlayerHp;
    public int EnemyHp;

    public bool isDataLoaded;


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

    public void Initialize()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _savePath = Path.Combine(Application.persistentDataPath, _saveFileName);
        }
        else
        {
            _savePath = Path.Combine(Application.dataPath, _saveFileName);
        }

        LoadFromFile();
    }

    public void ClearSaveData()
    {
        DataItems.Clear();
        InventoryItems.Clear();
        HeadItem = null;
        BodyItem = null;

        File.Delete(_savePath);
    }

    public void SaveToFile()
    {
        DataItems.Clear();
        foreach (var item in InventoryItems)
        {
            DataItems.Add(new InventoryData(item.ItemSO, item.Count));
        }

        GameData data = new GameData() { Items = DataItems, EnemyHP = EnemyHp, PlayerHp = PlayerHp, BodyItem = BodyItem, HeadItem = HeadItem };
        string serializeData = JsonUtility.ToJson(data);

        try
        {
            File.WriteAllText(_savePath, serializeData);
        }
        catch (Exception e)
        {
            Debug.Log("{GameLog} => [SaveManager] - (<color=Red>Error</color>) - SaveToFileError" + e);
            throw;
        }
    }

    private void LoadFromFile()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("{GameLog} => [SaveManger] - LoadToFile -> FileNotFound");
            return;
        }

        string jsonData = File.ReadAllText(_savePath);
        GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);

        DataItems = loadedData.Items;
        PlayerHp = loadedData.PlayerHp;
        EnemyHp = loadedData.EnemyHP;
        HeadItem = loadedData.HeadItem;
        BodyItem = loadedData.BodyItem;

        isDataLoaded = true;
    }

    public void SaveEquippedItem(ScriptableObject item, bool isHeadSlot)
    {
        if (isHeadSlot)
        {
            HeadItem = item;
        }
        else
        {
            BodyItem = item;
        }
    }
}