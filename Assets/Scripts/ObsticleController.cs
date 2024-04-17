using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleController : MonoBehaviour
{
    // Update is called once per frame
    float maxHeight;
    float maxWidth;

    private void Awake()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        RectTransform rt = canvas.GetComponent<RectTransform>();
        maxHeight = rt.rect.height/2;
        maxWidth = rt.rect.width/2;
        transform.localPosition = new Vector3(Random.Range(-1 * maxWidth, maxWidth), Random.Range(-1 * maxHeight, maxHeight), 5000);
    }

    void Update()
    {
        if (this.transform.position.z < -10)
        {
            Debug.Log("Obsticle destoyed (went behind camera)");
            Destroy(this.gameObject);
        }
        transform.position += new Vector3(0, 0, -1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag != "Player")
        {
            return;
        }
        collision.gameObject.GetComponent<PlayerController>().TakeDamage(this.gameObject);
        Destroy(this.gameObject);
        Debug.Log("You got hit");
    }
}
