using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private EnvironmentGenerator environmentGenerator;
    bool hasTriggeredSpawn = false;
    public void Init(EnvironmentGenerator environmentGenerator)
    {
        this.environmentGenerator = environmentGenerator;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * environmentGenerator.GetSpeed());
        if (!hasTriggeredSpawn)
        {
            float distance = Mathf.Abs(Vector3.Distance(gameObject.transform.position, environmentGenerator.gameObject.transform.position));
            if (distance > 10 * transform.localScale.z) //plane size 10m*1*10m
            {
                environmentGenerator.Spawn();
                hasTriggeredSpawn = true;
            }
        }
    }
}
