using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public PlayerController player;
    [SerializeField] protected EnvironmentGenerator generator;
    [SerializeField] protected ObsticleSpawner obstacleSpawner;
    [SerializeField] protected DroneSpawner droneSpawner;

    [Header("Game over Related")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] Transform playerFinalTransform;


    [Header("Game Win Related")]
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] public float gameWinTime = 10f;
    [SerializeField] public float gameTimeDelta = 0f;

    protected bool isGameOver = false;

    [SerializeField] protected float distance = 0f;
    [field: SerializeField] public float tokensCollected { get; private set; } = 0f;
    [field: SerializeField] public float tokensDeposited { get; private set; } = 0f;

    private void OnEnable()
    {
        player.onTokenCollect += TokenCollected;
        player.onTokenBanked += TokenBanked;
    }

    private void OnDisable()
    {
        player.onTokenCollect -= TokenCollected;
        player.onTokenBanked -= TokenBanked;
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;
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
            UpdateLoseScreenUI();
            gameOverUI.gameObject.SetActive(true);
            OnGameOver();
            StartCoroutine(RemovePlayer());
            return;

        }
        if (gameTimeDelta < gameWinTime)
        {
            gameTimeDelta += Time.deltaTime;
        }
        else
        {
            Debug.Log("WIN");
            OnGameOver();
            gameWinUI.gameObject.SetActive(true);
            return;
        }
    }

    protected void OnGameOver()
    {
        isGameOver = true;
        droneSpawner.RecallDone();
        player.enabled = false;
        obstacleSpawner.enabled = false;
        droneSpawner.enabled = false;
        //generator.enabled = false;
    }

    protected IEnumerator RemovePlayer()
    {
        //move player off screen
        while (player.transform.position.z > Camera.main.transform.position.z)
        {
            Vector3 currentPos = player.transform.position;
            Vector3 newPos = Vector3.MoveTowards(currentPos, playerFinalTransform.position, Time.deltaTime * player.playerSpeed);
            player.transform.position = newPos;
            yield return new WaitForEndOfFrame();
        }
        player.gameObject.SetActive(false);
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
