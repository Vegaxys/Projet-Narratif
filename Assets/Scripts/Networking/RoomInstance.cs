using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Vegaxys
{
    public class RoomInstance :MonoBehaviour
    {
        public string roomNameString;
        public RoomInfo room;
        [SerializeField] private TextMeshProUGUI roomNameText;

        public void Setup(RoomInfo _room) {
            room = _room;
            roomNameText.text = room.Name;
            roomNameString = room.Name;
        }
        public void JoinRoom() {
            TypedLobby newLobby = new TypedLobby("VegaxysLobby", LobbyType.SqlLobby);
            PhotonNetwork.JoinLobby(newLobby);
            //PhotonNetwork.JoinRoom(roomNameString);
        }
    }
}
