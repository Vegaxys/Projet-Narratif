using Photon.Pun;
using System.IO;
using UnityEngine;


namespace Vegaxys {
    public class Spawner :MonoBehaviour, IPunObservable
    {
        public GameObject prefab;
        public RoomManager roomManager;
        private PhotonView view;
        private bool hasSpawn;

        private void Awake() {
            view = GetComponent<PhotonView>();
         /*   if (!PhotonNetwork.IsMasterClient) {
                return;
            }*/
        }
        private void OnDrawGizmos() {
            switch (prefab.name) {
                case "Ennemi_Alpha":
                    Gizmos.color = Color.cyan;
                    break;
                case "Ennemi_Beta":
                    Gizmos.color = Color.green;
                    break;
                case "Ennemi_Lambda":
                    Gizmos.color = Color.grey;

                    break;
                default:
                    break;
            }
            Gizmos.DrawCube(transform.position, new Vector3(2, 25, 2));
        }

        public void Spawn() {
            if (!hasSpawn) {
                view.RPC("RPC_SpawnEnemi", RpcTarget.All);
                hasSpawn = true;
            }
        }

        [PunRPC]
        private void RPC_SpawnEnemi() {
            string prefabName = prefab.name;
            GameObject npc = PhotonNetwork.Instantiate(Path.Combine("EnnemiPhotonPrefab", prefabName),
                transform.position, Quaternion.identity, 0);
            npc.GetComponentInChildren<Ennemi>().room = roomManager;
        }

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting && view.IsMine) {
            } else
            if (stream.IsReading) {
            }
        }

        #endregion
    }
}
