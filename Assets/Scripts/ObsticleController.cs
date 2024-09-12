using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleController : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] public InteractableType obsticle_type;
    [SerializeField] public float speed = 0;
    // give it random movement on (x) similar to (gust of wind)
    [SerializeField] private float delZ;
    // [SerializeField] private PlayerController playerScript;
    private void Awake()
    {
        //delZ = delZ = Camera.main.transform.position.z;
        //Vector3 pos = Camera.main.WorldToViewportPoint(new Vector3(Random.Range(-1, 1), Random.Range(-0.5f, 1.5f), transform.position.z));
        //pos.x = Mathf.Clamp01(pos.x);
        //pos.y = Mathf.Clamp01(pos.y);
        //transform.position = Camera.main.ViewportToWorldPoint(pos);
        //Debug.Log("new obs coords: (X, Y): " + transform.position.x + ", " + transform.position.y);
    }

    private void Start()
    {
        // gameObject.transform.rotation.eulerAngles.Set(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        //Debug.Log("Ima " + obsticle_type + " type obsticle");
        // playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * speed * 0.5f);
        transform.Rotate(new Vector3(0, 0, 0.01f));
        if (transform.position.z < delZ)
        {
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.gameObject.name);
        // PlayerController playerScript = collision.GetComponent<PlayerController>();

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
        Debug.Log("You got hit");

        /*if (playerScript != null)
        {
            collision.gameObject.GetComponent<PlayerController>().OnCollision(this.gameObject);
            Debug.Log("You got hit");
        }
        else
        {
            Debug.Log("Player controller = Null");
        }*/

        Destroy(this.gameObject);

    }
}