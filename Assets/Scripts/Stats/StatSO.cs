using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Stat", menuName = "ScriptableObjects/Stats")]
public class StatSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Level { get; private set; } = 0;
    [field: SerializeField] public int MaxLevel { get; private set; } = 5;
    [field: SerializeField] public Sprite Icon { get; private set; }

    [SerializeField] [TextArea] [Tooltip("Add to array if Description of Stat changes with each upgrade. Main use for Research Upgrades")]
    private string[] description = new string[1];
    
    [SerializeField] [Tooltip("Cost to upgrade Stat at each level under MaxLevel")]
    private int[] levelCosts = new int[4];

    public void Upgrade()
    {
        if(Level == MaxLevel) { return; }
        Level += 1;
    }
    public int GetCost()
    {
        if(Level >= MaxLevel) { return -1; }
        return levelCosts[Level];
    }

    public string GetDescription()
    {
        return description[Mathf.Min(description.Length, Level)];
    }
}

[CreateAssetMenu(fileName = "new PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    /*
     Holds the Players Total Money
     and Holds the Players Stat Levels in a Dict<StatType, StatSO>
     */
    [field: SerializeField] public int Tokens { get; private set; } = 0;
    [field: SerializeField] public SerializedDictionary<StatType, StatSO> Stats { get; private set; }

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
//Differnt type of upgradeable stats
public enum StatType { Health, Speed, Dash, DashRefillParry, Food1Spawn, Food2S, DashRefillSpawn, TokenSpawn, DroneCollectionRate, DroneSpawn, OilResearch, TrashResearch, AcidityResearch };
