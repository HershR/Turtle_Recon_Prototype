using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTimer : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public Slider progressBar;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        float progress = Mathf.Clamp01(gameManager.gameTimeDelta / gameManager.gameWinTime);
        progressBar.value = progress;
    }
}
