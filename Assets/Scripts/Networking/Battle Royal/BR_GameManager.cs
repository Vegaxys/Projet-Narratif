using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;
using Photon.Pun;

public class BR_GameManager : MonoBehaviourPun, IPunObservable{

    public GameObject lobbyCamera;
    public TextMeshProUGUI cooldownTimer;

    public float spawnTime;
    private bool hasPlayersSpawned = false;

    private void Start() {
        lobbyCamera.SetActive(true);

    }

    private void Update() {
        spawnTime -= Time.deltaTime;
        cooldownTimer.text = "Spawning in : " + Mathf.Round(spawnTime) + " sec";
        if (spawnTime < 0) {
            if (!hasPlayersSpawned) {
                lobbyCamera.SetActive(false);
                cooldownTimer.gameObject.SetActive(false);
                PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
                hasPlayersSpawned = true;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {

        }else if (stream.IsReading) {

        }
    }
}
