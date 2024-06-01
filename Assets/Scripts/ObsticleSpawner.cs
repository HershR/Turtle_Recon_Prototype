using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum InteractableType
{
	Trash,
	Sharp,
	Oil,
	Wires,
	Tokens,
	Kelp,
	JellyFish,
	Dash
}

public class ObsticleSpawner : MonoBehaviour
{
	// Initialize Enviornment Generator var
	[SerializeField] public EnvironmentGenerator environmentGenerator;

	[SerializeField] public SerializedDictionary<InteractableType, int> InteractableWeights; // Serves no purpose anymore

	// Public dictionary mapping obsticle types types to game objects
	[SerializeField] public SerializedDictionary<InteractableType, GameObject> InteractableObjects;

	// Initialize current env var
	[SerializeField] public EnvironmentType CurrentEnviornment;

	// Initialize spawn weight dictionaries for ALL enviornments
	[SerializeField] public SerializedDictionary<InteractableType, int> NormalEnv;
	[SerializeField] public SerializedDictionary<InteractableType, int> OilEnv;
	[SerializeField] public SerializedDictionary<InteractableType, int> TrashEnv;
	[SerializeField] public SerializedDictionary<InteractableType, int> CoralEnv;

	// Create dictionary mapping enviornments to their respective spawn dicts
	[SerializeField] private SerializedDictionary<EnvironmentType, SerializedDictionary<InteractableType, int>> EnvWeightDict =
			new SerializedDictionary<EnvironmentType, SerializedDictionary<InteractableType, int>> { };

	// Initialize spawn rate upgrades from player
	[SerializeField] public StatSO tokenSpawnUpgrade;
	[SerializeField] public StatSO kelpSpawnUpgrade;
	[SerializeField] public StatSO jellyfishSpawnUpgrade;
	[SerializeField] public StatSO dashSpawnUpgrade;
	[SerializeField] public StatSO oilSpawnUpgrade;
	[SerializeField] public StatSO trashSpawnUpgrade;
	[SerializeField] public StatSO acidityUpgrade;

	float timer = 0f; // Spawn rate timer
	private int totalWeight = 0;
	// List of obsticles and their spawn weights (affected by player upgrades)
	private List<(InteractableType, int)> objWeights = new List<(InteractableType, int)>();

    private void Start()
	{
		// Get Current env
		CurrentEnviornment = environmentGenerator.CurrentEnvironmentToSpawn;
		// Populate Weight dictionaries based on current enviornemnt
		EnvWeightDict.Add(EnvironmentType.Normal, NormalEnv);
		EnvWeightDict.Add(EnvironmentType.OilField, OilEnv);
		EnvWeightDict.Add(EnvironmentType.CoralReef, CoralEnv);
		EnvWeightDict.Add(EnvironmentType.TrashField, TrashEnv);
		UpdateObsticleWeights();

		// Fill dict with env values (make ur own dict)
		UpdateObsticleWeights();
		Debug.Log("Total weight:" + totalWeight);
		Debug.Log("(Objects, weights): " + objWeights);
		for(int i = 0; i < objWeights.Count; i++)
        {
			Debug.Log(i + ": " + objWeights[i]);
        }
	}

	void Update()
	{
		if(CurrentEnviornment != environmentGenerator.CurrentEnvironmentToSpawn && environmentGenerator.CurrentEnvironmentToSpawn != EnvironmentType.Transition)
        {
			CurrentEnviornment = environmentGenerator.CurrentEnvironmentToSpawn;
			Debug.Log("New enviornemnt: " + CurrentEnviornment);
			UpdateObsticleWeights();
        }
		if (timer <= 3f)
		{
			timer += Time.deltaTime;
		}
		else
		{
			timer = 0;
			for(int i = 0; i < Random.Range(1, 4); i++)
            {
				int index = Random.Range(0, totalWeight);
				foreach ((InteractableType t, int w) in objWeights)
				{
					if (w > index)
					{
						Debug.Log("About to spawn a: " + InteractableObjects[t]);
						Debug.Log("at" + this.transform);
						GameObject new_obsticle = Instantiate(InteractableObjects[t], this.transform);
						new_obsticle.GetComponent<ObsticleController>().obsticle_type = t;
						Debug.Log("Spawned a " + t);
						break;
					}
				}
				Debug.Log("obsticle spawned");
			}
				
		}
	}
	// grab values from corresponding dict
	void UpdateObsticleWeights()
    {
		foreach ((InteractableType t, int i) in EnvWeightDict[CurrentEnviornment])
		{
			if (i > 0)
			{
				int weight = i;
                switch (t)
                {
					case InteractableType.Tokens:
						weight *= 1 + tokenSpawnUpgrade.Level;
						break;
					case InteractableType.JellyFish:
						weight *= 1 + jellyfishSpawnUpgrade.Level * acidityUpgrade.Level;
						break;
					case InteractableType.Kelp:
						weight *= 1 + kelpSpawnUpgrade.Level * acidityUpgrade.Level;
						break;
					case InteractableType.Trash:
						weight /= 1 + trashSpawnUpgrade.Level;
						break;
					case InteractableType.Oil:
						weight /= 1 + oilSpawnUpgrade.Level;
						break;
                }
				objWeights.Add((t, weight + totalWeight));
				totalWeight += weight;
			}
		}
		Debug.Log("New Obsticle Spawn Rates: " + string.Join(", ", objWeights));
	}
	// THIS IS WHERE ALL THE ENV DICTIONARIES WILL GO IF WE MAKE EM PRIVATE!!!!

 
}