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
    public bool iFrames = false;
    private bool canParry = true;
    private bool screenBlur = false;
    private bool bleed = false;
    private bool slowed = false;
    float maxHeight;
    float maxWidth;

    public TextMeshProUGUI livesText;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        //maxHeight = (canvas.planeDistance / 2) - 1;
        //maxWidth = canvas.planeDistance - 1;
        //Debug.Log("Height: " + maxHeight);
        //Debug.Log("Width: " + maxWidth);
        controller = gameObject.AddComponent<CharacterController>();
        Debug.Log(this.transform.localPosition.x);
        livesText.text = "Lives Remaining: " + health;
        baseColor = this.GetComponentInChildren<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (this.transform.localPosition.x > 2 && move[0] > 0)
        {
            move[0] = 0;
            Debug.Log("out of bounds on X");
        }
        else if (this.transform.localPosition.x < -2 && move[0] < 0)
        {
            move[0] = 0;
            Debug.Log("out of bounds on X");
        }
        if (this.transform.localPosition.y > 2 && move[1] > 0)
        {
            move[1] = 0;
            Debug.Log("out of bounds on Y");
        }
        else if (this.transform.localPosition.y < 0 && move[1] < 0)
        {
            move[1] = 0;
            Debug.Log("out of bounds on Y");
        }
        controller.Move(move * Time.deltaTime * playerSpeed);

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
        else if (true) // Collider is trash
        {
            InteractableType obst_type = collider.GetComponent<ObsticleController>().obsticle_type;
            Debug.Log("You hit a " + obst_type);
            if (obst_type == InteractableType.Trash)
            {
                Debug.Log("thats a trash");
            }
            else if(obst_type == InteractableType.Sharp)
            {
                Debug.Log("thats a sharp");
            }
            StartCoroutine(TakeDamage());
        }
        //else if (collider is token) // Case for token
        //{
        //    tokenCount += 1;
        //}
        //else if (collider is food) // Case for food
        //{
        //    StartCoroutine(CollectFood());
        //}
        //else if (collider is oil) // case for oil
        //{
        //    StartCoroutine(CollideOil());
        //}
        //else if (collider is sharp) // case for sharps
        //{
        //    StartCoroutine(CollideSharp());
        //}
        //else if (collider is wire) // case for wire
        //{
        //    StartCoroutine(CollideWire());
        //}
        Destroy(collider);
    }

    public void OnDeath()
    {
        Debug.Log("You died!");
    }

    IEnumerator PlayerParry()
    {
        Debug.Log("Player has parried");
        parry = true;
        this.GetComponentInChildren<Renderer>().material.color = new Color(255, 255, 255);
        canParry = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ParryCooldown(3));
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
        this.GetComponentInChildren<Renderer>().material.color = new Color(255, 0, 0);
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
        this.GetComponentInChildren<Renderer>().material.color = new Color(0, 255, 0); // Turtle flashes green
        yield return new WaitForSeconds(1);
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideOil()
    {
        screenBlur = true;
        StartCoroutine(TakeDamage());
        this.GetComponentInChildren<Renderer>().material.color = new Color(0, 0, 0); // Turtle goes black
        yield return new WaitForSeconds(5);
        screenBlur = false;
        this.GetComponentInChildren<Renderer>().material.color = baseColor;
    }

    IEnumerator CollideSharp()
    {
        bleed = true;
        StartCoroutine(TakeDamage());
        this.GetComponentInChildren<Renderer>().material.color = new Color(255, baseColor[1], baseColor[2]);
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
}
