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
    
    [SerializedDictionary("Environment Type", "Weights")]    
    [SerializeField] private SerializedDictionary<EnvironmentType, float> environmentWeights;
    
    [SerializedDictionary("Environment Type", "Prefabs")]
    [SerializeField] private SerializedDictionary<EnvironmentType, List<GameObject>> environments;

    [SerializeField] private EnvironmentType currentEnvironment;

    //[SerializeField] private Queue<GameObject> environmentSpawnQueue;

    [SerializeField] private float environmentSpeed;
    [SerializeField] private float environmentTargetSpeed;
    private float speedDelta = 1f;

    [SerializeField] private int minEnvironmentTime; //min amount to spawn an env
    [SerializeField] private int maxEnvironmentTime; //max amount to spawn an env
    [SerializeField] private float environmentTimer; //amount left for current env    

    [SerializeField] private PostProcessingManager postProcessingManager;

    private void Awake()
    {
        InitEnvironmentWeights();
    }

    private void Start()
    {
        currentEnvironment = EnvironmentType.Normal;
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
            var oldEnv = currentEnvironment;
            if (currentEnvironment != EnvironmentType.Transition)
            {
                currentEnvironment = EnvironmentType.Transition;
                environmentTimer = Random.Range(minEnvironmentTime / 3, maxEnvironmentTime / 3);
                postProcessingManager.SwitchEnvironment(EnvironmentType.Normal);
            }
            else
            {
                float weight = Random.Range(0, environmentWeights.Values.Sum());
                foreach (var pair in environmentWeights)
                {
                    weight -= pair.Value;
                    if (weight <= 0f)
                    {
                        currentEnvironment = pair.Key;
                        break;
                    }
                }
                environmentTimer = Random.Range(minEnvironmentTime, maxEnvironmentTime);
                postProcessingManager.SwitchEnvironment(currentEnvironment);
            }
            Debug.Log($"Change Env from {oldEnv} to {currentEnvironment}");
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
        
        //60% chance to get a differnt env
        float baseWeight = 90f / (Enum.GetValues(typeof(EnvironmentType)).Length - 2); //exclude transition and basic

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
        int index = Random.Range(0, environments[currentEnvironment].Count);
        nextSpawn = environments[currentEnvironment][index];
        GameObject spawned = Instantiate(nextSpawn, transform.position, Quaternion.identity);
        EnvironmentController environmentController = spawned.GetComponent<EnvironmentController>();
        environmentController.Init(this);
    }

    public float GetSpeed()
    {
        return environmentSpeed;
    }
}
