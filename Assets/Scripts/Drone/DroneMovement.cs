using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    Rigidbody drone;
    public float upForce;
    public float moveSpeed;
    private Vector3 velocityDamp;

    private Vector3 targetPosition;
    private Vector3 startPosition;
    public float minHeight;
    public float maxHeight;
    public float minWidth;
    public float maxWidth;
    public float timeDuration;
    private float timeRemaining;
    private bool isEntering = true;
    private bool isCollecting = false;
    public float tiltIntensity;
    public float droneRange;
    private float wantedYRotation;
    private float currentYRotation;
    private float rotateAmount = 2.5f;
    private float rotationYVelocity;
    

    [SerializeField] PlayerController player;
    [SerializeField] PlayerStatsSO stats;

    void Awake() {
        drone = GetComponent<Rigidbody>();
    }

    void Start() {
        timeRemaining = timeDuration;
        drone = GetComponent<Rigidbody>();
 
        float x = Random.value > 0.5f ? Random.Range(10, 16) : Random.Range(-15, -9);
        float y = Random.value > 0.5f ? -10 : 10;

        startPosition = new Vector3(x, y, 0); 
        transform.position = startPosition;
        targetPosition = new Vector3(0, 0, 0); 
        StartCoroutine(MoveDroneIntoView());
    }

    IEnumerator MoveDroneIntoView() {
        while (Vector3.Distance(transform.position, targetPosition) > 3f) {
            MoveToPosition();
            yield return null;
        }
        isEntering = false;
        UpdateRandomTarget();
    }

    void Update() {
        if (!isEntering && timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
            // Debug.Log("Time remaining: " + timeRemaining + " seconds");
            if (Vector3.Distance(transform.position, targetPosition) < 3f || transform.position.y < -maxHeight || transform.position.y > maxHeight) {
                UpdateRandomTarget();
            }    
            NearPlayer();    
        } else if (!isEntering && timeRemaining <= 0) {
            OnTimerEnd();
        }
    }

    void FixedUpdate() {
        if (!isEntering) {
            MoveToPosition();
            AdjustTilt();
            // Rotation();
            AdjustUpwardForce();
            NearPlayer();
        }
    }

    void MoveToPosition() {
        Vector3 direction = (targetPosition - transform.position).normalized;
        drone.velocity = Vector3.SmoothDamp(drone.velocity, direction * moveSpeed, ref velocityDamp, 0.3f);
    }

    void AdjustUpwardForce() {
        drone.AddForce(Vector3.up * upForce);
    }

    void UpdateRandomTarget() {
        float x = Random.Range(minWidth, maxWidth);
        float y = Random.Range(minHeight, maxHeight);
        targetPosition = new Vector3(x, y, 0);
    }

    void AdjustTilt() {
        Vector3 horizontalVelocity = drone.velocity;
        horizontalVelocity.y = 0;
        float forwardTilt = -horizontalVelocity.z * tiltIntensity;
        float sideTilt = -horizontalVelocity.x * tiltIntensity;
        Quaternion targetRotation = Quaternion.Euler(forwardTilt, drone.rotation.eulerAngles.y, sideTilt);
        drone.rotation = Quaternion.Slerp(drone.rotation, targetRotation, Time.deltaTime * moveSpeed);
    }

    // void Rotation() {
    //     if (targetPosition.x < transform.position.x) {
    //         wantedYRotation -= rotateAmount;
    //     }
    //     if (targetPosition.x > transform.position.x) {
    //         wantedYRotation += rotateAmount;
    //     }

    //     currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    //     drone.rotation = Quaternion.Euler(new Vector3(1, currentYRotation, drone.rotation.z));
    // }

    void OnTimerEnd()
    {
        if (isCollecting) {
            isCollecting = false;
        }

        targetPosition = new Vector3(15, 0, 0);
        MoveToPosition();
        if (Vector3.Distance(transform.position, targetPosition) < 3f) {
            Destroy(gameObject);
        }    
    }

    // End timer early
    public void EndTimer()
    {
        if (isCollecting) {
            isCollecting = false;
        }
        timeRemaining = 0;
        OnTimerEnd(); 
    }

    void NearPlayer() {
        Vector3 turtlePos = player.transform.position;
        Vector3 dronePos = transform.position;
        Vector2 turtle = new Vector2(turtlePos.x, turtlePos.y);
        Vector2 drone = new Vector2(dronePos.x / 8, dronePos.y / 8);
        float distance = Vector2.Distance(turtle, drone);
        if (distance < droneRange && !isCollecting && player.tokenCount > 0) {
            StartCoroutine(TokenCollection());
        }
        
    }

    // Every second, collect one token from player and update total tokens
    // in player stats
    IEnumerator TokenCollection()
    {
        isCollecting = true;
        player.tokenCount -= 1;
        stats.AddToken();
        yield return new WaitForSeconds(1);
        isCollecting = false;   
    }

    // Collect token, add visual / sound feedback
    void OnCollectToken()
    {
        // To Do
        return;
    }

}



