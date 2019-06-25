using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class TestConneect :MonoBehaviourPunCallbacks
    {
        [SerializeField] private int gameVersion;
        void Start() {
            print("Connecting to sever...");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = MasterManager.GameSettings.PlayerName;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster() {
            print("Connected to sever!");
            print(PhotonNetwork.LocalPlayer.NickName);
            if(!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby() {
            print("Joined the lobby!");
        }

        public override void OnDisconnected(DisconnectCause cause) {
            print("Disconnected to sever! Cause :" + cause.ToString());
        }
    }
}
