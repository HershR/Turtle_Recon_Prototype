using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStatsSO : ScriptableObject, IDataPersistence
{
    /*
     Holds the Players Total Money, Name, Highscore,
     and Holds the Players Stat Levels in a Dict<StatType, StatSO>
     */
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Tokens { get; private set; } = 10;
    [field: SerializeField] public int HighScore { get; private set; } = 0;

    [Header("Stats")]
    [SerializeField] private SerializedDictionary<StatType, StatSO> Stats;
    [field: SerializeField] public SerializedDictionary<StatType, StatSO> PlayerStats { get; private set; }
    [field: SerializeField] public SerializedDictionary<StatType, StatSO> SpawnStats { get; private set; }
    [field: SerializeField] public SerializedDictionary<StatType, StatSO> ResearchStats { get; private set; }

    public bool IsUpgradable(StatType type)
    {
        return Stats[type].IsUpgradable(Tokens);
    }

    public void UpgradeStat(StatType type)
    {
        if (!IsUpgradable(type)) 
        {
            Debug.Log($"Fail to upgrade stat");
            return; 
        }
        Tokens -= Stats[type].GetCost();
        Stats[type].Upgrade();
    }

    public StatSO GetStat(StatType type)
    {        
        Stats.TryGetValue(type, out StatSO stat);
        return stat;
    }

    [ContextMenu("Reset Stats to 0")]
    public void ResetAllStats()
    {
        Name = "Player";
        HighScore = 0;
        Tokens = 20;
        foreach(StatSO stat in Stats.Values)
        {
            stat.ResetLevel();
        }
    }
    public void SetPlayerName(string name)
    {
        Name = name;
    }
    public string GetPlayerName()
    {
        return Name;
    }
    public void AddTokens(int amount)
    {
        Tokens += amount;
    }
    public int GetPlayerHighScore()
    {
        return HighScore;
    }
    public void SetPlayerHighScore(int score)
    {
        HighScore = score;
    }

    public void LoadData(GameData gameData)
    {
        Name = gameData.playerName;
        Tokens = gameData.playerTokens;
        HighScore = gameData.playerHighscore;
        foreach (var stat in Stats.Values)
        {
            stat.LoadData(gameData);
        }
    }

    public void SaveData(GameData gameData)
    {
        gameData.playerName = Name;
        gameData.playerTokens = Tokens;
        gameData.playerHighscore = HighScore;
        foreach (var stat in Stats.Values)
        {
            stat.SaveData(gameData);
        }
    }
}
