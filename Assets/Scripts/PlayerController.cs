using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
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
    private bool screenBlur = false;
    private bool bleed = false;
    float maxHeight;
    float maxWidth;

    public TextMeshProUGUI livesText;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        maxHeight = (canvas.planeDistance / 2) - 1;
        maxWidth = (canvas.planeDistance / 4) - 1;
        Debug.Log("Height: " + maxHeight);
        Debug.Log("Width: " + maxWidth);
        controller = gameObject.AddComponent<CharacterController>();
        Debug.Log(this.transform.localPosition.x);
        livesText.text = "Lives Remaining: " + health;
        baseColor = this.GetComponentInChildren<Renderer>().material.color;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
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
        StartCoroutine(TakeDamage());
        
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
}
