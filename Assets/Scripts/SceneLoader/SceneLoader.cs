using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public SceneIndexes scene;
    public void LoadScene()
    {
        SceneTransitionManager.instance.LoadScene(scene);
    }
}
