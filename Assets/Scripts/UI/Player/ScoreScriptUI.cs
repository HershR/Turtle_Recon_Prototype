using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScriptUI : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerStatsSO stats;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        // scoreText.text = "Score: " + player.playerScore.ToString("0,#");
        scoreText.text = $"Score: {player.playerScore:n0}";
        highScoreText.text = $"High Score: {stats.HighScore:n0}";
    }
}
