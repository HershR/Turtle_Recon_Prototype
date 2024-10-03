using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string playerName;
    public int playerTokens;
    public int playerHighscore;
    public StatType activeResearch;
    public Dictionary<StatType, int> researchProgress = new();
    public Dictionary<StatType, int> playerStats = new();
    public GameData() 
    {
    
    }
}
