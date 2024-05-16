using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleController : MonoBehaviour
{
    // Update is called once per frame
    float maxHeight;
    float maxWidth;
    public InteractableType obsticle_type;

    // give it random movement on (x) similar to (gust of wind)

    private void Awake()
    {
        // Height = 2 * Tan(0.5 * field_of_view) * distance;
        // Recall aspect Ratio = 16:9
        // Width =
        // fov =
        GameObject camera = GameObject.Find("Main Camera");
        GameObject player = GameObject.Find("Player");
        float fov = camera.GetComponent<Camera>().fieldOfView;
        float dist = Vector3.Distance(camera.GetComponent<Transform>().transform.position, player.GetComponent<Transform>().transform.position);
        dist = Mathf.Abs(camera.GetComponent<Transform>().transform.position.z - player.GetComponent<Transform>().transform.position.z);
        maxHeight = Mathf.Abs(2 * Mathf.Tan(0.5f * fov) * dist * camera.transform.localScale.x) * 0.9f;
        maxWidth = maxHeight * 1.78f;
        // Debug.Log("MH: " + maxHeight);
        // Debug.Log("MW: " + maxWidth);
        transform.position = new Vector3(Random.Range(-1 * maxWidth, maxWidth) / transform.localScale.x,
                                        Random.Range(-1 * maxHeight, maxHeight) / transform.localScale.y,
                                        -1 * player.transform.position.z) + player.transform.position;
        Debug.Log("X, Y: " + transform.position.x + ", " + transform.position.y);
        transform.Rotate(new Vector3(0, 0, Random.Range(-180, 180)));
    }

    private void Start()
    {
        // gameObject.transform.rotation.eulerAngles.Set(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        Debug.Log("Ima "+ obsticle_type + " type obsticle");
    }

    void Update()
    {
        // gameObject.transform.rotation.eulerAngles.Set(gameObject.transform.rotation.eulerAngles.x,
        //    gameObject.transform.rotation.eulerAngles.y,
        //    gameObject.transform.rotation.eulerAngles.z + 1);
        
        transform.position += new Vector3(0, 0, -0.01f);
        transform.Rotate(new Vector3(0, 0, 0.01f));
        if (transform.position.z > (transform.position.z  + 0.5f))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.tag);
        
        if (collision.gameObject.tag == "Despawner")
        {
            Debug.Log("Obsticle destoyed (went behind camera)");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "Player")
        {
            return;
        }
        collision.gameObject.GetComponent<PlayerController>().OnCollision(this.gameObject);
    
        Destroy(this.gameObject);
        Debug.Log("You got hit");
    }
}
