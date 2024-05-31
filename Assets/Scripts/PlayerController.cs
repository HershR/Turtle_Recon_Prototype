using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
<<<<<<< Updated upstream
<<<<<<< HEAD
    public PlayerStatsSO playerStats;

    private CharacterController controller;
=======
    // private CharacterController controller;
>>>>>>> ObsticleSpawnerBranch3.0
    public float playerSpeed;
    public float maxSpeed;
    public int health;
    public int maxHealth;
    public int dashes;
    public int maxDashes;
    public int tokenCount;
    public float parryCooldownStat;
    public bool parry = false;
    public bool parrySucceed = false;
    public bool iFrames = false;
    private bool canParry = true;
    private bool screenBlur = false;
    private bool bleed = false;
    private bool slowed = false;

    // Stat levels
    private int healthLevel;
    private int speedLevel;
    private int dashLevel;
    private int parryCooldownLevel;

    public TextMeshProUGUI livesText;
    public Color baseColor;
    public Color damageColor;
    public Color healColor;
    public Color researchColor;
    public Color bleedColor;
    public Color oilColor;
    public Color parryColor;

    // Start is called before the first frame update
    void Start()
<<<<<<< HEAD
    {   
        controller = gameObject.AddComponent<CharacterController>();
=======
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        // controller = gameObject.AddComponent<CharacterController>();

>>>>>>> ObsticleSpawnerBranch3.0
        Debug.Log(this.transform.localPosition.x);
        baseColor = this.GetComponentInChildren<Renderer>().material.color;
        damageColor = Color.Lerp(baseColor, Color.red, 0.5f);
        healColor = Color.Lerp(baseColor, Color.green, 0.5f);
        researchColor = Color.Lerp(baseColor, Color.blue, 0.5f);
        bleedColor = Color.Lerp(baseColor, Color.red, 1);
        oilColor = Color.Lerp(baseColor, Color.black, 0.75f);
        parryColor = Color.Lerp(baseColor, Color.cyan, 0.5f);

        // Collect Stat levels.
        healthLevel = playerStats.GetStat(StatType.Health).Level;
        Debug.Log("Health Level: " + healthLevel);
        speedLevel = playerStats.GetStat(StatType.Speed).Level;
        Debug.Log("Speed Level: " + speedLevel);
        dashLevel = playerStats.GetStat(StatType.Dash).Level;
        Debug.Log("Dash Level: " + dashLevel);
        parryCooldownLevel = playerStats.GetStat(StatType.ParryCooldown).Level;
        Debug.Log("Dash Level: " + dashLevel);

        // Initialize Player Stats
        maxHealth = 3 + healthLevel;
        health = maxHealth;
        maxSpeed = 3 + (speedLevel * 2);
        playerSpeed = maxSpeed;
        maxDashes = 1 + dashLevel;
        dashes = 0;
        parryCooldownStat = 3 - (0.5f * (float)parryCooldownLevel);
        tokenCount = 0;

        // Set UI text
        livesText.text = "Lives Remaining: " + health;
    }

=======
    private CharacterController controller;
    public float playerSpeed = 100.0f;
    public float maxSpeed = 150.0f;
    public int health = 3;
    public int maxHealth = 3;
    public int dashes = 0;
    public int maxDashes = 3;
    public int tokenCount = 0;
    public bool parry = false;
    public bool iFrames = false;
    private bool canParry = true;
    float maxHeight;
    float maxWidth;

    public TextMeshProUGUI livesText;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        maxHeight = (canvas.planeDistance / 2) - 1;
        maxWidth = canvas.planeDistance - 1;
        Debug.Log("Height: " + maxHeight);
        Debug.Log("Width: " + maxWidth);
        controller = gameObject.AddComponent<CharacterController>();
        Debug.Log(this.transform.localPosition.x);
        livesText.text = "Lives Remaining: " + health;
        baseColor = this.GetComponentInChildren<Renderer>().material.color;
    }
    
>>>>>>> Stashed changes
    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
<<<<<<< Updated upstream
        if(move.magnitude > 1f)
        {
            move = move.normalized;
        }
        transform.position += (move * Time.deltaTime * playerSpeed);
        Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		transform.position = Camera.main.ViewportToWorldPoint(pos);
=======
        if (this.transform.localPosition.x > maxWidth && move[0] > 0)
        {
            move[0] = 0;
            Debug.Log("out of bounds on X");
        }
        else if (this.transform.localPosition.x < -1 * maxWidth && move[0] < 0)
        {
            move[0] = 0;
            Debug.Log("out of bounds on X");
        }
        if (this.transform.localPosition.y > maxHeight && move[1] > 0)
        {
            move[1] = 0;
            Debug.Log("out of bounds on Y");
        }
        else if (this.transform.localPosition.y < -1 * maxHeight && move[1] < 0)
        {
            move[1] = 0;
            Debug.Log("out of bounds on Y");
        }
        controller.Move(move * Time.deltaTime * playerSpeed);
