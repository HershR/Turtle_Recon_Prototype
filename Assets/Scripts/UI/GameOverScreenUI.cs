using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameOverScreenUI : MonoBehaviour
{
    public TextMeshProUGUI runtimeText;
    public TextMeshProUGUI tokensCollectedText;
    public TextMeshProUGUI tokensBankedText;

    // Update is called once per frame
    public void UpdateDisplay(float runtime, float maxRunTime, float tokensCollected, float tokensBanked)
    {

        //Debug.Log("Updating Display");
        //Debug.Log("Runtime: " + runtime);
        //Debug.Log("Tokens Collected: " + tokensCollected);
        //Debug.Log("Tokens Banked: " + tokensBanked);

        // Update the text with the current runtime, tokens collected, and tokens banked
        var runTimeSpan = TimeSpan.FromSeconds(runtime);
        var maxRunTimeSpan = TimeSpan.FromSeconds(maxRunTime);
        runtimeText.text = $"Run Time: {string.Format("{0:00}:{1:00}", runTimeSpan.Minutes, runTimeSpan.Seconds)} / {string.Format("{0:00}:{1:00}", maxRunTimeSpan.Minutes, maxRunTimeSpan.Seconds)}";
        tokensCollectedText.text = $"Tokens Collected: {tokensCollected}";
        tokensBankedText.text = $"Tokens Banked: {tokensBanked}";
    }
}
