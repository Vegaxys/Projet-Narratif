using UnityEngine.SceneManagement;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class PlayerInfo{
    public string displayName;
    public string playerID;
    public DateTime createdIn;
    public DateTime LastLogin;
}


public class Client : MonoBehaviour{

    public static Client client;

    public PlayerProfileModel playerInfo;

    private void Awake() {
        if (client == null) {
            DontDestroyOnLoad(gameObject);
            client = this;
        } else if (client != this) {
            Destroy(gameObject);
        }
    }
    public void GetUserData(string playFabId, string nextScene) {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = playFabId,
            Keys = null
        }, result => {
            GetPlayerInfo(playFabId);
            SceneManager.LoadScene(nextScene);
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public void GetPlayerInfo(string playerID) {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() {
            PlayFabId = playerID
        }, result => {
            playerInfo = result.PlayerProfile;
        }, (error) => {
            Debug.LogWarning("Player non trouvé");
        });
    }
}
