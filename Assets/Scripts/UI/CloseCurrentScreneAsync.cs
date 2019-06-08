using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CloseCurrentScreneAsync : MonoBehaviour
{
    public void CloseScene(string scene) {
        SceneManager.UnloadSceneAsync(scene);
    }
}
