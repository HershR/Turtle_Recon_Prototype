using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public enum EnvironmentType { Normal, OilField, TrashField, CoralReef, Transition }
public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStats;
    [SerializedDictionary("Environment Type", "Prefabs")]
    [SerializeField] private SerializedDictionary<EnvironmentType, float> environmentWeights;
    [SerializeField] private SerializedDictionary<EnvironmentType, List<GameObject>> environments;

    [field: SerializeField] public EnvironmentType CurrentEnvironment;

    //[SerializeField] private Queue<GameObject> environmentSpawnQueue;

    [SerializeField] private float environmentSpeed;
    [SerializeField] private float environmentTargetSpeed;
    private float speedDelta = 1f;

    [SerializeField] private int minEnvironmentTime; //min amount to spawn an env
    [SerializeField] private int maxEnvironmentTime; //max amount to spawn an env
    [SerializeField] private float environmentTimer; //amount left for current env    

    private void Awake()
    {
        InitEnvironmentWeights();
    }

    private void Start()
    {
        CurrentEnvironment = EnvironmentType.Normal;
        environmentTargetSpeed = environmentSpeed;
        environmentTimer = Random.Range(minEnvironmentTime, maxEnvironmentTime);
        Spawn();
    }

    private void Update()
    {
        if (environmentSpeed < environmentTargetSpeed)
        {
            var newSpeed = environmentSpeed + speedDelta * Time.deltaTime;
            environmentSpeed = Mathf.Min(environmentTargetSpeed, newSpeed);
        }
        if (environmentTimer < 0.0f)
        {
            var oldEnv = CurrentEnvironment;
            if (CurrentEnvironment != EnvironmentType.Transition)
            {
                CurrentEnvironment = EnvironmentType.Transition;
                environmentTimer = Random.Range(minEnvironmentTime / 3, maxEnvironmentTime / 3);
            }
            else
            {
                float weight = Random.Range(0, environmentWeights.Values.Sum());
                foreach (var pair in environmentWeights)
                {
                    weight -= pair.Value;
                    if (weight <= 0f)
                    {
                        CurrentEnvironment = pair.Key;
                        break;
                    }
                }
                environmentTimer = Random.Range(minEnvironmentTime, maxEnvironmentTime);
            }
            Debug.Log($"Change Env from {oldEnv} to {CurrentEnvironment}");
        }
        else
        {
            environmentTimer -= Time.deltaTime;
        }
    }

    private void InitEnvironmentWeights()
    {
        environmentWeights = new SerializedDictionary<EnvironmentType, float>();
        float totalWeight = 100f;
        float baseWeight = 100f / (Enum.GetValues(typeof(EnvironmentType)).Length - 1); //exclude transition

        float oilWeight = baseWeight - (baseWeight * playerStats.GetStat(StatType.OilResearch).Level / playerStats.GetStat(StatType.OilResearch).MaxLevel);
        float trashWeight = baseWeight - (baseWeight * playerStats.GetStat(StatType.TrashResearch).Level / playerStats.GetStat(StatType.TrashResearch).MaxLevel);
        float coralWeight = baseWeight - (baseWeight * playerStats.GetStat(StatType.AcidityResearch).Level / playerStats.GetStat(StatType.AcidityResearch).MaxLevel);
        environmentWeights.Add(EnvironmentType.OilField, oilWeight);
        environmentWeights.Add(EnvironmentType.TrashField, trashWeight);
        environmentWeights.Add(EnvironmentType.CoralReef, coralWeight);
        environmentWeights.Add(EnvironmentType.Normal, totalWeight - (oilWeight + trashWeight + coralWeight));
    }

    public void Spawn()
    {
        GameObject nextSpawn;
        int index = Random.Range(0, environments[CurrentEnvironment].Count);
        nextSpawn = environments[CurrentEnvironment][index];
        GameObject spawned = Instantiate(nextSpawn, transform.position, Quaternion.identity);
        EnvironmentController environmentController = spawned.GetComponent<EnvironmentController>();
        environmentController.Init(this);
    }

    public float GetSpeed()
    {
        return environmentSpeed;
    }
}
