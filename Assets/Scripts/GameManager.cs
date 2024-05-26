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
    [SerializeField] private float gameWinTime = 10f;
    [SerializeField] private float gameTimeDelta = 0f;

    private bool isGameOver = false;
    private float distance = 0f;

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }
        if(player.health <= 0)
        {
            isGameOver = true;
            StartCoroutine("GameOver");
            Debug.Log("Loose");
            return;

        }        
        if (gameTimeDelta < gameWinTime)
        {
            gameTimeDelta += Time.deltaTime;
        }
        else
        {
            isGameOver = true;
            gameWinUI?.SetActive(true);
            Debug.Log("WIN");
            return;
        }
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
        gameOverUI?.SetActive(true);
    }
}
