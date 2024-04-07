using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private float playerSpeed = 100.0f;
    float maxHeight;
    float maxWidth;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        RectTransform rt = canvas.GetComponent<RectTransform>();
        maxHeight = rt.rect.height - 300;
        maxWidth = rt.rect.width - 400;
        Debug.Log("Height: " + maxHeight);
        Debug.Log("Width: " + maxWidth);
        controller = gameObject.AddComponent<CharacterController>();
        Debug.Log(this.transform.localPosition.x);
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
    }
}
