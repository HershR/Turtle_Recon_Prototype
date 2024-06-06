using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] EnvironmentGenerator generator;
    [SerializeField] ObsticleSpawner spawner;

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

        distance += generator.GetSpeed() * Time.deltaTime;

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
            spawner.gameObject.SetActive(false);
            isGameOver = true;
            gameWinUI.gameObject.SetActive(true);
            return;
        }
    }

    private IEnumerator GameOver()
    {
        player.enabled = false;
        //move player off screen
        gameOverUI.gameObject.SetActive(true);
        while(player.transform.position.z > Camera.main.transform.position.z)
        {
            Vector3 currentPos = player.transform.position;
            Vector3 newPos = Vector3.MoveTowards(currentPos, playerFinalTransform.position, Time.deltaTime * player.playerSpeed);
            player.transform.position = newPos;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Runtime: " + gameTimeDelta);
        Debug.Log("Tokens Collected: " + tokensCollected);
        Debug.Log("Tokens Banked: " + tokensDeposited);

        UpdateLoseScreenUI();

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


    private void UpdateLoseScreenUI()
    {
        Debug.Log("Calling UpdateDisplay on UIScreens");
        GameOverScreenUI uiScreens = gameOverUI.GetComponent<GameOverScreenUI>();
        if (uiScreens != null)
        {
            uiScreens.UpdateDisplay(gameTimeDelta, gameWinTime, tokensCollected, tokensDeposited);
        }
        else
        {
            Debug.LogError("UIScreens component not found on gameOverUI");
        }
    }
}
