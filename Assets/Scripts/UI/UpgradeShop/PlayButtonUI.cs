using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonUI : MonoBehaviour
{
    [SerializeField] private AudioClip PlaySound;
    public void LoadPlayScene()
    {
        SoundManager.instance.PlaySoundClip(PlaySound, transform, 1f);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
