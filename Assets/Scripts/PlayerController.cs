using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private float playerSpeed = 100.0f;
    public int health = 3;
    public TextMeshProUGUI livesText;
    public bool parry = false;
    private bool canParry = true;
    float maxHeight;
    float maxWidth;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        RectTransform rt = canvas.GetComponent<RectTransform>();
        // Global height and width are about 100. Not sure how to grab the exact numbers yet
        // maxHeight = rt.rect.height / 2;
        maxHeight = 50;
        // maxWidth = rt.rect.width / 2;
        maxWidth = 100;
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

    public void TakeDamage(GameObject collider)
    {
        if (parry)
        {
            Debug.Log("Nice Parry!");
            Destroy(collider);
            StartCoroutine(SuccessfulParry());
            return;
        }
        health -= 1;
        livesText.text = "Lives Remaining: " + health;
        if(health <= 0)
        {
            Debug.Log("You died!");
        }
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
        canParry = true;
        yield return null;
    }
}
