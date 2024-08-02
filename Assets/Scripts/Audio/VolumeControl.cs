using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public GameObject volController;
    private bool volViewable = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            ToggleVolumeControl();
        }
    }
    public void ToggleVolumeControl()
    {
        volViewable = !volViewable;
        volController.SetActive(volViewable);
        Time.timeScale = volViewable ? 0 : 1;
    }
}
