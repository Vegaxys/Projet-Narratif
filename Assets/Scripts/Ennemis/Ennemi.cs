using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

namespace Vegaxys {

    public class Ennemi :MonoBehaviourPunCallbacks, IPunObservable, IEntity
    {
        #region Public Fields

        public GameObject fireBullet;
        public GameObject lifeUI;
        public Mesh ring_Aggro;
        public Transform canon;         //Bullet spawn position
        public Transform model;         //Rotate towards player
        public Transform anchor;
        public List<BaseCharacter> characters = new List<BaseCharacter>();

        [Tooltip("General Values")]
        public string avatarName;
        public float maxSpeed;
        public float acceleration;
        [Range(0, 100)] public int aggro_Range;
        [Range(2, 15)] public float stoppingRange;

        [Header("Auto Attack")]
        public float fireRate;
        public float precision;
        public float reloadingSpeed;
        public int damageFire;
        public int currentBulletInWeapon;
        public int maxBulletInWeapon;
        public int maxBulletInEntity;

        [Header("Health & Shield")]
        public int currentLife;
        public int maxLife;

        [HideInInspector] public RoomManager room;
        [HideInInspector] public PhotonView view;

        #endregion


        #region Private Serializable Fields

        [SerializeField] private Transform target;
        [SerializeField] private SphereCollider aggroCollider;

        #endregion


        #region Private Fields

        private NavMeshAgent navigation;
        private EntityBar bar;
        private float timmingFire;

        #endregion


        #region Testing Methods

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(ring_Aggro, 0, transform.position, Quaternion.identity, Vector3.one * aggro_Range);
            Gizmos.color = Color.blue;
            Gizmos.DrawMesh(ring_Aggro, 0, transform.position, Quaternion.identity, Vector3.one * stoppingRange);
            aggroCollider.radius = aggro_Range;
        }

        #endregion


        #region MonoBehaviour CallBacks

        public virtual void Awake() {
            navigation = GetComponent<NavMeshAgent>();
            view = GetComponent<PhotonView>();
            aggroCollider = GetComponentInChildren<SphereCollider>();

            GameObject _ui = Instantiate(lifeUI, GameObject.Find("HUD").transform);
            bar = _ui.GetComponent<EntityBar>();
            _ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

            navigation.stoppingDistance = stoppingRange;

        }

        public virtual void Update() {
            if (!photonView.IsMine) {
                return;
            }
            if (target != null) {
                LookAtTarget();
                Fire();
            }
        }

        public void GetTriggered(Projectile projectile, string tag) {
            if (projectile.originalPlayer == transform) {
                return;
            }
            TakeDamage(projectile.damage);
            GameManager.instance.InstantiateDamageParticle("Damage", projectile.damage, transform.position);
            if (tag == "Projectile") {
                Destroy(projectile.gameObject);
            }
        }

        #endregion


        #region Virtuals Methods

        public virtual void Fire() {
            timmingFire -= Time.deltaTime;
            if (timmingFire <= 0 && currentBulletInWeapon > 0) {
                timmingFire = fireRate;
                Quaternion rot = GameManager.instance.GetRandomPrecision(canon.rotation, precision);
                int _damage = GameManager.instance.GetRandomDamage(damageFire);
                view.RPC("RPC_Ennemi_Shoot", RpcTarget.AllBuffered, rot, _damage);
                currentBulletInWeapon--;
            }
            if(currentBulletInWeapon == 0) {
                view.RPC("RPC_Ennemi_Reload", RpcTarget.AllBuffered);
            }
        }

        public virtual void TakeDamage(int amount) {
            if (amount < 0) amount *= -1;
            currentLife -= amount;
            print(avatarName + "'health is " + currentLife);
            if (currentLife <= 0) {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        public virtual IEnumerator ReturnToRoom(Vector3 pos) {
            navigation.SetDestination(pos);
            navigation.stoppingDistance = 1;
            yield return new WaitUntil(() => Vector3.Distance(transform.position, pos) < 1.5f);
            navigation.stoppingDistance = stoppingRange;
        }

        #endregion


        #region Publics Methods

        public void AddCharacter(BaseCharacter character) {
            foreach(var item in characters) {
                if(character == item) {
                    return;
                }
            }
            characters.Add(character);
            RefreshAggro();
            print(character.GetDisplayedName() + " Added to the list ");
        }
        public void RemoveCharacter(BaseCharacter character) {
            characters.Remove(character);
            RefreshAggro();
            print("Removed " + character.GetDisplayedName() + " from list ");
        }

        #endregion


        #region Private Methods

        private void RefreshAggro() {
            BaseCharacter _character = GetHighestAggroValue();
            target = _character != null ? _character.transform : null;
            if (target != null) {
                view.RPC("RPC_GoToTarget", RpcTarget.AllBuffered, target.position);
            }

            Invoke("RefreshAggro", 0.5f);
        }

        private BaseCharacter GetHighestAggroValue() {
            BaseCharacter _character = new BaseCharacter();
            for (int i = 0; i < characters.Count; i++) {
                if(characters[i].aggroValue >= _character.aggroValue) {
                    _character = characters[i];
                }
            }
            return _character;
        }

        private void LookAtTarget() {
            model.LookAt(target.position);
            Quaternion rotation = model.rotation;
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            model.rotation = Quaternion.Lerp(model.rotation, rotation, Time.deltaTime * 5);
        }

        private IEnumerator Reload() {
            yield return new WaitForSeconds(reloadingSpeed);
            currentBulletInWeapon = maxBulletInWeapon;
            maxBulletInEntity -= maxBulletInWeapon;
        }

        #endregion


        #region RPCs Methods

        [PunRPC]
        public virtual void RPC_Ennemi_Shoot(Quaternion rot, int _damage) {
            GameObject bullet = Instantiate(fireBullet, canon.position, rot);
            bullet.GetComponent<Projectile>().Setup(transform, _damage);
        }

        [PunRPC]
        public virtual void RPC_Ennemi_Reload() {
            StartCoroutine(bar.Reloading(reloadingSpeed));
            StartCoroutine(Reload());
        }

        [PunRPC]
        public void RPC_GetNewPos(Vector3 newPos) {
            CancelInvoke();
            StartCoroutine(ReturnToRoom(newPos));
        }

        [PunRPC]
        public virtual void RPC_GoToTarget(Vector3 pos) {
            navigation.SetDestination(pos);
        }

        #endregion


        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
           /* if (stream.IsWriting) {
                stream.SendNext(currentLife);
            } else {
                currentLife = (int)stream.ReceiveNext();
            }*/
        }

        #endregion


        #region IEntity implementation

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

        #endregion

    }
}
