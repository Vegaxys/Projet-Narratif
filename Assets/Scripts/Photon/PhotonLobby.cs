using System.Collections.Generic;
using System.Collections;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Vegaxys {
    public class PhotonLobby :MonoBehaviourPunCallbacks
    {
        #region Variables 

        public static PhotonLobby instance;
        public GameObject queueButton;
        public GameObject cancelButton;

        #endregion


        #region Singleton

        private void Awake() {
            instance = this;
        }

        #endregion


        #region MonoBehaviours Callbacks

        void Start() {
            queueButton.SetActive(false);
            cancelButton.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
        }

        #endregion


        #region Photon Methods

        public override void OnConnectedToMaster() {
            Debug.Log("Player is connected to " + PhotonNetwork.CloudRegion + " server !");
            queueButton.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.Log("Random join failed");
            CreateRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            Debug.Log("Room cannot be created");
            CreateRoom();
        }

        public override void OnJoinedRoom() {
            Debug.Log("Player is now in a room");
            PlayerInfos.instance.player.playerID = PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonNetwork.LoadLevel(1);
        }

        #endregion


        #region Public Methods

        public void Button_QueueClicked() {
            queueButton.SetActive(false);
            cancelButton.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
        }

        public void Button_CancelQueueClicked() {
            queueButton.SetActive(true);
            cancelButton.SetActive(false);
            PhotonNetwork.LeaveRoom();
        }

        public void SetCharacterID(int ID) {
            PlayerInfos.instance.player.avatarID = ID;
            switch (ID) {
                case 0:
                    PlayerInfos.instance.player.avatarName = "Epsilon";
                    PlayerInfos.instance.player.className = "Assassin";
                    break;
                case 1:
                    PlayerInfos.instance.player.avatarName = "Exilé";
                    PlayerInfos.instance.player.className = "Tireur";
                    break;
                case 2:
                    PlayerInfos.instance.player.avatarName = "Native";
                    PlayerInfos.instance.player.className = "Tank";
                    break;
                case 3:
                    PlayerInfos.instance.player.avatarName = "Oméga";
                    PlayerInfos.instance.player.className = "Support";
                    break;
                default:
                    PlayerInfos.instance.player.avatarName = "Error";
                    PlayerInfos.instance.player.className = "Error";
                    break;
            }
        }

        public void PlayerNameUpdated(string result) {
            PhotonNetwork.NickName = result;
            PlayerInfos.instance.player.playerName = result;
        }

        #endregion


        #region Private Methods

        private void CreateRoom() {
            Debug.Log("Creating a new room...");
            string randomRoomName = "Room#" + Random.Range(0, 10000);
            RoomOptions options = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(randomRoomName, options);
        }

        private void RandomUsername() {
            string randomName = "Player#" + Random.Range(0, 10000);
            PhotonNetwork.NickName = randomName;
            PlayerInfos.instance.player.playerName = randomName;
        }

        #endregion
    }
}
