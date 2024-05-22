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
	[SerializeField] public EnvironmentGenerator environmentGenerator;
	[SerializeField] public SerializedDictionary<InteractableType, int> InteractableWeights; // Serves no purpose anymore
	[SerializeField] public SerializedDictionary<InteractableType, GameObject> InteractableObjects;
	
	[SerializeField] public EnvironmentType CurrentEnviornment;

	[SerializeField] public SerializedDictionary<InteractableType, int> NormalEnv;
	[SerializeField] public SerializedDictionary<InteractableType, int> OilEnv;
	[SerializeField] public SerializedDictionary<InteractableType, int> TrashEnv;
	[SerializeField] public SerializedDictionary<InteractableType, int> CoralEnv;
	[SerializeField] private SerializedDictionary<EnvironmentType, SerializedDictionary<InteractableType, int>> EnvWeightDict =
			new SerializedDictionary<EnvironmentType, SerializedDictionary<InteractableType, int>> { };


	// We get env passed in
	// From that enum, we get 

	float timer = 0f;
	private int totalWeight = 0;
	private List<(InteractableType, int)> objWeights = new List<(InteractableType, int)>{};

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
				objWeights.Add((t, i + totalWeight));
				totalWeight += i;
			}
		}
		Debug.Log("New Obsticle Spawn Rates: " + string.Join(", ", objWeights));
	}
	// THIS IS WHERE ALL THE ENV DICTIONARIES WILL GO IF WE MAKE EM PRIVATE!!!!
 
}