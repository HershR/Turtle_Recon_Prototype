using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Dan.Main;

public class ScoreManager : MonoBehaviour
{   
    private string publicLeaderboardKey = 
        "de6c979ad751bd0d931daf189b9d3102edc7d5e05769a41dae565afea6e1700f";

    public void SetLeaderboardEntry(string username, int score) {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) => {
        }));
    }
}
