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
            if (!PhotonNetwork.IsMasterClient) {
                return;
            }
            print("I am the masterClient");
            view.RPC("ChooseSpawnPosition", RpcTarget.All);
        }

        [PunRPC]
        private void ChooseSpawnPosition() {
            string prefabName = prefab.name;
            PhotonNetwork.Instantiate(Path.Combine("EnnemiPhotonPrefab", prefabName),
                transform.position, Quaternion.identity, 0);
        }
    }
}
