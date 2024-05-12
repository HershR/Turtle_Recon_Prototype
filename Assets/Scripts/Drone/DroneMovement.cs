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
    private float wantedYRotation;
    private float currentYRotation;
    private float rotateAmount = 2.5f;
    private float rotationYVelocity;
    public float tiltIntensity;

    [SerializeField] PlayerController player;
    [SerializeField] PlayerStatsSO stats;

    void Awake() {
        drone = GetComponent<Rigidbody>();
    }

    void Start() {
        timeRemaining = timeDuration;
        drone = GetComponent<Rigidbody>();
        GetComponent<Collider>().enabled = true;
        float x = Random.value > 0.5f ? Random.Range(20, 31) : Random.Range(-20, -31);
        float y = Random.value > 0.5f ? Random.Range(20, 31) : Random.Range(-20, -31);
        
        startPosition = new Vector3(x, y, 0); 
        transform.position = startPosition;
        targetPosition = new Vector3(0, 0, 0); 
        StartCoroutine(MoveDroneIntoView());
    }

    IEnumerator MoveDroneIntoView() {
        while (Vector3.Distance(transform.position, targetPosition) > 5f) {
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
            // Debug.Log("z: " + transform.position.z + " y: " + transform.position.y + " x: " + transform.position.x);
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
        }
    }

    void MoveToPosition() {
        Vector3 newPos = new Vector3(targetPosition.x, targetPosition.y, 0);
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

    void Rotation() {
        if (targetPosition.x < transform.position.x) {
            wantedYRotation -= rotateAmount;
        }
        if (targetPosition.x > transform.position.x) {
            wantedYRotation += rotateAmount;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
        drone.rotation = Quaternion.Euler(new Vector3(1, currentYRotation, drone.rotation.z));
    }

    void OnTimerEnd()
    {
        if (isCollecting) {
            isCollecting = false;
        }

        GetComponent<Collider>().enabled = false;
        targetPosition = new Vector3(25, 0, 0);
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

    // Every second, collect one token from player and update total tokens
    // in player stats
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) {
            return;
        }
        isCollecting = true;
        collision.gameObject.GetComponent<PlayerController>().OnCollision(this.gameObject);
        while (timeRemaining > 0 && player.tokenCount > 0)
        {
            StartCoroutine(TokenCollection());
        }  
        
    }

    IEnumerator TokenCollection()
    {
        Debug.Log("Player touched drone");
        player.tokenCount -= 1;
        stats.AddTokens(1);
        yield return new WaitForSeconds(1);   
    }

    // Collect token, add visual / sound feedback
    void OnCollectToken()
    {
        // To Do
        return;
    }

}



