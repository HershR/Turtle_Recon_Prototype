using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public enum DroneState { Idle, Collecting, EnterExit }

    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private AudioClip TokenDepositSound;

    public DroneState state;

    Rigidbody rb;
    public float upForce;
    public float moveSpeed;
    private Vector3 velocityDamp;

    private Vector3 targetDestination;
    private float minHeight;
    private float maxHeight;
    private float minWidth;
    private float maxWidth;

    public float timeDuration;
    private float timeRemaining;

    public float tiltIntensity;
    public float droneRange;

    public int tokenCollectionRate;
    // private float wantedYRotation;
    // private float currentYRotation;
    // private float rotateAmount = 2.5f;
    // private float rotationYVelocity;


    private PlayerController player;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("Drone Could not find player");
        }
        float z = player.transform.position.z - Camera.main.transform.position.z;
        // Bottom-left corner
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z));
        // Top-right corner
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, z));

        minHeight = Mathf.Max(bottomLeft.y, -6f);
        maxHeight = topRight.y;
        minWidth = bottomLeft.x;
        maxWidth = topRight.x;
        //moveSpeed = player.maxSpeed;
        state = DroneState.EnterExit;
        timeRemaining = timeDuration;
        rb = GetComponent<Rigidbody>();
        targetDestination = new Vector3((maxWidth + minWidth) / 2, (maxHeight + minHeight) / 2, transform.position.z);
    }

    void Update()
    {
        if (state == DroneState.EnterExit)
        {
            if (Vector3.Distance(transform.position, targetDestination) < 1f)
            {
                UpdateTargetDestination();
                state = DroneState.Idle;
            }
            return;
        }
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            //Debug.Log("Time remaining: " + timeRemaining + " seconds");
            if (Vector3.Distance(transform.position, targetDestination) < 1f)
            {
                UpdateTargetDestination();
            }
            NearPlayer();
        }
        else
        {
            OnTimerEnd();
        }
    }

    void FixedUpdate()
    {
        MoveToPosition();
        if (state != DroneState.EnterExit)
        {
            AdjustTilt();
            // Rotation();
            AdjustUpwardForce();
        }
    }

    void MoveToPosition()
    {
        Vector3 direction = (targetDestination - transform.position).normalized;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, direction * moveSpeed, ref velocityDamp, 0.3f);
        //transform.position = Vector3.Lerp(transform.position, targetDestination, moveSpeed * Time.deltaTime / 5f);
        //Debug.Log($"Drone moving to {targetDestination} ({direction})");
    }

    void AdjustUpwardForce()
    {
        //rb.AddForce(Vector3.up * upForce);
    }

    void UpdateTargetDestination()
    {
        float x = Random.Range(minWidth, maxWidth);
        float y = Random.Range(minHeight, maxHeight);

        targetDestination = new Vector3(x, y, transform.position.z);
        Debug.Log($"Drone New Target Destination: {targetDestination}");
    }

    void AdjustTilt()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        float forwardTilt = -horizontalVelocity.z * tiltIntensity;
        float sideTilt = -horizontalVelocity.x * tiltIntensity;
        Quaternion targetRotation = Quaternion.Euler(forwardTilt, rb.rotation.eulerAngles.y, sideTilt);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * moveSpeed);
    }

    void OnTimerEnd()
    {
        if (state == DroneState.EnterExit) { return; }
        state = DroneState.EnterExit;
        StopAllCoroutines();
        targetDestination = new Vector3(maxWidth + 3f, maxHeight + 3f, transform.position.z);
        moveSpeed = 20f;
        if (Vector3.Distance(transform.position, targetDestination) < 1f)
        {
            Destroy(gameObject);
        }
    }

    // End timer early
    public void EndTimer()
    {
        timeRemaining = 0;
        OnTimerEnd();
    }

    void NearPlayer()
    {
        if (state != DroneState.Idle)
        {
            return;
        }
        state = DroneState.Collecting;
        Vector3 turtlePos = player.transform.position;
        Vector3 dronePos = transform.position;
        Vector2 turtle = new Vector2(turtlePos.x, turtlePos.y);
        Vector2 drone = new Vector2(dronePos.x, dronePos.y);
        float distance = Vector2.Distance(turtle, drone);
        //Debug.Log($"Drone Distance: {distance}");
        if (distance < droneRange && player.tokenCount > 0)
        {
            StartCoroutine(TokenCollection());
        }
        else
        {
            state = DroneState.Idle;
        }

    }

    // Every second, collect one token from player and update total tokens
    // in player stats
    IEnumerator TokenCollection()
    {
        Debug.Log("Player in range of drone");
        //Debug.Log("Player token count: " + player.tokenCount);
        //Debug.Log("Drone token count: " + stats.Tokens);
        SoundManager.instance.PlaySoundClip(TokenDepositSound, transform, 1f);
        player.tokenCount -= 1;
        stats.AddTokens(1);
        player.onTokenBanked.Invoke();
        OnCollectToken();
        yield return new WaitForSeconds(1f / (1f + stats.GetStat(StatType.DroneCollectionRate).Level));
        state = DroneState.Idle;
    }

    // Collect token, add visual / sound feedback
    void OnCollectToken()
    {
        // To Do
        return;
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
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = state == DroneState.Collecting ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, droneRange);
    }

}