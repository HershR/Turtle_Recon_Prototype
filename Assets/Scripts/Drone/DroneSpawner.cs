using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private StatSO droneSpawnRate;
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private float baseSpawnRate = 60f;
    [SerializeField] private float levelModifier = 5f;
    
    [Header("Timers")]
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnTimer;
    private GameObject activeDrone;


    private void Start()
    {
        spawnRate = baseSpawnRate - droneSpawnRate.Level * levelModifier;
        spawnRate = Mathf.Max(10f, spawnRate);
        spawnTimer = Random.Range(spawnTimer - 5f, spawnTimer + 5f);
    }

    private void Update()
    {
        if(activeDrone != null) { return; }
        if(spawnTimer <= 0)
        {
            float x = Random.value > 0.5f ? -20 : 20;
            float y = Random.Range(5, 10);
            Vector3 spawnPosition = new Vector3(x, y, transform.position.z);
            activeDrone = Instantiate(dronePrefab, spawnPosition, Quaternion.identity);
            spawnTimer = Random.Range(spawnRate - 5f, spawnRate + 5f);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }


}
