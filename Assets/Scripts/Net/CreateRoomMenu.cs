using System.Collections;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace Vegaxys
{
    public class CreateRoomMenu :MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField roomName;
        private PanelsManager panelsManager;

        public void FirstInitialize(PanelsManager panels) {
            panelsManager = panels;
        }

        public void OnClick_CreateRoom() {
            if (!PhotonNetwork.IsConnected) return;
            RoomOptions options = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(roomName.text, options, TypedLobby.Default);
        }

        public override void OnCreatedRoom() {
            Debug.Log("Create room successfully", this);
            panelsManager.CurrentRoom.Show();
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            Debug.LogError("Room creation failed : " + message, this);
        }
    }
}
