using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon;
using Photon.Pun;
namespace Vegaxys {

    public class Ennemi :MonoBehaviourPunCallbacks, IPunObservable, IEntity
    {

        #region Public Fields

        public GameObject fireBullet;
        public GameObject lifeUI;
        public Mesh ring_AA;
        public Mesh ring_Aggro;
        public Transform canon;
        public Transform anchor;

        [Tooltip("General Values")]
        public string avatarName;
        public float maxSpeed;
        public float acceleration;
        [Range(0, 100)] public int aggro_Range;
        [Range(0, 100)] public int AA_Range;

        [Header("Auto Attack")]
        public float fireRate;
        public int damageFire;
        public int currentBulletInWeapon;
        public int maxBulletInWeapon;
        public int maxBulletInEntity;

        [Header("Health & Shield")]
        public int currentLife;
        public int maxLife;

        #endregion


        #region Private Fields

        private NavMeshAgent navigation;
        private PhotonView view;
        private float timmingFire;
        private float timmingCapa01;
        private float timmingCapa02;
        private bool isShooting, fireReady = true;

        #endregion


        #region Testing Methods

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawMesh(ring_AA, 0, transform.position, Quaternion.identity, Vector3.one * AA_Range);
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(ring_Aggro, 0, transform.position, Quaternion.identity, Vector3.one * aggro_Range);
        }

        #endregion


        #region MonoBehaviour CallBacks

        public virtual void Awake() {
            navigation = GetComponent<NavMeshAgent>();
            view = GetComponent<PhotonView>();

            GameObject _ui = Instantiate(lifeUI, GameObject.Find("HUD").transform);
            _ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        public virtual void Update() {

        }

        #endregion


        #region Virtuals Methods



        #endregion

        //*********************
        public virtual void AddLife(int amount) {
            /*  if (photonView.IsMine) {
                  photonView.RPC("GetHeal", RpcTarget.All, amount);
              }*/
        }
        [PunRPC]
        private void GetHeal(int amount) {
            currentLife += amount;
            if (currentLife > maxLife) {
                currentLife = maxLife;
            }
        }

        public int GetShield() {
            return 0;
        }

        public int GetLife() {
            return currentLife;
        }

        public int GetMaxLife() {
            return maxLife;
        }

        public int GetMaxShield() {
            return 0;
        }
        public Transform GetTransform() {
            return transform;
        }

        public Transform GetAnchor() {
            return anchor;
        }

        public string GetDisplayedName() {
            return avatarName;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            /*  if (stream.IsWriting) {
                  stream.SendNext(currentLife);
                  stream.SendNext(currentShield);
              } else {
                  currentLife = (int)stream.ReceiveNext();
                  currentShield = (int)stream.ReceiveNext();
              }*/
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "PlayerBullet") {
                other.gameObject.SetActive(false);
            }
        }
    }
}
