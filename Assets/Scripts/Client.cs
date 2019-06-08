using UnityEngine.SceneManagement;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;

public class Client : MonoBehaviour{

    public static Client client;


    private void Awake() {
        if (client == null) {
            DontDestroyOnLoad(gameObject);
            client = this;
        } else if (client != this) {
            Destroy(gameObject);
        }

    }
    public void GetDisplayName(string playFabID, string nextScene) {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() {
            PlayFabId = playFabID,
            ProfileConstraints = new PlayerProfileViewConstraints() {
                ShowDisplayName = true
            }
        },
        result => {
            Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.DisplayName);
            SceneManager.LoadScene(nextScene);
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }
}
