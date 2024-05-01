using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{

    public float timeDuration = 10f;
    private float timeRemaining;
    private bool isCollecting = false;

    // Start is called before the first frame update
    // Move to position, start timer, enable collider
    void Start()
    {
        MoveToPosition(0, 0, 0);
        timeRemaining = timeDuration;
        isCollecting = true;
        GetComponent<Collider>().enabled = true;
    }

    // Update is called once per frame
    // Update timer, move drone around screen randomly
    void Update()
    {
        if (isCollecting)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                EndTimer();
            }

            RandomMovement();
        }
        
    }

    // Every second, collect one token from player and update total tokens
    // in player stats
    void OnCollision()
    {
        return;
    }

    // Helper function for randomzing movement
    void RandomMovement() 
    {
        // Move drone around screen (Values subject to change)
        float x = Random.Range(-10, 10);
        float y = Random.Range(-10, 10);
        transform.Translate(new Vector3(x, y, 0));
    }

    // Disable collider, move off screen, and destroy object
    void OnTimerEnd()
    {
        // Disable the collider and stop collecting
        isCollecting = false;
        GetComponent<Collider>().enabled = false;
        transform.position = new Vector3(-1000, 0, 0);
        Destroy(gameObject);
    }

    // End timer early
    public void EndTimer()
    {
        timeRemaining = 0;
    }

    // Collect token, add visual / sound feedback
    void OnCollectToken()
    {
        return;
    }

    // Helper function to move drone to specified position
    void MoveToPosition(float x, float y, float z)
    {
        // Move the drone to the specified position
        transform.position = new Vector3(x, y, z);
    }
    
}
