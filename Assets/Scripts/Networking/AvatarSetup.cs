using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys
{
    public class AvatarSetup :MonoBehaviour
    {
        private PhotonView view;
        public GameObject character;
        public int characterID;

        private void Awake() {
            view = GetComponent<PhotonView>();
            if (view.IsMine) {
                view.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfos.instance.characterID);
            }
        }

        [PunRPC]
        void RPC_AddCharacter(int index) {
            characterID = index;
            character = Instantiate(PlayerInfos.instance.characters[index], transform.position, transform.rotation, transform);
        }
    }
}
