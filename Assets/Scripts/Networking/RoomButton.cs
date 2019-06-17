using System.Collections.Generic;
using System.Collections;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Vegaxys
{
    public class RoomButton :MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI sizeText;

        private string roomName;
        private int roomSize;
        private int playerCount;

        public void JoinRoomOnClick() {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void SetRoom(string nameInput, int sizeInput, int countInput) {
            roomName = nameInput;
            roomSize = sizeInput;
            playerCount = countInput;
            nameText.text = roomName;
            sizeText.text = countInput + " / " + sizeInput;
        }
    }

}
