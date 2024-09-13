using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenScriptUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI tokenText;

    private void OnEnable()
    {
        player.onTokenCollect += UpdateTextCollected;
        player.onTokenBanked += UpdateBankedText;
    }


    private void OnDisable()
    {
        player.onTokenCollect -= UpdateTextCollected;
        player.onTokenBanked -= UpdateBankedText;
    }
    private void UpdateTextCollected()
    {
        //add delay so gamemanager can update itself
        StartCoroutine(UpdateTextDelay());
    }
    public void UpdateBankedText()
    {
        StartCoroutine(UpdateTextDelay(1f));
    }
    private IEnumerator UpdateTextDelay(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        tokenText.text = $"{player.tokenCount}|{gameManager.TokensDeposited}";
    }
}
