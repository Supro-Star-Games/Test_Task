using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct GameData
{
    public List<InventoryData> Items;
    public int PlayerHp;
    public int EnemyHP;
    public ScriptableObject HeadItem;
    public ScriptableObject BodyItem;
}