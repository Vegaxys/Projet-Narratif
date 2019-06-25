using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class PlayerListingMenu :MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform content;
        [SerializeField] private PlayerObjectUI playerPrefab;
        private List<PlayerObjectUI> playerUIs = new List<PlayerObjectUI>();
        private PanelsManager panelsManager;

        public override void OnEnable() {
            base.OnEnable();
            GetCurrentPlayerInRoom();
        }

        public override void OnDisable() {
            base.OnDisable();
            for (int i = 0; i < playerUIs.Count; i++) {
                Destroy(playerUIs[i].gameObject);
            }
            playerUIs.Clear();
        }

        public void FirstInitialize(PanelsManager panels) {
            panelsManager = panels;
        }

        private void GetCurrentPlayerInRoom() {
            if (!PhotonNetwork.IsConnected) return;
            if (PhotonNetwork.CurrentRoom == null) return;
            foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players) {
                AddPlayer(playerInfo.Value);
            }
        }

        public void AddPlayer(Player _player) {
            int index = playerUIs.FindIndex(x => x.player == _player);
            if (index != -1) {
                playerUIs[index].SetPlayerInfo(_player);
            } else { 
                PlayerObjectUI objectUI = Instantiate(playerPrefab, content);
                if (objectUI != null) {
                    objectUI.SetPlayerInfo(_player);
                    playerUIs.Add(objectUI);
                }
            }
        }

        public override void OnMasterClientSwitched(Player newMasterClient) {
            panelsManager.CurrentRoom.leaveRoomMenu.OnClick_LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer) {
            AddPlayer(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            int index = playerUIs.FindIndex(x => x.player.NickName == otherPlayer.NickName);
            if (index != -1) {
                Destroy(playerUIs[index].gameObject);
                playerUIs.RemoveAt(index);
            }
        }

        public void OnClick_StartGame() {
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(1);
            }
        }
    }
}
