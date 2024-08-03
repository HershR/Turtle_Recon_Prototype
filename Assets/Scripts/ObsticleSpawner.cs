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
	[SerializeField] public GameManager gameManager;

	// Initialize Enviornment Generator var
	[SerializeField] private EnvironmentGenerator environmentGenerator;

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
	[SerializeField]
	private SerializedDictionary<EnvironmentType, SerializedDictionary<InteractableType, int>> EnvWeightDict =
			new SerializedDictionary<EnvironmentType, SerializedDictionary<InteractableType, int>> { };

	// Initialize spawn rate upgrades from player
	[SerializeField] private StatSO tokenSpawnUpgrade;
	[SerializeField] private StatSO kelpSpawnUpgrade;
	[SerializeField] private StatSO jellyfishSpawnUpgrade;
	[SerializeField] private StatSO dashSpawnUpgrade;
	[SerializeField] private StatSO oilSpawnUpgrade;
	[SerializeField] private StatSO trashSpawnUpgrade;
	[SerializeField] private StatSO acidityUpgrade;

	[SerializeField] private PlayerController player;

	private float minHeight;
	private float maxHeight;
	private float minWidth;
	private float maxWidth;

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

		// Fill dict with env values (make ur own dict)
		UpdateObsticleWeights();
		Debug.Log("Total weight:" + totalWeight);
		Debug.Log("(Objects, weights): " + objWeights);
		for (int i = 0; i < objWeights.Count; i++)
		{
			Debug.Log(i + ": " + objWeights[i]);
		}
		float z = Camera.main.transform.position.z;
		// Bottom-left corner
		Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z));
		// Top-right corner
		Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, z));

		minHeight = Mathf.Max(bottomLeft.y, -6f);
		maxHeight = topRight.y;
		minWidth = bottomLeft.x;
		maxWidth = topRight.x;

		// Get player
		player = gameManager.player;
	}

	void Update()
	{
		if (CurrentEnviornment != environmentGenerator.CurrentEnvironmentToSpawn && environmentGenerator.CurrentEnvironmentToSpawn != EnvironmentType.Transition)
		{
			CurrentEnviornment = environmentGenerator.CurrentEnvironmentToSpawn;
			Debug.Log("New enviornemnt: " + CurrentEnviornment);
			UpdateObsticleWeights();
		}
		if (timer <= 3f / (1 + gameManager.gameTimeDelta / 60))
		{
			timer += Time.deltaTime;
		}
		else
		{
			timer = 0;
			for (int i = 0; i < Random.Range(1, 4); i++)
			{
				int index = Random.Range(0, totalWeight);
				foreach ((InteractableType t, int w) in objWeights)
				{
					if (w > index)
					{
						//Debug.Log("About to spawn a: " + InteractableObjects[t] + " at " + this.transform.position);
						GameObject new_obsticle = Instantiate(InteractableObjects[t], transform);
						float x = Random.Range(minWidth, maxWidth);
						float y = Random.Range(minHeight, maxHeight);
						if(i % 2 == 0)
                        {
							x = player.gameObject.transform.position.x;
							y = player.gameObject.transform.position.y;

						}
						Vector3 start_position = new Vector3(x, y, new_obsticle.transform.position.z);
						new_obsticle.transform.position = start_position;
                        ObsticleController obsticleController = new_obsticle.GetComponent<ObsticleController>();
                        obsticleController.obsticle_type = t;
						obsticleController.speed += environmentGenerator.GetSpeed();
						//Debug.Log("env speed: " + environmentGenerator.GetSpeed());
						//Debug.Log("Spawned a " + t);
						break;
					}
				}
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
		//Debug.Log("New Obsticle Spawn Rates: " + string.Join(", ", objWeights));
	}

	// THIS IS WHERE ALL THE ENV DICTIONARIES WILL GO IF WE MAKE EM PRIVATE!!!!


}