using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System.Linq;

namespace Vegaxys {

    public class Ennemi :MonoBehaviourPunCallbacks, IEntity
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
        public int score;
        [Range(0, 100)] public int aggro_Range;
        [Range(2, 15)] public float stoppingRange;


        [Header("Auto Attack")]
        public AutoAttack currentAttack;

        [Header("Health & Shield")]
        public int currentLife;
        public int maxLife;

        [HideInInspector] public RoomManager room;
        [HideInInspector] public PhotonView view;

        [HideInInspector] public int currentBulletInWeapon;
        [HideInInspector] public int maxBulletInWeapon;
        [HideInInspector] public int maxBulletInEntity;

        #endregion


        #region Private Serializable Fields

        [SerializeField] private Transform target;
        [SerializeField] private SphereCollider aggroCollider;

        #endregion


        #region Private Fields

        private NavMeshAgent navigation;
        private EntityBar bar;
        private Coroutine takeTicCoroutine;

        private float timmingFire;
        private bool takingTic;

        [Header("Auto Attack")]
        private float fireRate;
        private int damageFire;
        private float precision;
        private float reloadingSpeed;

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

            InitialyzeAutoAttack();
            UpdateAutoAttack();

            StartCoroutine(RefreshAggro());
        }

        public virtual void Update() {
            if (target != null) {
                LookAtTarget();
                Fire();
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
            if (currentLife <= 0) {
                Companion_Drone drone = GetComponentInChildren<Companion_Drone>();
                if (drone != null) {
                    drone.transform.parent = GameObject.Find("Companions").transform;
                    drone.LaunchDrone(Companion_Drone.CompanionState.IDLE, null);
                }
                GameManager.instance.AddScore(score);
                PhotonNetwork.Destroy(gameObject);
            }
            GameManager.instance.InstantiateDamageParticle("Damage", amount, transform.position);
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
          /*  foreach(var item in characters) {
                if(character == item) {
                    return;
                }
            }*/
            //characters.Add(character);
            print(character.GetDisplayedName() + " Added to the list ");
        }

        public void RemoveCharacter(BaseCharacter character) {
           // characters.Remove(character);
            print("Removed " + character.GetDisplayedName() + " from list ");
        }

        public void GetTriggered(Projectile projectile, string tag) {
            if (projectile.originalPlayer == transform) {
                return;
            }
            if(takeTicCoroutine != null) {
                StopCoroutine(takeTicCoroutine);
            }
            takeTicCoroutine = StartCoroutine(TakeTic(projectile.dot_Row, projectile.dot_Damage));
            TakeDamage(projectile.damage);
            if (tag == "Projectile") {
                Destroy(projectile.gameObject);
            }
        }

        public void InitialyzeAutoAttack() {
            currentAttack.currentBulletInWeapon = currentAttack.save_currentBulletInWeapon;
            currentAttack.maxBulletInWeapon = currentAttack.save_maxBulletInWeapon;
            currentAttack.maxBulletInPlayer = currentAttack.save_maxBulletInPlayer;
        }

        public void UpdateAutoAttack() {
            fireRate = currentAttack.fireRate;
            damageFire = currentAttack.damageFire;
            precision = currentAttack.precision;
            reloadingSpeed = currentAttack.reloadingSpeed;
            currentBulletInWeapon = currentAttack.currentBulletInWeapon;
            maxBulletInWeapon = currentAttack.maxBulletInWeapon;
            maxBulletInEntity = currentAttack.maxBulletInPlayer;
        }

        #endregion


        #region Private Methods

        private IEnumerator RefreshAggro() {
            BaseCharacter _character = GetHighestAggroValue();
            target = _character != null ? _character.transform : null;
            if (target != null) {
                view.RPC("RPC_GoToTarget", RpcTarget.AllBuffered, target.position);
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(RefreshAggro());
        }

        private BaseCharacter GetHighestAggroValue() {
            characters.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, aggro_Range);
            foreach (var item in colliders) {
                if(item.GetComponent<BaseCharacter>() != null) {
                    if (!item.GetComponent<BaseCharacter>().furtiv)
                        characters.Add(item.GetComponent<BaseCharacter>());
                }
            }
            BaseCharacter _character = null;
            for (int i = 0; i < characters.Count; i++) {
                if(_character == null || characters[i].aggroValue >= _character.aggroValue) {
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

        public virtual IEnumerator TakeTic(int row, int damage) {
            for (int i = 0; i < row; i++) {
                TakeDamage(damage);
                yield return new WaitForSeconds(1);
            }
        }

        #endregion


        #region RPCs Methods

        [PunRPC]
        public virtual void RPC_Ennemi_Shoot(Quaternion rot, int _damage) {
            GameObject bullet = Instantiate(fireBullet, canon.position, rot);
            bullet.GetComponent<Projectile>().Setup(transform, _damage, 0, 0);
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
