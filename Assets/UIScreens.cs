using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScreens : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI runtimeText;
    public TextMeshProUGUI tokensCollectedText;
    public TextMeshProUGUI tokensBankedText;

    // Update is called once per frame
    public void UpdateDisplay(float runtime, float tokensCollected, float tokensBanked)
    {
        if (gameManager != null)
        {
            // Update the text with the current runtime, tokens collected, and tokens banked
            runtimeText.text = "Runtime: " + gameManager.GetCurrentRuntime().ToString("F2") + " seconds";
            tokensCollectedText.text = "Tokens Collected: " + gameManager.tokensCollected.ToString();
            tokensBankedText.text = "Tokens Banked: " + gameManager.tokensDeposited.ToString();
        }
    }
}
