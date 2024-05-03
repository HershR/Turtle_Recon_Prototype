using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/", order = 1)]
public class StatsSO : ScriptableObject
{
    [field: SerializeField] public int HealthLvl { get; private set; } = 0;
    [field: SerializeField] public int SpeedLvl { get; private set; } = 0;
    [field: SerializeField] public int DashLvl { get; private set; } = 0;
    [field: SerializeField] public int ParryDashRefillLvl { get; private set; } = 0;
    
    [field: SerializeField] public int Food1SpawnLvl { get; private set; } = 0;
    [field: SerializeField] public int Food2SpawnLvl { get; private set; } = 0;
    [field: SerializeField] public int DashRefillSpawnLvl { get; private set; } = 0;
    [field: SerializeField] public int TokenSpawnLvl { get; private set; } = 0;
    [field: SerializeField] public int DroneCollectionLvl { get; private set; } = 0;
    [field: SerializeField] public int DroneSpawnLvl { get; private set; } = 0;

    [field: SerializeField] public int OilResearchLvl { get; private set; } = 0;
    [field: SerializeField] public int TrashResearchLvl { get; private set; } = 0;
    [field: SerializeField] public int AcidityResearchLvl { get; private set; } = 0;


}
