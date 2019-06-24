using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class LeaveRoomMenu :MonoBehaviour
    {

        private PanelsManager panelsManager;

        public void FirstInitialize(PanelsManager panels) {
            panelsManager = panels;
        }

        public void OnClick_LeaveRoom() {
            PhotonNetwork.LeaveRoom(true);
            panelsManager.CurrentRoom.Hide();
        }

    }
}
