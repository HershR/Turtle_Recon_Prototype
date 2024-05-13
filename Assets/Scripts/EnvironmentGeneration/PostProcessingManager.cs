using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using AYellowpaper.SerializedCollections;
public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] private AYellowpaper.SerializedCollections.SerializedDictionary<EnvironmentType, Volume> environmentVolumes;
    [SerializeField] private EnvironmentType currentEnvironment = EnvironmentType.Normal;
    [SerializeField] private EnvironmentType nextEnvironment = EnvironmentType.Normal;
    private float fadeRate = 0.5f;

    void Start()
    {
        environmentVolumes[EnvironmentType.Normal].weight = 1;
    }

    private void Update()
    {
        if (environmentVolumes[nextEnvironment] != environmentVolumes[currentEnvironment])
        {
            fadeToVolume();
        }
    }

    public void SwitchEnvironment(EnvironmentType type)
    {
        if (type == currentEnvironment) { return; }
        nextEnvironment = type;

    }

    private void fadeToVolume()
    {
        if(environmentVolumes[nextEnvironment].weight < 1f)
        {
            environmentVolumes[nextEnvironment].weight += Time.deltaTime * fadeRate;
            environmentVolumes[currentEnvironment].weight -= Time.deltaTime * fadeRate;
        }
        else
        {
            environmentVolumes[nextEnvironment].weight = 1f;
            environmentVolumes[currentEnvironment].weight = 0f;
            currentEnvironment = nextEnvironment;

        }
    }
}
