using System.Collections;
using UnityEngine;
using Photon;
using Photon.Pun;
using TMPro;


public class GameManager_Dungeon : MonoBehaviourPun, IPunObservable{

    public TextMeshProUGUI countdownText;
    public GameObject lobbyCamera;

    private float spawnTimer = 3;

    private void Awake() {
        StartCoroutine(SpawnTime());
    }

    private IEnumerator SpawnTime() {
        lobbyCamera.SetActive(true);
        float t = spawnTimer;
        string time = "";
        while (t > 0) {
            t -= Time.deltaTime;
            time = string.Format("{0:0.00}", t);
            countdownText.text = "Spawning in : " + time + " sec";
            yield return null;
        }
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        lobbyCamera.SetActive(false);
        countdownText.gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {

        }else if (stream.IsWriting) {

        }
    }

}
