using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonUI : MonoBehaviour
{
    public void LoadPlayScene()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
