using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
	public class ObsticleSpawner : MonoBehaviour
	{
		public GameObject obsticle;
		public GameObject token;

		[SerializedDictionary("Interactable", "Weight")]
		public SerializedDictionary<InteractableType, int> InteractableWeights;

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

		// We get env passed in
		// From that enum, we get 

		float timer = 0f;

		private void Start()
		{
			Debug.Log(InteractableWeights);
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
				Instantiate(obsticle, this.transform);
				Debug.Log("obsticle spawned");
			}
		}
	}
}