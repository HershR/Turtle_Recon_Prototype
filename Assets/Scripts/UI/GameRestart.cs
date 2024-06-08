using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    [SerializeField] PlayerStatsSO playerStats;
    public void RestartGame()
    {
        playerStats.ResetAllStats();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
