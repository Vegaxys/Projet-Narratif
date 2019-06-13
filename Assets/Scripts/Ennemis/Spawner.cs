using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Vegaxys {
    public class Spawner :MonoBehaviour
    {
        public GameObject prefab;
        private PhotonView view;

        private void Awake() {
            view = GetComponent<PhotonView>();
            if (!view.IsMine) {
                return;
            }
            view.RPC("ChooseSpawnPosition", RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void ChooseSpawnPosition() {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
