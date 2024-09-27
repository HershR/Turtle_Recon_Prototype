using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey = 
        "de6c979ad751bd0d931daf189b9d3102edc7d5e05769a41dae565afea6e1700f";
    
    private void Start() {
        GetLeaderboard();
    }
    public void GetLeaderboard() {
        try
        {
            LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; ++i) {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
        }
        catch (Exception e)
        {
            Debug.Log("Error retrieving leaderboard.");
        }
    }

    public void SetLeaderboardEntry(string username, int score) {
        try
        {
           LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) => {
            GetLeaderboard();
        })); 
        }
        catch (Exception e)
        {
            Debug.Log("Error uploading leaderboard entry.");
        }
    }
}
