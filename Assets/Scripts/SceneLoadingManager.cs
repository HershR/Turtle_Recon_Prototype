using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneIndexes { Game = 0, Store = 1 };
public class SceneLoadingManager : MonoBehaviour
{
    public GameObject loadingScreen;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Game));
        //StartCoroutine(GetSceneLoadProgress());
    }
    public void LoadStore()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Store));
        //StartCoroutine(GetSceneLoadProgress());
    }


    //public IEnumerator GetSceneLoadProgress()
    //{
    //    for (int i = 0; i < scenesLoading.Count; i++)
    //    {
    //        while (!scenesLoading[i].isDone)
    //        {
    //            yield return null;
    //        }
    //    }
    //    loadingScreen.gameObject.SetActive(false);
    //    scenesLoading.Clear();
    //}
}
