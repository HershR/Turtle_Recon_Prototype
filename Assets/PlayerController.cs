using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerObj;
    private CharacterController controller;
    private float playerSpeed = 100.0f;
    private Vector3 playerVelocity;
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (Mathf.Abs(GetComponentInParent<Transform>().transform.position.x) > maxWidth)
        {
            move[0] = 0;
            Debug.Log("out of bounds on X");
        }
        if (Mathf.Abs(GetComponentInParent<Transform>().transform.position.y) > maxHeight)
        {
            move[1] = 0;
            Debug.Log("Out of bounds on Y");
        }
        // Debug.Log(move);
        controller.Move(move * Time.deltaTime * playerSpeed);
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
