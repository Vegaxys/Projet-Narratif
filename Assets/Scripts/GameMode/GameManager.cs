using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
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
        public GameObject damageParticle;
        public GameObject grenadePrefab;
        public GameObject gizGrenade;
        public Transform particleUIContainer;
        public int healValue;
        public int shieldValue;
        public int ammoValue;
        public int granadeDamage;

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
                if (Vector3.Distance(hit.point, origin) < range) {
                    IEntity entity = null;
                    switch (hit.transform.tag) {
                        case "Player":
                            hit.transform.gameObject.AddComponent<OutlineRegister>();
                            entity = hit.transform.GetComponent<IEntity>();
                            break;
                        case "Ennemi":
                            OutlineRegister register = hit.transform.parent.GetChild(0).gameObject.AddComponent<OutlineRegister>();
                            register.OutlineTint = GetSettings("Ennemi").color;
                            register.setupPropertyBlock();
                            entity = hit.transform.parent.GetComponent<IEntity>();
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
            Destroy(entity.GetComponentInChildren<OutlineRegister>());
        }

        public Quaternion GetRandomPrecision(Quaternion rot, float precision) {
            float newY = Random.Range(-precision / 2, precision / 2);
            return Quaternion.Euler(0, rot.eulerAngles.y + newY, 0);
        }

        public int GetRandomDamage(int damage) {
            return Random.Range(damage - (damage / 10), damage + (damage / 10));
        }

        public void InstantiateDamageParticle(string tag, int amount, Vector3 pos) {
            GameObject _particle = Instantiate(damageParticle, particleUIContainer);
            _particle.transform.position = Camera.main.WorldToScreenPoint(pos);
            TextMeshProUGUI text = _particle.GetComponentInChildren<TextMeshProUGUI>();
            switch (tag) {
                case"Damage":
                    text.color = new Color(.7f, .1f, .1f, 1);
                    break;
                case "Health":
                    text.color = Color.green;
                    //text.color = new Color(.1f, .5f, .13f, 1);
                    break;
                case "Shield":
                    text.color = Color.blue;
                    //text.color = new Color(.1f, .5f, .1f, 1);
                    break;
            }
            text.text = amount.ToString();
            Destroy(_particle, 1);
        }

        public Vector3 MousePosition() {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float point = 0f;

            if (plane.Raycast(ray, out point)) {
                return ray.GetPoint(point);
            } else {
                return Vector3.zero;
            }
        }

        public Vector3 MousePosition(float radius, Vector3 origin) {
            Vector3 pos = MousePosition();
            if (Vector3.Distance(origin, pos) < radius) {
                return pos;
            } else {
                float distance = Vector3.Distance(origin, pos);
                Vector3 result = Vector3.Lerp(origin, pos, (radius / distance));     //le multiplier par le radius
                result.y = pos.y;
                return result;
            }
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