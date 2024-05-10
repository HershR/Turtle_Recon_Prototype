using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DroneScript : MonoBehaviour
{
    public float timeDuration;
    public float moveSpeed;
    public float minWidth;
    public float maxWidth;
    public float minHeight;
    public float maxHeight;
    private float timeRemaining;
    private bool isCollecting;
    private Vector3 targetPosition;
    private bool isEntering; 

    [SerializeField] public PlayerController playerStats;

    void Start()
    {
        // float x = RandVal();
        float x = Random.value > 0.5f ? Random.Range(250, 301) : Random.Range(-300, -249);
        float y = Random.value > 0.5f ? Random.Range(250, 301) : Random.Range(-300, -249);
        transform.position = new Vector3(x, y, 350); 
        targetPosition = new Vector3(0, 0, 350);
        timeRemaining = timeDuration;
        isCollecting = true;
        isEntering = true;
        GetComponent<Collider>().enabled = true;
    }

    void Update() 
    {
        if (isEntering) {
            // Move to on-screen position
            MoveToPosition(targetPosition.x, targetPosition.y, targetPosition.z);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Debug.Log("Drone in screen");
                isEntering = false; 
                
            }
        } else {

            timeRemaining -= Time.deltaTime;

            if (timeRemaining >= 0)
            {   
                RandomMovement();

            } else {

                OnTimerEnd();

            }
            
            
        }
    }

    void RandomMovement()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) // Check if reached the current target
        {
            // Generate new random target position
            float x = Random.Range(minWidth, maxWidth);
            float y = Random.Range(minHeight, maxHeight);

            targetPosition = new Vector3(x, y, 350);
        }
        MoveToPosition(targetPosition.x, targetPosition.y, targetPosition.z);
    }

    void MoveToPosition(float x, float y, float z)
    {
        Vector3 newPos = new Vector3(x, y, z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
    }

    void OnTimerEnd()
    {
        if (isCollecting) {
            isCollecting = false;
        }

        GetComponent<Collider>().enabled = false;
        MoveToPosition(400, 0, 350);
        if (Vector3.Distance(transform.position, new Vector3(400, 0, 350)) < 0.1f)
        {
            Debug.Log("Drone off screen");
            Destroy(gameObject);
        }
    
    }

    // End timer early
    public void EndTimer()
    {
        timeRemaining = 0;
        OnTimerEnd(); 
        
    }

    // Every second, collect one token from player and update total tokens
    // in player stats
    private void OnCollision(GameObject collider)
    {
        isCollecting = true;
        // To Do

    }

    // Collect token, add visual / sound feedback
    void OnCollectToken()
    {
        // To Do
        return;
    }

    
}

