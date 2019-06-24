using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Vegaxys {
    public class RoomObjectUI :MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roomName;
        public RoomInfo RoomInfo { get; private set; }

        public void SetRoomInfo(RoomInfo info) {
            RoomInfo = info;
            roomName.text = info.PlayerCount + "/" + info.MaxPlayers + " -- " +  info.Name;
        }
        public void OnClick_EnterRoom() {
            PhotonNetwork.JoinRoom(RoomInfo.Name);
        }
    }
}
