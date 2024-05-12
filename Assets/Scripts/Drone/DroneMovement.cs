using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    Rigidbody drone;
    public float upForce;
    public float movementForwardSpeed = 500f;
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
    private float wantedYRotation;
    private float currentYRotation;
    private float rotateAmount = 2.5f;
    private float rotationYVelocity;
    public float tiltIntensity;

    void Awake() {
        drone = GetComponent<Rigidbody>();
    }

    void Start() {
        timeRemaining = timeDuration;
        drone = GetComponent<Rigidbody>();
        startPosition = new Vector3(0, -25, 10); 
        transform.position = startPosition;
        targetPosition = new Vector3(0, 0, 10); 
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
            Debug.Log("Time remaining: " + timeRemaining + " seconds");
            if (Vector3.Distance(transform.position, targetPosition) < 3f || transform.position.y < -maxHeight || transform.position.y > maxHeight) {
                UpdateRandomTarget();
            }
        } else if (!isEntering && timeRemaining <= 0) {
            targetPosition = new Vector3(0, -25, 10);
            MoveToPosition();
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
        Vector3 direction = (targetPosition - transform.position).normalized;
        drone.velocity = Vector3.SmoothDamp(drone.velocity, direction * moveSpeed, ref velocityDamp, 0.3f);
    }

    void AdjustUpwardForce() {
        drone.AddForce(Vector3.up * upForce);
    }

    void UpdateRandomTarget() {
        float x = Random.Range(minWidth, maxWidth);
        float y = Random.Range(minHeight, maxHeight);
        targetPosition = new Vector3(x, y, 10);
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

}



