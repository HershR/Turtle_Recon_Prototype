using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTimer : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        float progress = Mathf.Clamp01(gameManager.gameTimeDelta / gameManager.gameWinTime);
        progressBar.value = progress;
        progressText.text = Mathf.Round(progress * 100) + "%";
    }
}
