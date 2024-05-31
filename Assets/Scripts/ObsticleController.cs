using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleController : MonoBehaviour
{
    // Update is called once per frame
    float maxHeight;
    float maxWidth;
<<<<<<< Updated upstream
    public InteractableType obsticle_type;
=======
>>>>>>> Stashed changes

    // give it random movement on (x) similar to (gust of wind)

    private void Awake()
    {
<<<<<<< Updated upstream
        /* Height = 2 * Tan(0.5 * field_of_view) * distance;
        // Recall aspect Ratio = 16:9
        // Width =
        // fov =
        GameObject camera = GameObject.Find("Main Camera");
        GameObject player = GameObject.Find("Player");
        float fov = camera.GetComponent<Camera>().fieldOfView;
        float dist = Vector3.Distance(camera.GetComponent<Transform>().transform.position, player.GetComponent<Transform>().transform.position);
        dist = Mathf.Abs(camera.GetComponent<Transform>().transform.position.z - player.GetComponent<Transform>().transform.position.z);
        maxHeight = Mathf.Abs(Mathf.Tan(0.5f * fov) * dist);
        maxWidth = maxHeight * 1.78f;
        // Debug.Log("MH: " + maxHeight);
        // Debug.Log("MW: " + maxWidth);
        transform.position = new Vector3(Random.Range(-1 * maxWidth, maxWidth),
                                        Random.Range(-1 * maxHeight, maxHeight),
                                       transform.position.z);*/

        Vector3 pos = Camera.main.WorldToViewportPoint(new Vector3(Random.Range(-1, 1), Random.Range(0, 1), transform.position.z));
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        Debug.Log("new obs coords: (X, Y): " + transform.position.x + ", " + transform.position.y);
        // transform.Rotate(new Vector3(0, 0, Random.Range(-180, 180)));
=======
        RectTransform rt = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        maxHeight = rt.rect.height / 2;
        maxWidth = rt.rect.width / 2;
        gameObject.transform.localPosition = new Vector3(Random.Range(-1 * maxWidth, maxWidth), Random.Range(-1 * maxHeight, maxHeight), 5000);
>>>>>>> Stashed changes
    }

    private void Start()
    {
        // gameObject.transform.rotation.eulerAngles.Set(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
<<<<<<< Updated upstream
        Debug.Log("Ima "+ obsticle_type + " type obsticle");
=======
>>>>>>> Stashed changes
    }

    void Update()
    {
        // gameObject.transform.rotation.eulerAngles.Set(gameObject.transform.rotation.eulerAngles.x,
        //    gameObject.transform.rotation.eulerAngles.y,
        //    gameObject.transform.rotation.eulerAngles.z + 1);
<<<<<<< Updated upstream
        
        transform.position += new Vector3(0, 0, -0.01f / transform.lossyScale.z);
        transform.Rotate(new Vector3(0, 0, 0.01f));
        if (transform.position.z < Camera.main.transform.position.z)
        {
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.tag == "Despawner")
=======
        if (gameObject.transform.position.z < -10)
>>>>>>> Stashed changes
        {
            Debug.Log("Obsticle destoyed (went behind camera)");
            Destroy(gameObject);
        }
<<<<<<< Updated upstream
        else if (collision.gameObject.tag != "Player")
        {
            return;
        }
        
        PlayerController playerScript = collision.GetComponent<PlayerController>();
        if (playerScript != null)
        {
            playerScript.OnCollision(this.gameObject);
            Debug.Log("You got hit");
        }
        else
        {
            Debug.Log("Player controller = Null");
        }
    
        Destroy(this.gameObject);
        
=======
        transform.position += new Vector3(0, 0, -0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag != "Player")
        {
            return;
        }
        collision.gameObject.GetComponent<PlayerController>().OnCollision(this.gameObject);
        Destroy(this.gameObject);
        Debug.Log("You got hit");
>>>>>>> Stashed changes
    }
}
