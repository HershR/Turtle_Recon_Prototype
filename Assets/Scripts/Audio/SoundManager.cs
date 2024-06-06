using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource soundObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn in gameObject
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);
        // Assign the audioClip
        audioSource.clip = audioClip;
        // Assign volume
        audioSource.volume = volume;
        // Play sound
        audioSource.Play();
        // Get length of sound clip
        float clipLength = audioSource.clip.length;
        // Destroy clip after it's done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
