using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField] private EnvironmentGenerator environmentGenerator;
    private Renderer objectRenderer;
    private float currentYOffset = 0f;
    [SerializeField, Range(0, 100)] private float speedModifer = 1f;

    private int offsetID;

    private void OnEnable()
    {
        offsetID = Shader.PropertyToID("_Offset");
    }
    void Start()
    {
        // Get the Renderer component from the object this script is attached to
        objectRenderer = GetComponent<Renderer>();

        //// Check if the Renderer component and material exist
        //if (objectRenderer == null || objectRenderer.material == null)
        //{
        //    Debug.LogError("Renderer or Material not found on the object.");
        //}
        //currentYOffset = objectRenderer.material.mainTextureOffset.y;
    }

    private void Update()
    {
        currentYOffset -= Time.deltaTime * environmentGenerator.GetSpeed() / speedModifer;
        Vector2 offset = new Vector2(0f, currentYOffset);
        objectRenderer.material.SetVector(offsetID, offset);
    }
}
