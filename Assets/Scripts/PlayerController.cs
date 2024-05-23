using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public float playerSpeed;
    public float maxSpeed;
    public int health = 3;
    public int maxHealth = 3;
    public int dashes = 0;
    public int maxDashes = 3;
    public int tokenCount = 0;
    public bool parry = false;
    public bool parrySucceed = false;
    public bool iFrames = false;
    private bool canParry = true;
    private bool screenBlur = false;
    private bool bleed = false;
    private bool slowed = false;

    public TextMeshProUGUI livesText;
    private Color baseColor;
    private Color damageColor;
    private Color healColor;
    private Color researchColor;
    private Color bleedColor;
    private Color oilColor;
    private Color parryColor;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        controller = gameObject.AddComponent<CharacterController>();
        Debug.Log(this.transform.localPosition.x);
        livesText.text = "Lives Remaining: " + health;
        baseColor = this.GetComponentInChildren<Renderer>().material.color;
        damageColor = Color.Lerp(baseColor, Color.red, 0.5f);
        healColor = Color.Lerp(baseColor, Color.green, 0.5f);
        researchColor = Color.Lerp(baseColor, Color.blue, 0.5f);
        bleedColor = Color.Lerp(baseColor, Color.red, 1);
        oilColor = Color.Lerp(baseColor, Color.black, 0.75f);
        parryColor = Color.Lerp(baseColor, Color.cyan, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if(move.magnitude > 1f)
        {
            move = move.normalized;
        }
        controller.Move(move * Time.deltaTime * playerSpeed);
        Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		transform.position = Camera.main.ViewportToWorldPoint(pos);

        if (Input.GetKeyDown(KeyCode.Space) && canParry){
            StartCoroutine(PlayerParry()); 
        }
    }

    public void OnCollision(GameObject collider)
    {
        if (parry)
        {
            Debug.Log("Nice Parry!");
            Destroy(collider);
            StartCoroutine(SuccessfulParry());
            return;
        }
        else if (iFrames)
        {
            Debug.Log("In damage iFrames");
            Destroy(collider);
            return;
        }
        else
        {
            InteractableType obst_type = collider.GetComponent<ObsticleController>().obsticle_type;
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
            else 
            {
                Debug.Log("Error in finding collider type");
            }
            Destroy(collider);
        }
    }

    public void OnDeath()
    {
        Debug.Log("You died!");
    }

    IEnumerator PlayerParry()
    {
        Debug.Log("Player has parried");
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
            StartCoroutine(ParryCooldown(3));
        }
        else
        {
            parrySucceed = false;
            StartCoroutine(ParryCooldown(0));
        }
        yield return null;
    }

    IEnumerator ParryCooldown(int seconds)
    {
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
        parry = false;
        yield return new WaitForSeconds(seconds);
        canParry = true;
        yield return null;
    }

    IEnumerator SuccessfulParry()
    {   
        parrySucceed = true;
        parry = false;
        canParry = true;
        if (dashes < maxDashes)
        {
            dashes = dashes + 1;
        }
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
        this.GetComponentInChildren<Renderer>().material.color = damageColor; // Swap to the damage color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
        iFrames = false;
    }

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
        playerSpeed = 5;
        slowed = false;
    }

    IEnumerator CollideToken()
    {
        tokenCount += 1;
        this.GetComponentInChildren<Renderer>().material.color = researchColor; // Swap to research color.
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }
}
