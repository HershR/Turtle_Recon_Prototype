using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("SoundFXS")]
    [SerializeField] private AudioClip ParrySound;
    [SerializeField] private AudioClip ParrySucceedSound;
    [SerializeField] private AudioClip collectTokenSound;
    [SerializeField] private AudioClip TrashHitSound;
    [SerializeField] private AudioClip OilHitSound;
    [SerializeField] private AudioClip SharpHitSound;
    [SerializeField] private AudioClip WireHitSound;
    [SerializeField] private AudioClip IAteAJellyfish;


    [field: SerializeField] public PlayerStatsSO playerStats { get; private set; }

    [SerializeField] private CanvasGroup blurCanvas;

    public int playerScore = 0;
    public float playerSpeed;
    public float maxSpeed;
    public float health;
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

    public Color baseColor;
    public Color damageColor;
    public Color healColor;
    public Color researchColor;
    public Color bleedColor;
    public Color oilColor;
    public Color parryColor;

    public UnityAction onTokenCollect;
    public UnityAction onTokenBanked;
    public UnityAction onHealthChange;

    // Event used to set high scores
    public UnityEvent<string, int> submitScoreEvent;

    // Start is called before the first frame update
    void Awake()
    {   
        baseColor = this.GetComponentInChildren<Renderer>().material.color;
        damageColor = Color.Lerp(baseColor, Color.red, 0.5f);
        healColor = Color.Lerp(baseColor, Color.green, 0.5f);
        researchColor = Color.Lerp(baseColor, Color.blue, 0.5f);
        bleedColor = Color.Lerp(baseColor, Color.red, 1);
        oilColor = Color.Lerp(baseColor, Color.black, 0.75f);
        parryColor = Color.Lerp(baseColor, Color.cyan, 0.5f);

        // Collect Stat levels.
        healthLevel = playerStats.GetStat(StatType.Health).Level;
        speedLevel = playerStats.GetStat(StatType.Speed).Level;
        dashLevel = playerStats.GetStat(StatType.Dash).Level;
        parryCooldownLevel = playerStats.GetStat(StatType.ParryCooldown).Level;
        Debug.Log("Health Level: " + healthLevel);
        Debug.Log("Speed Level: " + speedLevel);
        Debug.Log("Dash Level: " + dashLevel);
        Debug.Log("Dash Level: " + dashLevel);

        // Initialize Player Stats
        maxHealth = 1 + healthLevel;
        health = maxHealth;
        maxSpeed = 3 + playerStats.GetStat(StatType.Speed).Level / playerStats.GetStat(StatType.Speed).MaxLevel;
        playerSpeed = maxSpeed;
        maxDashes = 1 + dashLevel;
        dashes = 0;
        parryCooldownStat = 3 - (0.5f * (float)parryCooldownLevel);
        tokenCount = 0;
    }

    private void OnDisable()
    {
        onHealthChange = null;
        onTokenBanked = null;
        onTokenCollect = null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if(move.magnitude > 1f)
        {
            move = move.normalized;
        }
        // controller.Move(move * Time.deltaTime * playerSpeed);
        transform.position += (move * Time.deltaTime * playerSpeed);
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        if (Input.GetKeyDown(KeyCode.Space) && canParry){
            StartCoroutine(PlayerParry()); 
        }
    }

    public void OnCollision(GameObject collider)
    {
        if(enabled == false) return;
        InteractableType obst_type = collider.GetComponent<ObsticleController>().obsticle_type;
        if (parry && (obst_type != InteractableType.Tokens && obst_type != InteractableType.Kelp && obst_type != InteractableType.JellyFish))
        {
            Debug.Log("Nice Parry!");
            Destroy(collider);
            StartCoroutine(SuccessfulParry());
            return;
        }
        else if (iFrames && (obst_type != InteractableType.Tokens && obst_type != InteractableType.Kelp && obst_type != InteractableType.JellyFish))
        {
            Debug.Log("In damage iFrames");
            Destroy(collider);
            return;
        }
        else
        {
            Debug.Log("You hit a " + obst_type);
            if (obst_type == InteractableType.Trash) // case for Trash
            {   
                SoundManager.instance.PlaySoundClip(TrashHitSound, transform, 1f);
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
                //SoundManager.instance.PlaySoundClip(collectTokenSound, transform, 1f);
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
                Debug.Log("That's a jellyfish");
                StartCoroutine(CollideJellyfish());
            }
            else 
            {
                Debug.Log("Error in finding collider type");
            }
            Destroy(collider);
        }
    }

    public void OnDeath()
    {
        SetPlayerHighScore(playerScore);
        Debug.Log("You died!");
    }

    IEnumerator PlayerParry()
    {
        Debug.Log("Player has parried");
        float parryDuration = 0.5f;
        float passedTime = 0;
        //Quaternion initialRotation = this.transform.rotation;
        parry = true;
        canParry = false;
        this.GetComponentInChildren<Renderer>().material.color = parryColor; // Swap to parry color.
        SoundManager.instance.PlaySoundClip(ParrySound, transform, 1f);
        // SPIIIIIIIIIIIIIINNNNN
        while (passedTime < parryDuration)
        {
            this.transform.Rotate(Vector3.up * (360 * Time.deltaTime / parryDuration));
            passedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.rotation = Quaternion.identity;
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
    {
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
        parry = false;
        yield return new WaitForSeconds(seconds);
        canParry = true;
        yield return null;
    }

    IEnumerator SuccessfulParry()
    {   
        SoundManager.instance.PlaySoundClip(ParrySucceedSound, transform, 1f);
        playerScore += 1000;
        Debug.Log("Score: " + playerScore.ToString());
        parrySucceed = true;
        parry = false;
        canParry = true;
        if (dashes < maxDashes)
        {
            dashes = dashes + 1;
        }
        yield return null;
    }

    IEnumerator TakeDamage(float amount = 1f)
    {
        canParry = true;
        iFrames = true;
        health -= amount;
        onHealthChange?.Invoke();
        playerScore -= 5000;
        Debug.Log("Score: " + playerScore.ToString());
        if (health <= 0)
        {
            OnDeath();
        }
        this.GetComponentInChildren<Renderer>().material.color = damageColor; // Swap to the damage color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
        iFrames = false;
    }

    IEnumerator CollectFood()
    {
        SoundManager.instance.PlaySoundClip(IAteAJellyfish, transform, 1f);
        playerScore += 2000;
        Debug.Log("Score: " + playerScore.ToString());
        if (health < maxHealth)
        {
            health += 1;
            onHealthChange?.Invoke();
        }
        this.GetComponentInChildren<Renderer>().material.color = healColor; // Swap to heal color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideOil()
    {
        SoundManager.instance.PlaySoundClip(OilHitSound, transform, 1f);
        StartCoroutine(TakeDamage(1f));
        if (screenBlur) 
        { 
            StopCoroutine(BlurScreen()); 
        }
        StartCoroutine(BlurScreen());
        yield return null;
    }
    IEnumerator BlurScreen()
    {
        screenBlur = true;
        blurCanvas.alpha = 1f;
        this.GetComponentInChildren<Renderer>().material.color = oilColor; // Swap to oil color.
        yield return new WaitForSeconds(2f);
        while (blurCanvas.alpha > 0f)
        {
            blurCanvas.alpha -= (1f * Time.deltaTime) / 3f;
            yield return new WaitForEndOfFrame();
        }
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
        screenBlur = false;
    }

    IEnumerator CollideSharp()
    {
        SoundManager.instance.PlaySoundClip(SharpHitSound, transform, 1f);
        bleed = true;
        StartCoroutine(TakeDamage(2f));
        this.GetComponentInChildren<Renderer>().material.color = bleedColor; // Flash to bleed color.
        yield return new WaitForSeconds(20);
        bleed = false;
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideWire()
    {
        SoundManager.instance.PlaySoundClip(WireHitSound, transform, 1f);
        slowed = true;
        playerSpeed = 1;
        StartCoroutine(TakeDamage());
        yield return new WaitForSeconds(10);
        playerSpeed = maxSpeed;
        slowed = false;
    }

    IEnumerator CollideToken()
    {
        SoundManager.instance.PlaySoundClip(collectTokenSound, transform, 1f);
        playerScore += 5000;
        Debug.Log("Score: " + playerScore.ToString());
        tokenCount += 1;
        onTokenCollect?.Invoke();
        this.GetComponentInChildren<Renderer>().material.color = researchColor; // Swap to research color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideJellyfish()
    {
        health = Mathf.Min(maxHealth, health + 2);
        playerScore += 3000;
        Debug.Log("Score: " + playerScore.ToString());
        onHealthChange?.Invoke();
        SoundManager.instance.PlaySoundClip(IAteAJellyfish, transform, 1f);
        this.GetComponentInChildren<Renderer>().material.color = healColor; // Swap to heal color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    public void SetPlayerHighScore(int score)
    {
        if (score > playerStats.GetPlayerHighScore())
        {
            playerStats.SetPlayerHighScore(score);
            SubmitScore();
        }
    }

    public void SubmitScore() {
        Debug.Log("Submitting high score of " + playerScore.ToString() + " to leaderboard database.");
        submitScoreEvent.Invoke(playerStats.GetPlayerName(), playerScore);
    }
}
