using UnityEngine;


[CreateAssetMenu(fileName = "new Stat", menuName = "ScriptableObjects/Stats")]
public class StatSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Level { get; private set; } = 0;
    [field: SerializeField] public int MaxLevel { get; private set; } = 5;
    [field: SerializeField] public Sprite Icon { get; private set; }

    [SerializeField]
    [TextArea]
    [Tooltip("Add to array if Description of Stat changes with each upgrade. Main use for Research Upgrades")]
    private string[] description = new string[1];

    [SerializeField]
    [Tooltip("Cost to upgrade Stat at each level under MaxLevel")]
    private int[] levelCosts = new int[4];

    public void Upgrade()
    {
        if (Level == MaxLevel) { return; }
        Level += 1;
    }
    public int GetCost()
    {
        if (Level >= MaxLevel) { return -1; }
        return levelCosts[Mathf.Min(levelCosts.Length, Level)];
    }

    public string GetDescription()
    {
        return description[Mathf.Min(description.Length, Level)];
    }
}


//Differnt type of upgradeable stats
public enum StatType { Health, Speed, Dash, DashRefillParry, Food1Spawn, Food2Spawn, DashRefillSpawn, TokenSpawn, DroneCollectionRate, DroneSpawn, OilResearch, TrashResearch, AcidityResearch };
