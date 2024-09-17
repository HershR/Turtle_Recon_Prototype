using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerSetup : MonoBehaviour
{
    [SerializeField] private string playerName;
    public void SetName(string name) { playerName = name; }

    public void CreatePlayer()
    {
        //create new highscore entry
        if(playerName == null)
        {
            return;
        }
        DataPersistenceManager.Instance.NewGame(playerName);
    }
}
