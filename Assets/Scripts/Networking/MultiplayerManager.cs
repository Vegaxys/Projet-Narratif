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
    public void JoinBR_Solo() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message) {
        CreateBR_Solo();
    }
    public void CreateBR_Solo() {
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomOptions room = new RoomOptions { MaxPlayers = 10, IsOpen = true, IsVisible = true};
        PhotonNetwork.CreateRoom("BR_Solo", room, TypedLobby.Default);
    }
    public override void OnJoinedRoom() {
        SceneManager.LoadScene("BR_Solo");
    }
}
