using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public enum SceneIndexes { Title = 0, Game = 1, Store = 2, Tutorial = 3, Quit = -1};
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public GameObject loadingScreen;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void LoadTitle()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Title));
        StartCoroutine(GetSceneLoadProgress());
    }
    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Game));
        StartCoroutine(GetSceneLoadProgress());
    }
    public void LoadStore()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Store));
        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadTutorial()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Tutorial));
        StartCoroutine(GetSceneLoadProgress());
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator GetSceneLoadProgress()
    {
        DataPersistenceManager.Instance.SaveGame();
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }
        loadingScreen.gameObject.SetActive(false);
        scenesLoading.Clear();
    }

    public void LoadScene(SceneIndexes scene)
    {
        switch (scene)
        {
            case SceneIndexes.Title:
                LoadTitle();
                break;
            case SceneIndexes.Game:
                LoadGame();
                break;
            case SceneIndexes.Store:
                LoadStore();
                break;
            case SceneIndexes.Tutorial:
                LoadTutorial();
                break;
            case SceneIndexes.Quit:
                QuitGame();
                break;
        }
    }

}
