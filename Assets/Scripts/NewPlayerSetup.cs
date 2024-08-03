using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private string playerName;
    public void SetName(string name) { playerName = name; }

    public void CreatePlayer()
    {
        //create new highscore entry
        playerStatsSO.ResetAllStats();
        playerStatsSO.SetPlayerName(playerName.Length > 0 ? playerName : "Player 1");
    }
}
