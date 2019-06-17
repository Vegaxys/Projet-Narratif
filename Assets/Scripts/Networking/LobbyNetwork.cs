using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Vegaxys
{
    public class LobbyNetwork :MonoBehaviourPunCallbacks, ILobbyCallbacks
    {
        [SerializeField] private Button createRoomButton;
        private int roomSize;

        void Start() {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster() {
            Debug.Log("Player is connected to " + PhotonNetwork.CloudRegion + " server !");
        }

        /*
        #region Public Fields

        public static LobbyNetwork instance;
        public string sceneName;

        public string roomName;
        public byte maxPlayers = 4;

        #endregion


        #region Private Serializable Fields

        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private GameObject roomPrefab;
        [SerializeField] private Transform roomParent;

        #endregion


        #region Private Fields

        private bool isConnecting;

        #endregion

        void Awake() {
            instance = this;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start() {
            PhotonNetwork.ConnectUsingSettings();
            createRoomButton.interactable = false;
            cancelButton.interactable = false;
        }

        public void CreateRoom() {
            if (!PhotonNetwork.IsConnectedAndReady) {
                return;
            }
            RoomOptions roomOpt = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayers };
            if (PhotonNetwork.JoinOrCreateRoom(roomName, roomOpt, null)) {
                print("Room '" + roomName + "' will be created");
            } else {
                print("Room failed created");
            }
        }

        public void SetRoomName(string _name) {
            roomName = _name;
        }

        public override void OnConnectedToMaster() {
            Debug.Log("Player is connected to Photon");
            PhotonNetwork.AutomaticallySyncScene = true;
            createRoomButton.interactable = true;
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby() {
            Debug.Log("Player has joined to the lobby");
        }

        public override void OnDisconnected(DisconnectCause cause) {
            Debug.LogWarningFormat("Vegaxys/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnCreatedRoom() {
            Debug.Log("Room successfuly created");
        }
        */
    }
}
