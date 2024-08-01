using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStatsSO;
    private string playerName;
    public void SetName(string name) { playerName = name; }

    public void CreatePlayer()
    {
        playerStatsSO.ResetAllStats();
        playerStatsSO.SetPlayerName(playerName);
    }


}
