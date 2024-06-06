using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] EnvironmentGenerator generator;

    [Header("Game over Related")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] Transform playerFinalTransform;
    

    [Header("Game Win Related")]
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] public float gameWinTime = 10f;
    [SerializeField] public float gameTimeDelta = 0f;

    private bool isGameOver = false;

    [SerializeField] private float distance = 0f;
    [field: SerializeField] public float tokensCollected { get; private set; } = 0f;
    [field: SerializeField] public float tokensDeposited { get; private set; } = 0f;

    private float gameTime = 0f;

    private void OnEnable()
    {
        player.onTokenCollect.AddListener(TokenCollected);
        player.onTokenBanked.AddListener(TokenBanked);
    }

    private void OnDisable()
    {
        player.onTokenCollect.RemoveListener(TokenCollected);
        player.onTokenBanked.RemoveListener(TokenBanked);
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        gameTime += Time.deltaTime; // Increment the runtime

        if (player.health <= 0)
        {
            Debug.Log("Loose");
            isGameOver = true;
            StartCoroutine("GameOver");
            return;

        }        
        if (gameTimeDelta < gameWinTime)
        {
            gameTimeDelta += Time.deltaTime;
        }
        else
        {
            Debug.Log("WIN");
            isGameOver = true;
            gameWinUI.gameObject.SetActive(true);
            return;
        }
        distance += generator.GetSpeed() * Time.deltaTime;
    }

    private IEnumerator GameOver()
    {
        player.enabled = false;
        //move player off screen
        while(player.transform.position.z > Camera.main.transform.position.z)
        {
            Vector3 currentPos = player.transform.position;
            Vector3 newPos = Vector3.MoveTowards(currentPos, playerFinalTransform.position, Time.deltaTime * player.playerSpeed);
            player.transform.position = newPos;
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Game Over UI Active");
        gameOverUI.gameObject.SetActive(true);

        Debug.Log("Runtime: " + gameTime);
        Debug.Log("Tokens Collected: " + tokensCollected);
        Debug.Log("Tokens Banked: " + tokensDeposited);

        Debug.Log("Before calling UpdateLoseScreenUI");
        // update runtime display when game is over
        UpdateLoseScreenUI();
        Debug.Log("After calling UpdateLoseScreenUI");

    }

    private void TokenCollected()
    {
        tokensCollected += 1;
        Debug.Log("Token Collected. Total: " + tokensCollected);

    }
    private void TokenBanked()
    {
        tokensDeposited += 1f;
        Debug.Log("Token Banked. Total: " + tokensDeposited);
    }

    // Method to get the current runtime
    public float GetCurrentRuntime()
    {
        return gameTime;
    }

    private void UpdateLoseScreenUI()
    {
        Debug.Log("Calling UpdateDisplay on UIScreens");
        UIScreens uiScreens = gameOverUI.GetComponent<UIScreens>();
        if (uiScreens != null)
        {
            uiScreens.UpdateDisplay(GetCurrentRuntime(), tokensCollected, tokensDeposited);
        }
        else
        {
            Debug.LogError("UIScreens component not found on gameOverUI");
        }
    }

    // show the win screen
    private void ShowWinScreen()
    {
        gameWinUI.gameObject.SetActive(true);
    }
}
