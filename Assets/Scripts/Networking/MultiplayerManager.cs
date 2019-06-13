using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;

public class MultiplayerManager : MonoBehaviourPunCallbacks{

    public GameObject[] enableObjectsOnConnect;
    public GameObject[] disableObjectsOnConnect;

    private void Start() {
        foreach (var item in enableObjectsOnConnect) {
            item.SetActive(false);
        }
        foreach (var item in disableObjectsOnConnect) {
            item.SetActive(true);
        }
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster() {
        print("Connected to Photon !");
        foreach (var item in enableObjectsOnConnect) {
            item.SetActive(true);
        }
        foreach (var item in disableObjectsOnConnect) {
            item.SetActive(false);
        }
    }
    public void JoinDungeon() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
        print("room join");
    }
    public override void OnJoinRandomFailed(short returnCode, string message) {
        CreateDungeon();
    }
    public override void OnJoinedRoom() {
        SceneManager.LoadScene("RandomDungeon");
    }
    public void CreateDungeon() {
        print("room crée");
        PhotonNetwork.AutomaticallySyncScene = true;

        RoomOptions room = new RoomOptions { MaxPlayers = 5, IsOpen = true, IsVisible = true};
        PhotonNetwork.CreateRoom("RandomDungeon", room, TypedLobby.Default);
    }
}
