﻿using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using Vegaxys;

public class MultiplayerManager : MonoBehaviourPunCallbacks{

    public ChampionSelection champion;

    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
        print("Connecting to Photon...");
    }
    public override void OnConnectedToMaster() {
        print("Connected to Photon !");
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
        int randomName = Random.Range(0, 10000);
        PhotonNetwork.CreateRoom("Scene#" + randomName, room, TypedLobby.Default);
    }
    public override void OnJoinedRoom() {
        PlayerInfos.instance.player.playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        SceneManager.LoadScene("GameScene");
    }

    public void SetUsername(string input) {
        PlayerInfos.instance.player.playerName = input;
        PhotonNetwork.LocalPlayer.NickName = input;
    }
}