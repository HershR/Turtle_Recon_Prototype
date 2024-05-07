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

    [SerializeField] private int minEnvironmentTime; //min amount to spawn an env
    [SerializeField] private int maxEnvironmentTime; //max amount to spawn an env
    [SerializeField] private float environmentTimer; //amount left for current env    
    private void Start()
    {
        CurrentEnvironment = EnvironmentType.Normal;
        environmentTargetSpeed = environmentSpeed;
        environmentTimer = Random.Range(minEnvironmentTime, maxEnvironmentTime);
        Spawn();
    }

    private void Update()
    {
        if(environmentSpeed < environmentTargetSpeed)
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
                CurrentEnvironment = (EnvironmentType)Random.Range(0, Enum.GetValues(typeof(EnvironmentType)).Length - 1);
                environmentTimer = Random.Range(minEnvironmentTime, maxEnvironmentTime);
            }
            Debug.Log($"Change Env from {oldEnv} to {CurrentEnvironment}");
        }
        else
        {
            environmentTimer -= Time.deltaTime;
        }
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