>>>>>>> Stashed changes

        if (Input.GetKeyDown(KeyCode.Space) && canParry){
            StartCoroutine(PlayerParry()); 
        }
    }

    public void OnCollision(GameObject collider)
    {
<<<<<<< Updated upstream
        InteractableType obst_type = collider.GetComponent<ObsticleController>().obsticle_type;
        if (parry && (obst_type != InteractableType.Tokens && obst_type != InteractableType.Kelp && obst_type != InteractableType.JellyFish))
=======
        if (parry)
>>>>>>> Stashed changes
        {
            Debug.Log("Nice Parry!");
            Destroy(collider);
            StartCoroutine(SuccessfulParry());
            return;
        }
<<<<<<< Updated upstream
        else if (iFrames && (obst_type != InteractableType.Tokens && obst_type != InteractableType.Kelp && obst_type != InteractableType.JellyFish))
=======
        else if (iFrames)
>>>>>>> Stashed changes
        {
            Debug.Log("In damage iFrames");
            Destroy(collider);
            return;
        }
<<<<<<< Updated upstream
        else
        {
            Debug.Log("You hit a " + obst_type);
            if (obst_type == InteractableType.Trash) // case for Trash
            {
                Debug.Log("That's trash");
                StartCoroutine(TakeDamage());
            }
            else if (obst_type == InteractableType.Oil) // case for oil
            {
                Debug.Log("That's oil");
                StartCoroutine(CollideOil());
            }
            else if (obst_type == InteractableType.Wires) // case for wire
            {
                Debug.Log("That's wire");
                StartCoroutine(CollideWire());
            }
            else if (obst_type == InteractableType.Tokens) // Case for token
            {
                Debug.Log("That's a token");
                StartCoroutine(CollideToken());
            }
            else if (obst_type == InteractableType.Kelp) // Case for food
            {
                Debug.Log("That's kelp");
                StartCoroutine(CollectFood());
            }
            else if (obst_type == InteractableType.Sharp) // case for sharps
            {
                Debug.Log("That's sharp");
                StartCoroutine(CollideSharp());
            }
            else if (obst_type == InteractableType.JellyFish) // case for jellyfish
            {
                Debug.Log("That's jellyfish");
                // Add coroutine for jellyfish
            }
            else
            {
                Debug.Log("Error in finding collider type");
            }
            Destroy(collider);
        }
=======
        StartCoroutine(TakeDamage());
        
>>>>>>> Stashed changes
    }

    public void OnDeath()
    {
        Debug.Log("You died!");
    }

    IEnumerator PlayerParry()
    {
        Debug.Log("Player has parried");
<<<<<<< Updated upstream
        float parryDuration = 0.5f;
        float passedTime = 0;
        Quaternion initialRotation = this.transform.rotation;
        parry = true;
        canParry = false;
        this.GetComponentInChildren<Renderer>().material.color = parryColor; // Swap to parry color.
        // SPIIIIIIIIIIIIIINNNNN
        while (passedTime < parryDuration)
        {
            this.transform.Rotate(Vector3.up * (360 * Time.deltaTime / parryDuration));
            passedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.rotation = initialRotation;
        //yield return new WaitForSeconds(parryDuration);
        if (parrySucceed == false) {
            StartCoroutine(ParryCooldown(parryCooldownStat));
        }
        else
        {
            parrySucceed = false;
            StartCoroutine(ParryCooldown(0));
        }
        yield return null;
    }

    IEnumerator ParryCooldown(float seconds)
=======
        parry = true;
        this.GetComponentInChildren<Renderer>().material.color = new Color(255, 255, 255);
        canParry = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ParryCooldown(3));
        yield return null;
    }

    IEnumerator ParryCooldown(int seconds)
>>>>>>> Stashed changes
    {
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
        parry = false;
        yield return new WaitForSeconds(seconds);
        canParry = true;
        yield return null;
    }

    IEnumerator SuccessfulParry()
<<<<<<< Updated upstream
    {   
        parrySucceed = true;
        parry = false;
        canParry = true;
        if (dashes < maxDashes)
        {
            dashes = dashes + 1;
        }
=======
    {
        parry = false;
        canParry = true;
>>>>>>> Stashed changes
        yield return null;
    }

    IEnumerator TakeDamage()
    {
        canParry = true;
        iFrames = true;
        health -= 1;
        if (health <= 0)
        {
            OnDeath();
        }
        livesText.text = "Lives Remaining: " + health;
<<<<<<< Updated upstream
        this.GetComponentInChildren<Renderer>().material.color = damageColor; // Swap to the damage color.
=======
        this.GetComponentInChildren<Renderer>().material.color = new Color(255, 0, 0);
>>>>>>> Stashed changes
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
        iFrames = false;
    }
<<<<<<< Updated upstream

    IEnumerator CollectFood()
    {
        if (health < maxHealth)
        {
            health += 1;
        }
        livesText.text = "Lives Remaining: " + health;
        this.GetComponentInChildren<Renderer>().material.color = healColor; // Swap to heal color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideOil()
    {
        screenBlur = true;
        StartCoroutine(TakeDamage());
        this.GetComponentInChildren<Renderer>().material.color = oilColor; // Swap to oil color.
        yield return new WaitForSeconds(5);
        screenBlur = false;
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideSharp()
    {
        bleed = true;
        StartCoroutine(TakeDamage());
        this.GetComponentInChildren<Renderer>().material.color = bleedColor; // Flash to bleed color.
        yield return new WaitForSeconds(20);
        bleed = false;
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideWire()
    {
        slowed = true;
        playerSpeed = 1;
        StartCoroutine(TakeDamage());
        yield return new WaitForSeconds(10);
        playerSpeed = maxSpeed;
        slowed = false;
    }

    IEnumerator CollideToken()
    {
        tokenCount += 1;
        this.GetComponentInChildren<Renderer>().material.color = researchColor; // Swap to research color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }
=======
>>>>>>> Stashed changes
}
