using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision enter with {other.gameObject.name}");
        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(other.gameObject);
        }
    }

    

}
