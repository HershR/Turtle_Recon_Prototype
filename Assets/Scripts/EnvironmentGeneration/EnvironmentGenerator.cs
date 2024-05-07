using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using Random = UnityEngine.Random;

public enum EnvironmentType { Normal, OilField, TrashField, CoralReef, Transition }
public class EnvironmentGenerator : MonoBehaviour
{

    [SerializedDictionary("Environment Type", "Prefabs")]
    [SerializeField] private SerializedDictionary<EnvironmentType, List<GameObject>> environments;

    [field: SerializeField] public EnvironmentType CurrentEnvironment;

    //[SerializeField] private Queue<GameObject> environmentSpawnQueue;

    [SerializeField] private float environmentSpeed;
    [SerializeField] private float environmentTargetSpeed;
    private float speedDelta = 1f;

    [SerializeField] private int minEnvironmentSpawnCount; //min amount to spawn an env
    [SerializeField] private int maxEnvironmentSpawnCount; //max amount to spawn an env
    [SerializeField] private float environmentSpawnCount; //amount left for current env    
    private void Start()
    {
        CurrentEnvironment = EnvironmentType.Normal;
        environmentTargetSpeed = environmentSpeed;
        Spawn();
    }

    private void Update()
    {
        if(environmentSpeed < environmentTargetSpeed)
        {
            var newSpeed = environmentSpeed + speedDelta * Time.deltaTime;
            environmentSpeed = Mathf.Min(environmentTargetSpeed, newSpeed);
        }
        if (environmentSpawnCount < 1)
        {
            var oldEnv = CurrentEnvironment;
            if (CurrentEnvironment != EnvironmentType.Transition)
            {
                CurrentEnvironment = EnvironmentType.Transition;
                environmentSpawnCount = Random.Range(minEnvironmentSpawnCount / 3, maxEnvironmentSpawnCount / 3);
            }
            else
            {
                CurrentEnvironment = (EnvironmentType)Random.Range(0, Enum.GetValues(typeof(EnvironmentType)).Length - 1);
                environmentSpawnCount = Random.Range(minEnvironmentSpawnCount, maxEnvironmentSpawnCount);
            }
            Debug.Log($"Change Env from {oldEnv} to {CurrentEnvironment}");
        }
    }
    public void Spawn()
    {
        Debug.Log("Spawn");
        GameObject nextSpawn;
        int index = Random.Range(0, environments[CurrentEnvironment].Count);
        nextSpawn = environments[CurrentEnvironment][index];
        GameObject spawned = Instantiate(nextSpawn, transform.position, Quaternion.identity);
        EnvironmentController environmentController = spawned.GetComponent<EnvironmentController>();
        environmentController.Init(this);
        environmentSpawnCount -= 1;
    }

    public float GetSpeed()
    {
        return environmentSpeed;
    }
}
