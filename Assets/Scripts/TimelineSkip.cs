using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSkip : MonoBehaviour
{
    public PlayableDirector currentDirector;
    bool isSkipped = false;

    private void Update()
    {
        if (isSkipped) { return; }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSkipped = true;
            currentDirector.time = currentDirector.duration;
            Debug.Log("Skipped Cutscene");
        }
    }
}
