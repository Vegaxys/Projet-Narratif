using System.Collections;
using System.Collections.Generic;
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
        public string[] tagList;

        #endregion


        #region Private Serializable Fields

        [SerializeField] private GameObject loobyCamera;

        #endregion


        #region Private Fields

        private PhotonView view;

        #endregion


        #region MonoBehaviour Callbacks

        public void Awake() {
            instance = this;
            loobyCamera.SetActive(false);
            view = GetComponent<PhotonView>();
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
            if (Physics.Raycast(ray, out hit, 1000, 1 << 11)) {
                print(range);
                if (Vector3.Distance(hit.point, origin) < range) {
                    print(hit.transform.tag);
                    IEntity entity = null;
                    switch (hit.transform.tag) {
                        case "Player":
                            hit.transform.gameObject.AddComponent<OutlineRegister>();
                            entity = hit.transform.GetComponent<IEntity>();
                            break;
                        case "Ennemi":
                            hit.transform.gameObject.AddComponent<OutlineRegister>();
                            entity = hit.transform.GetComponent<IEntity>();
                            break;
                    }
                    return entity;
                }
            }
            return null;
        }

        public IEntity GetEntity(Vector3 pos) {
            Collider[] colliders = Physics.OverlapSphere(pos, .2f);
            foreach (var item in colliders) {
                return  item.GetComponent<IEntity>() != null ? item.GetComponent<IEntity>() : null;
            }
            return null;
        }

        public GameObject GetObjectByViewID(int id) {
            PhotonView obj = PhotonView.Find(id);
            return obj != null ? obj.gameObject : null;
        }

        public void DeselectTarget(Transform entity) {
            Destroy(entity.GetComponent<OutlineRegister>());
        }

        public string[] IncludeLayer(string[] layers) {
            List<string> _layers = new List<string>(tagList);
            foreach (var item in layers) {
                _layers.Remove(item);
            }
            foreach (var item in _layers) {
                print(item);
            }
            return _layers.ToArray();
        }

        public Quaternion GetRandomPrecision(Quaternion rot, float precision) {
            float newY = Random.Range(-precision / 2, precision / 2);
            return Quaternion.Euler(0, rot.eulerAngles.y + newY, 0);
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


