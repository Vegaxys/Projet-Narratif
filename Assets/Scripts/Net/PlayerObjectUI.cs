using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Vegaxys {
    public class PlayerObjectUI :MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roomName;
        public Player player;

        public void SetPlayerInfo(Player info) {
            player = info;
            roomName.text = info.NickName;
        }
    }
}
