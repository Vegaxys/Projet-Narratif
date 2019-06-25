using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System.Linq;
using System.Collections.Generic;
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
        public GameObject []playerPrefab;
        public List<PlayerProperties> players;
        public GameObject localPlayerInstance;
        public GameObject damageParticle;
        public GameObject grenadePrefab;
        public GameObject gizAOE;
        public Transform particleUIContainer;
        public int healValue;
        public int shieldValue;
        public int ammoValue;
        public int granadeDamage;
        public int score;

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
            if (PhotonNetwork.IsConnected) {
                loobyCamera.SetActive(false);
                view = GetComponent<PhotonView>();
                PhotonNetwork.Instantiate(playerPrefab[PlayerInfos.instance.player.avatarID].name, spawnPoints[PlayerInfos.instance.player.playerID].position, Quaternion.identity, 0);
                Invoke("CallObjectifManager", 1);
            }
        }


        #endregion


        #region Photon Callbacks

        public override void OnLeftRoom() {
            SceneManager.LoadScene(0);
        }

        #endregion


        #region Public Methods

        public void LeaveRoom() {
            print(PhotonNetwork.IsConnected);
            PhotonNetwork.LeaveRoom();
        }

        public IEntity GetEntity(float range, Vector3 origin) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = LayerMask.GetMask("Player", "Enemy");
            if (Physics.Raycast(ray, out hit, 1000, mask)) {
                if (Vector3.Distance(hit.point, origin) < range) {
                    IEntity entity = null;
                    switch (hit.transform.tag) {
                        case "Player":
                            OutlineRegister register1 = hit.transform.GetChild(0).gameObject.AddComponent<OutlineRegister>();
                            register1.OutlineTint = GetSettings("Player").color;
                            register1.setupPropertyBlock();
                            entity = hit.transform.GetComponent<IEntity>();
                            break;
                        case "Ennemi":
                            OutlineRegister register2 = hit.transform.parent.GetChild(0).gameObject.AddComponent<OutlineRegister>();
                            register2.OutlineTint = GetSettings("Ennemi").color;
                            register2.setupPropertyBlock();
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
                    break;
                case "Shield":
                    text.color = Color.blue;
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

        public Vector3 MousePositionWithoutWall(Transform originTransform, float radius, Vector3 origin) {
            Vector3 newPos = MousePosition();
            RaycastHit hit;
            if (Physics.Raycast(originTransform.position, originTransform.forward, out hit, radius, 1 << 12)) {
                Debug.DrawLine(originTransform.position, hit.point, Color.yellow);
                return hit.point;
            } else {
                Debug.DrawLine(originTransform.position, newPos, Color.blue);
                return newPos;
            }
        }

        public Vector3 GetRandomPositionInTorus(Vector3 origin, float diskRadius, float originRadius, bool collision) {
            Vector3 result = origin;
            Vector2 circle = Random.insideUnitCircle;
            result += (new Vector3(circle.x, 0, circle.y) * diskRadius);
            if (Vector3.Distance(result, transform.position) < originRadius) {
                return GetRandomPositionInTorus(origin, diskRadius, originRadius, collision);
            } else {
                if (collision) {
                    Collider[] colliders = Physics.OverlapSphere(result + Vector3.up, .9f);
                    if(colliders.Length ==0) return result;

                    foreach (var item in colliders) {
                        if(item.tag == "Untagged" || item.tag == "Ennemi" || item.tag == "Player") {
                            return GetRandomPositionInTorus(origin, diskRadius, originRadius, collision);
                        }
                    }
                }
                return result;
            }
        }

        public void AddScore(int amount) {
            score += amount;
            HUD_Manager.manager.Update_Score(score);
        }

        #endregion


        #region Private Methods

        private OutlineSettings GetSettings(string type) {
            for (int i = 0; i < settings.Length; i++) {
                if (settings[i].type == type) {
                    return settings[i];
                }
            }
            return null;
        }

        private void CallObjectifManager() {
            print(ObjectifManager.instance== null);
            if (ObjectifManager.instance != null) {
                ObjectifManager.instance.CreateRandomObjectifs();
            }
        }

        #endregion
    }
}