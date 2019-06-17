using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Vegaxys
{
    public class PhotonPlayer :MonoBehaviour
    {
        public GameObject playerAvatar;

        private void Awake() {
            ChooseSpawnPosition();
        }

        private void ChooseSpawnPosition() {
            int spawnIndex = Random.Range(0, GameManager.instance.spawnPoints.Length);
                string playerPrefabName = PlayerInfos.instance.characters[PlayerInfos.instance.characterID].name;
                playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", playerPrefabName), 
                    GameManager.instance.spawnPoints[spawnIndex].position, Quaternion.identity, 0);
        }
    }
}
