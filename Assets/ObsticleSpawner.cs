using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleSpawner : MonoBehaviour
{
    public GameObject obsticle;

	float timer = 0f;

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
