using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        Debug.Log($"Try load scene{sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
