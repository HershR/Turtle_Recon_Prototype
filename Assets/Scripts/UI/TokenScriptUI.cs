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
        player.onTokenCollect += UpdateText;
        player.onTokenBanked += UpdateText;
    }


    private void OnDisable()
    {
        player.onTokenCollect -= UpdateText;
        player.onTokenBanked -= UpdateText;
    }
    private void UpdateText()
    {
        //add delay so gamemanager can update itself
        StartCoroutine("UpdateTextDelay");
    }
    private IEnumerator UpdateTextDelay()
    {
        yield return new WaitForSeconds(0.01f);
        tokenText.text = $"{player.tokenCount}|{gameManager.TokensDeposited}";
    }
}
