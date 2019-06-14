using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Knife.PostProcessing;

namespace Vegaxys
{
    [System.Serializable]
    public class OutlineSettings
    {
        public string type;
        public Color color;
    }

    public class GameManager :MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public static GameManager instance;
        public Transform[] spawnPoints;
        public OutlineSettings[] settings;

        #endregion


        #region Private Serializable Fields

        [SerializeField] private GameObject loobyCamera;

        #endregion


        #region MonoBehaviour Callbacks

        public void Awake() {
            instance = this;
            loobyCamera.SetActive(false);
        }

        #endregion


        #region Photon Callbacks

        public override void OnLeftRoom() {
            SceneManager.LoadScene(0);
        }

        #endregion


        #region Public Methods

        public void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }

        public IEntity GetEntity(float range, Vector3 origin) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (!hit.transform.CompareTag("Untagged") && hit.transform.GetComponent<OutlineRegister>() == null) {
                    Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
                    if (Vector3.Distance(hit.point, origin) < range) {
                        IEntity entity = hit.transform.GetComponent<IEntity>();
                        hit.transform.gameObject.AddComponent<OutlineRegister>();
                        return entity;
                    }
                }
            }
            return null;
        }
        public void DeselectTarget(Transform entity) {
            Destroy(entity.GetComponent<OutlineRegister>());
        }

        #endregion


        #region Private Methods

        public OutlineSettings GetSettings(string type) {
            for (int i = 0; i < settings.Length; i++) {
                if (settings[i].type == type) {
                    return settings[i];
                }
            }
            return null;
        }

        #endregion

    }
}
















    /*public TextMeshProUGUI countdownText;
    public GameObject lobbyCamera;

    private float spawnTimer = 3;

    public static GameManager gm;
    

    private void Awake() {
        gm = this;
        //* Create Bullet pool
     /*   BulletPool = new Dictionary<string, Queue<GameObject>>();
        foreach (Bullet item in bullets) {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < item.size; i++) {
                GameObject bullet = PhotonNetwork.Instantiate(item.prefab.name, Vector3.zero, Quaternion.identity, 0);
                bullet.transform.parent = bulletContainer;
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
}*/


