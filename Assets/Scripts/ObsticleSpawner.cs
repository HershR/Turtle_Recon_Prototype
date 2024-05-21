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
	[SerializedDictionary("Interactable", "Weight")]
	[SerializeField] public SerializedDictionary<InteractableType, int> InteractableWeights;
	[SerializeField] public SerializedDictionary<InteractableType, GameObject> InteractableObjects;

	
	// We get env passed in
	// From that enum, we get 

	float timer = 0f;
	private int totalWeight = 0;
	private List<(InteractableType, int)> objWeights = new List<(InteractableType, int)>{};

	private void Start()
	{
		foreach((InteractableType t, int i) in InteractableWeights)
        {
			if(i > 0)
            {
				objWeights.Add((t, i + totalWeight));
				totalWeight += i;
			}
        }
		Debug.Log(InteractableWeights);
		Debug.Log("Total weight:" + totalWeight);
		Debug.Log("(Objects, weights): " + objWeights);
	}

	void Update()
	{
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
}