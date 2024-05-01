using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleController : MonoBehaviour
{
    // Update is called once per frame
    float maxHeight;
    float maxWidth;

    // give it random movement on (x) similar to (gust of wind)

    private void Awake()
    {
        RectTransform rt = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        maxHeight = rt.rect.height / 2;
        maxWidth = rt.rect.width / 2;
        gameObject.transform.localPosition = new Vector3(Random.Range(-1 * maxWidth, maxWidth), Random.Range(-1 * maxHeight, maxHeight), 5000);
    }

    private void Start()
    {
        // gameObject.transform.rotation.eulerAngles.Set(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    void Update()
    {
        // gameObject.transform.rotation.eulerAngles.Set(gameObject.transform.rotation.eulerAngles.x,
        //    gameObject.transform.rotation.eulerAngles.y,
        //    gameObject.transform.rotation.eulerAngles.z + 1);
        if (gameObject.transform.position.z < -10)
        {
            Debug.Log("Obsticle destoyed (went behind camera)");
            Destroy(gameObject);
        }
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
    }
}
