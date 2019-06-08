using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MasterManager : MonoBehaviour{

    public void Register() {
        SceneManager.LoadSceneAsync("Register", LoadSceneMode.Additive);
    }
    public void Login() {
        SceneManager.LoadSceneAsync("Login", LoadSceneMode.Additive);
    }
}
