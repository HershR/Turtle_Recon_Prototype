using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public enum DroneState { Idle, Collecting, EnterExit }

    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private AudioClip tokenDepositSound;
    [SerializeField] private GameObject tokenVisualPrefab;
    [SerializeField] private Vector3 topRight;

    public DroneState state;
    public float moveSpeed;
    public float upForce;


    private Rigidbody rb;
    private Vector3 startPos;
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

    private float tokenCollectionRate;
    private float tokenCollectionTimer = 0f;
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
        tokenCollectionRate = 1f / (1f + stats.GetStat(StatType.DroneCollectionRate).Level);
        player = FindAnyObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("Drone Could not find player");
        }
        float z = Camera.main.transform.position.z;
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, z));
        minHeight = Mathf.Max(topLeft.y, -6f);
        maxHeight = bottomRight.y;
        minWidth = topLeft.x;
        maxWidth = bottomRight.x;

        //moveSpeed = player.maxSpeed;
        state = DroneState.EnterExit;
        timeRemaining = timeDuration;
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
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
            if (Vector3.Distance(transform.position, targetDestination) < 1f)
            {
                UpdateTargetDestination();
            }
            if (state == DroneState.Idle)
            {
                if (PlayerInRange())
                {
                    state = DroneState.Collecting;
                    return;
                }
            }
            else if (state == DroneState.Collecting)
            {
                if (!PlayerInRange())
                {
                    state = DroneState.Idle;
                    return;
                }
                if (player.tokenCount > 0)
                {
                    if (tokenCollectionTimer > tokenCollectionRate)
                    {
                        TokenCollect();
                        tokenCollectionTimer = 0;
                        return;
                    }
                    tokenCollectionTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) < 1f)
            {
                Destroy(gameObject);
            }
            if (state != DroneState.EnterExit)
            {
                state = DroneState.EnterExit;
                targetDestination = startPos;
                moveSpeed = 20f;
                StopAllCoroutines();
                return;

            }
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
        //Debug.Log($"Drone New Target Destination: {targetDestination}");
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

    // End timer early
    public void EndTimer()
    {
        timeRemaining = 0;
    }
    private bool PlayerInRange()
    {
        Vector3 turtlePos = player.transform.position;
        Vector3 dronePos = transform.position;
        Vector2 turtle = new Vector2(turtlePos.x, turtlePos.y);
        Vector2 drone = new Vector2(dronePos.x, dronePos.y);
        float distance = Vector2.Distance(turtle, drone);
        if (distance < droneRange)
        {
            return true;
        }
        return false;
    }

    // Collect token, add visual / sound feedback
    private void TokenCollect()
    {
        var visual = Instantiate(tokenVisualPrefab, transform.position, Quaternion.identity);
        visual.GetComponent<TokenCollectVisual>().Init(topRight);
        player.tokenCount -= 1;
        stats.AddTokens(1);
        player.onTokenBanked?.Invoke();
        SoundManager.instance.PlaySoundClip(tokenDepositSound, transform, 1f);
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