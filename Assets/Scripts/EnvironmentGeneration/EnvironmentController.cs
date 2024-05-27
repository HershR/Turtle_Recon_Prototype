using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private EnvironmentType type;
    [SerializeField] private Transform despawnPoint;
    
    private EnvironmentGenerator environmentGenerator;
    private bool hasTriggeredSpawn = false;
    public void Init(EnvironmentGenerator environmentGenerator)
    {
        this.environmentGenerator = environmentGenerator;
    }

    private void Update()
    {
        if(environmentGenerator != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * environmentGenerator.GetSpeed());
            if (!hasTriggeredSpawn)
            {
                if(despawnPoint.transform.position.z < environmentGenerator.transform.position.z)
                {
                    Vector3 offset = despawnPoint.transform.position - environmentGenerator.transform.position;
                    environmentGenerator.Spawn(offset);
                    hasTriggeredSpawn = true;
                }
            }
            if (transform.position.z < Camera.main.transform.position.z)
            {
                environmentGenerator.TriggerEnvironmentChange(type);
            }
            if (despawnPoint.position.z < Camera.main.transform.position.z)
            {
                Destroy(gameObject, 1f);
            }
        }
    }
}
