using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using TMPro;

[System.Serializable]
public class Bullet
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class GameManager_Dungeon : MonoBehaviourPun, IPunObservable{

    public TextMeshProUGUI countdownText;
    public GameObject lobbyCamera;

    private float spawnTimer = 3;

    public static GameManager_Dungeon dungeon;

    [Header("Object Pool")]

    public List<Bullet> bullets;
    public Transform bulletContainer;
    public Dictionary<string, Queue<GameObject>> BulletPool;


    private void Awake() {
        dungeon = this;
        //* Create Bullet pool
        BulletPool = new Dictionary<string, Queue<GameObject>>();
        foreach (Bullet item in bullets) {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < item.size; i++) {
                GameObject bullet = Instantiate(item.prefab, bulletContainer);
                bullet.SetActive(false);
                queue.Enqueue(bullet);
            }
            BulletPool.Add(item.tag, queue);
        }
        StartCoroutine(SpawnTime());
    }

    public GameObject GetBullet(string tag, Vector3 position, Quaternion rotation) {
        if (!BulletPool.ContainsKey(tag)) {
            Debug.LogWarning("The bullet named " + tag + " doesn't exist");
            return null;
        }
        GameObject bullet = BulletPool[tag].Dequeue();
        bullet.SetActive(true);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        BulletPool[tag].Enqueue(bullet);
        return bullet;
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
    public void MousePosition(out Vector3 result) {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        if (plane.Raycast(ray, out point)) {
            result = ray.GetPoint(point);
        } else {
            result = Vector3.zero;
        }
    }
    public void MousePosition(out Transform result) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            result = hit.transform;
        } else {
            result = null;
        }

    }
}

