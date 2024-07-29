using UnityEngine;

public class TokenCollectVisual : MonoBehaviour
{
    private Vector3 targetPos;
    private bool targetSet;
    private float speed = 10f;

    public void Init(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        this.targetSet = true;
    }

    private void Update()
    {
        if (targetSet)
        {
            if (transform.position != targetPos) { 
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
