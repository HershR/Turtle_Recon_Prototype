using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public SceneIndexes scene;
    public void LoadScene()
    {
        SceneLoadingManager.instance.LoadScene(scene);
    }
}
