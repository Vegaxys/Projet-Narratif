using System.Collections.Generic;
using System.Collections;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Vegaxys
{
    public class Matchmaking_LobbyController :MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button lobbyConnectedButton;       //Join a lobby
        [SerializeField] private GameObject lobbyPanel;             //Panel for lobby
        [SerializeField] private GameObject mainPanel;              //main panel
        [SerializeField] private Transform roomContainer;           //panel that contains rooms
        [SerializeField] private GameObject roomListingPrefab;      //Prefab of a room for the UI

        [SerializeField] private TMP_InputField playerName;         //Join a lobby

        private string roomName;
        private List<RoomInfo> roomListing;

        void Start() {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster() {
            Debug.Log("Player is connected to " + PhotonNetwork.CloudRegion + " server !");
            PhotonNetwork.AutomaticallySyncScene = true;
            lobbyConnectedButton.interactable = true;
            roomListing = new List<RoomInfo>();

            if (PlayerPrefs.HasKey("Pseudo")) {
                if (PlayerPrefs.GetString("Pseudo") == "") {
                    PhotonNetwork.NickName = "Player#" + Random.Range(0, 10000);
                } else {
                    PhotonNetwork.NickName = PlayerPrefs.GetString("Pseudo");
                }
            } else {
                PhotonNetwork.NickName = "Player#" + Random.Range(0, 10000);
            }
            playerName.text = PhotonNetwork.NickName;
        }

        public void PlayerNameUpdated(string result) {
            PhotonNetwork.NickName = result;
            PlayerPrefs.SetString("Pseudo", result);
        }

        public void JoinLobbyOnClick() {
            lobbyPanel.SetActive(true);
            mainPanel.SetActive(false);
            PhotonNetwork.JoinLobby();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList) {
            int index;
            foreach (RoomInfo room in roomList) {
                if(roomList != null) {
                    index = roomListing.FindIndex(ByName(room.Name));
                } else {
                    index = -1;
                }
                if(index != -1) {               //room closed
                    roomListing.RemoveAt(index);
                    Destroy(roomContainer.GetChild(index).gameObject);
                }
                if(room.PlayerCount > 0) {      //A new room appears
                    roomListing.Add(room);
                    ListRoom(room);
                }
            }
            base.OnRoomListUpdate(roomList);
        }

        static System.Predicate<RoomInfo> ByName(string name) {
            return delegate (RoomInfo room) {
                return room.Name == name;
            };
        }

        private void ListRoom(RoomInfo room) {
            if(room.IsOpen && room.IsVisible) {
                GameObject roomInstance = Instantiate(roomListingPrefab, roomContainer);
                RoomButton roomButton = roomInstance.GetComponent<RoomButton>();
                roomButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
            }
        }
        public void OnRoomChanged(string name) {
            roomName = name;
        }

        public void CreateRoom() {
            print("Creating room now...");
            RoomOptions options = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(roomName, options);
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            print("Room creation failed");
        }

        public void MatchMakingCancel() {
            mainPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            PhotonNetwork.LeaveLobby();
        }
    }
}
