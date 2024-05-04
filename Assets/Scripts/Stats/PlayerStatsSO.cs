using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "new PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    /*
     Holds the Players Total Money
     and Holds the Players Stat Levels in a Dict<StatType, StatSO>
     */
    [field: SerializeField] public int Tokens { get; private set; } = 0;
    [field: SerializeField] public Dictionary<StatType, StatSO> Stats { get; private set; }

    public bool IsUpgradable(StatType type)
    {
        var stat = Stats[type];
        if (stat.Level == stat.MaxLevel) { return false; }
        if (stat.GetCost() > Tokens) { return false; }
        return true;
    }

    public void UpgradeStat(StatType type)
    {
        if (!IsUpgradable(type)) { return; }
        Stats[type].Upgrade();

    }

}
