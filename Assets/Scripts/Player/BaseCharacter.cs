using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

namespace Vegaxys
{
    [System.Serializable]
    public class PlayerProperties
    {
        public string playerName;
        public int playerID;
        public string avatarName;
        public string className;
        public string playerXP;
        public int avatarID;
    }

    public class BaseCharacter :MonoBehaviourPunCallbacks, IEntity , IPunObservable
    {
        #region Public Fields

        public GameObject fireBullet;
        public GameObject playerUI;
        public Transform canon;
        public Transform anchor;
        [HideInInspector] public Camera _cam;
        [HideInInspector] public PhotonView view;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public bool furtiv = false;
        [Tooltip("General Values")]
        public float camSpeed = 5;
        public PlayerProperties player;
        public AutoAttack currentAttack;
        [Range(0, 100)] public int aggroValue;
        public Capa capa01;
        public Capa capa02;

        [Header("Grenade")]
        public int grenade_Range;
        public GameObject gizmos_Grenade;

        [Header("Health & Shield")]
        public int currentLife;
        public int currentShield;
        public int maxLife;
        public int maxShield;

        [HideInInspector] public int currentBulletInWeapon;
        [HideInInspector] public int maxBulletInWeapon;
        [HideInInspector] public int maxBulletInPlayer;

        #endregion


        #region Private Fields

        private EntityBar bar;
        private Transform model;
        private Coroutine takeTicCoroutine;
        private float timmingFire;
        private int shieldCount;
        private int healthCount;
        private int grenadeCount;
        private int maxConso = 3;
        private bool takingTic;

        [Header("Auto Attack")]
        private float fireRate;
        private int damageFire;
        private float precision;
        private float reloadingSpeed;

        #endregion


        #region MonoBehaviour CallBacks

        public virtual void Start() {
            model = transform.GetChild(0);
            agent = GetComponent<NavMeshAgent>();
            view = GetComponent<PhotonView>();
            _cam = transform.parent.GetComponentInChildren<Camera>();
            HUD_Manager.manager.character = this;
            GameManager.instance.localPlayerInstance = gameObject;
            player = PlayerInfos.instance.player;

            transform.parent.GetComponentInChildren<Camera>().gameObject.SetActive(photonView.IsMine);

            GameObject _ui = Instantiate(playerUI, GameObject.Find("HUD").transform);
            bar = _ui.GetComponent<EntityBar>();
            _ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

            InitialyzeAutoAttack();
            UpdateAutoAttack();

            HUD_Manager.manager.Update_Capacites();
            HUD_Manager.manager.Update_Chargeur(currentBulletInWeapon, maxBulletInWeapon, maxBulletInPlayer);
            HUD_Manager.manager.Update_Consos(shieldCount, healthCount, grenadeCount);
        }

        public virtual void Update() {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
                return;
            }
            _cam.transform.parent.position = Vector3.Lerp(_cam.transform.parent.position, transform.position, camSpeed * Time.deltaTime);
            if (agent.enabled) {
                Virtual_Movements();
                Virtual_PlayerRotation();
            }
            #region Fire
            if (Input.GetButton("Fire")) {
                Virtual_Fire();
            }
            if (Input.GetButtonUp("Reload")) {
                if (maxBulletInPlayer >= maxBulletInWeapon) {
                    StartCoroutine(RefreshHUD());
                    view.RPC("RPC_Reload", RpcTarget.AllBuffered);
                }
            }
            #endregion
            #region Consommables 
            if (Input.GetButtonDown("Conso_Shield")) {
                if (currentShield < maxShield && shieldCount > 0) {
                    view.RPC("RPC_GetShield", RpcTarget.AllBuffered);
                    shieldCount--;
                    HUD_Manager.manager.Update_Consos(shieldCount, healthCount, grenadeCount);
                }
            }
            if (Input.GetButtonDown("Conso_Health")) {
                if (currentLife < maxLife && healthCount > 0) {
                    view.RPC("RPC_GetHeal", RpcTarget.AllBuffered);
                    healthCount--;
                    HUD_Manager.manager.Update_Consos(shieldCount, healthCount, grenadeCount);
                }
            }
            #endregion
            #region Grenade
            if (Input.GetButtonDown("Conso_Grenade") && grenadeCount > 0) {
                gizmos_Grenade.SetActive(true);
                GameManager.instance.gizAOE.transform.localScale = Vector3.one * 2.4f;
                GameManager.instance.gizAOE.SetActive(true);
            }
            if (Input.GetButton("Conso_Grenade") && grenadeCount > 0) {
                GameManager.instance.gizAOE.transform.position = GameManager.instance.MousePosition(grenade_Range, transform.position);
            }
            if (Input.GetButtonUp("Conso_Grenade") && grenadeCount > 0) {
                gizmos_Grenade.SetActive(false);
                GameManager.instance.gizAOE.SetActive(false);
                int _damage = GameManager.instance.GetRandomDamage(GameManager.instance.granadeDamage);
                Vector3 destination = GameManager.instance.MousePosition(grenade_Range, transform.position);
                view.RPC("RPC_LaunchGrenade", RpcTarget.AllBuffered, _damage, destination);
                grenadeCount--;
                HUD_Manager.manager.Update_Consos(shieldCount, healthCount, grenadeCount);
            }
            #endregion
        }

        void OnTriggerEnter(Collider other) {
            /*if (furtiv) {
                return;
            }*/
            if (other.CompareTag("Projectile")) {
                Projectile projectile = other.GetComponent<Projectile>();
                //entities = projectile.originalPlayer.GetComponent<IEntity>();
                if (projectile.originalPlayer == transform) {
                    return;
                }
                Virtual_TakeDamage(projectile.damage);
                GameManager.instance.InstantiateDamageParticle("Damage", projectile.damage, transform.position);
                Destroy(other.gameObject);
            }
            if (other.CompareTag("Grenade")) {
                Projectile projectile = other.GetComponent<Projectile>();
                Virtual_TakeDamage(projectile.damage);
                GameManager.instance.InstantiateDamageParticle("Damage", projectile.damage, transform.position);
            }
            if (other.CompareTag("PowerUp")) {
                switch (other.GetComponent<PowerUp>().type) {
                    case PowerUpType.SHIELD:
                        if (shieldCount < maxConso) {
                            shieldCount++;
                            EventManager.instance.SetEvent(WinConditionEnum.RAMASSER_X_SHIELD);
                            HUD_Manager.manager.Update_Consos(shieldCount, healthCount, grenadeCount);
                            Destroy(other.gameObject);
                        }
                        break;
                    case PowerUpType.HEALTH:
                        if (healthCount < maxConso) {
                            healthCount++;
                            EventManager.instance.SetEvent(WinConditionEnum.RAMASSER_X_HEAL);
                            HUD_Manager.manager.Update_Consos(shieldCount, healthCount, grenadeCount);
                            Destroy(other.gameObject);
                        }
                        break;
                    case PowerUpType.GRENADE:
                        if (grenadeCount < maxConso) {
                            grenadeCount++;
                            EventManager.instance.SetEvent(WinConditionEnum.RAMASSER_X_GRENADE);
                            HUD_Manager.manager.Update_Consos(shieldCount, healthCount, grenadeCount);
                            Destroy(other.gameObject);
                        }
                        break;
                    case PowerUpType.MUNITION:
                        maxBulletInPlayer += maxBulletInWeapon * 2;
                        EventManager.instance.SetEvent(WinConditionEnum.RAMASSER_X_MUNITION);
                        if (!currentAttack.needReload) {
                            currentBulletInWeapon = maxBulletInPlayer;
                        }
                        HUD_Manager.manager.Update_Chargeur(currentBulletInWeapon, maxBulletInWeapon, maxBulletInPlayer);
                        Destroy(other.gameObject);
                        break;
                }
            }
            if (other.CompareTag("SpawnerPowerUp")) {
                bool isOk = false;
                switch (other.GetComponent<PowerUpHandler>().type) {
                    case PowerUpType.SHIELD:
                        if (shieldCount < maxConso) isOk = true;
                        break;
                    case PowerUpType.HEALTH:
                        if (healthCount < maxConso) isOk = true;
                        break;
                    case PowerUpType.GRENADE:
                        if (grenadeCount < maxConso) isOk = true;
                        break;
                    case PowerUpType.MUNITION:
                        isOk = true;
                        break;
                }
                if(isOk) StartCoroutine(other.GetComponent<PowerUpHandler>().PickPowerUp(transform));
            }
        }

        #endregion


        #region Virtuals Methods

        public virtual void Virtual_Movements() {
            Vector3 direction = Vector3.zero;
            direction += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            agent.SetDestination(transform.position + direction);
        }

        public virtual void Virtual_PlayerRotation() {
            model.LookAt(GameManager.instance.MousePosition());
            Quaternion rotation = model.rotation;
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            model.rotation = rotation;
        }

        public virtual void Virtual_Fire() {
            timmingFire -= Time.deltaTime;
            if(timmingFire <= 0 && currentBulletInWeapon > 0) {
                timmingFire = fireRate;
                Quaternion rot = GameManager.instance.GetRandomPrecision(canon.rotation, precision);
                int _damage = GameManager.instance.GetRandomDamage(damageFire);
                view.RPC("RPC_Character_Shoot", RpcTarget.All, rot, _damage);
                if(maxBulletInPlayer != 1)
                    currentBulletInWeapon--;
                HUD_Manager.manager.Update_Chargeur(currentBulletInWeapon, maxBulletInWeapon, maxBulletInPlayer);
            }
        }

        public virtual void Virtual_TakeDamage(int amount) {
            if (amount < 0 && currentLife < maxLife) {
                AddHealth(amount *= -1);
                GameManager.instance
                    .InstantiateDamageParticle("Health", amount *= -1, transform.position);
                return;
            } else {
                currentShield -= amount;
                if (currentShield < 0) {
                    currentLife += currentShield;
                    currentShield = 0;
                }
                if (currentLife <= 0) {
                    Virtual_Death();
                }
            }
        }

        public virtual void Virtual_Death() {
            if (player.className == "Tank") {
                EventManager.instance.SetEvent(WinConditionEnum.TUER_TANK);
            }
            if (player.className == "Soutient") {
                EventManager.instance.SetEvent(WinConditionEnum.TUER_SOUTIENT);
            }
            if (player.className == "Tireur") {
                EventManager.instance.SetEvent(WinConditionEnum.TUER_TIREUR);
            }
            if (player.className == "Assassin") {
                EventManager.instance.SetEvent(WinConditionEnum.TUER_ASSASSIN);
            }
            //GameManager.instance.LeaveRoom();
        }

        public virtual IEnumerator Virtual_Reload(float sec) {
            yield return new WaitForSeconds(sec);
            currentBulletInWeapon = maxBulletInWeapon;
            maxBulletInPlayer -= maxBulletInWeapon;
        }

        #endregion


        #region Private Methods

        private IEnumerator RefreshHUD() {
            yield return new WaitForSeconds(reloadingSpeed + .1f);
            HUD_Manager.manager.Update_Chargeur(currentBulletInWeapon, maxBulletInWeapon, maxBulletInPlayer);
        }

        private void AddHealth() {
            int oldCurrentLife = currentLife;
            currentLife += GameManager.instance.healValue;
            if (currentLife > maxLife) currentLife = maxLife;

            GameManager.instance.InstantiateDamageParticle("Health", currentLife - oldCurrentLife, transform.position);
        }

        private void AddShield() {
            int oldCurrentShield = currentShield;
            currentShield += GameManager.instance.shieldValue;
            if (currentShield > maxShield) currentShield = maxShield;
            GameManager.instance.InstantiateDamageParticle("Shield", currentShield - oldCurrentShield, transform.position);
        }

        private void AddShield(int amount) {
            currentShield += amount;
            if (currentShield > maxShield) currentShield = maxShield;
        }

        private void InitialyzeAutoAttack() {
            currentAttack.currentBulletInWeapon = currentAttack.save_currentBulletInWeapon;
            currentAttack.maxBulletInWeapon = currentAttack.save_maxBulletInWeapon;
            currentAttack.maxBulletInPlayer = currentAttack.save_maxBulletInPlayer;
        }

        #endregion


        #region Public Methods

        public void AddHealth(int amount) {
            currentLife += amount;
            if (currentLife > maxLife) currentLife = maxLife;
            GameManager.instance.InstantiateDamageParticle("Health", GameManager.instance.healValue, transform.position);
        }

        public void UpdateAutoAttack() {
            fireRate = currentAttack.fireRate;
            damageFire = currentAttack.damageFire;
            precision = currentAttack.precision;
            reloadingSpeed = currentAttack.reloadingSpeed;
            currentBulletInWeapon = currentAttack.currentBulletInWeapon;
            maxBulletInWeapon = currentAttack.maxBulletInWeapon;
            maxBulletInPlayer = currentAttack.maxBulletInPlayer;
        }

        public void UploadAutoAttack() {
            currentAttack.fireRate = fireRate;
            currentAttack.damageFire = damageFire;
            currentAttack.precision = precision;
            currentAttack.reloadingSpeed = reloadingSpeed;
            currentAttack.currentBulletInWeapon = currentBulletInWeapon;
            currentAttack.maxBulletInWeapon = maxBulletInWeapon;
            currentAttack.maxBulletInPlayer = maxBulletInPlayer;
        }

        #endregion


        #region RPC Methods

        [PunRPC]
        public virtual void RPC_Character_Shoot(Quaternion rot, int _damage) {
            GameObject bullet = Instantiate(fireBullet, canon.position, rot);
            bullet.GetComponent<Projectile>().isHealing = currentAttack.healingBullet;
            bullet.GetComponent<Projectile>().Setup(transform, _damage, currentAttack.damageDOT, currentAttack.row);
        }

        [PunRPC]
        public virtual void RPC_Reload() {
            StartCoroutine(bar.Reloading(reloadingSpeed));
            StartCoroutine(Virtual_Reload(reloadingSpeed));
        }

        [PunRPC]
        public void RPC_GetShield() {
            AddShield();
        }

        [PunRPC]
        public void RPC_GetHeal() {
            AddHealth();
        }

        [PunRPC]
        public void RPC_LaunchGrenade(int _damage, Vector3 pos) {
            GameObject grenade = Instantiate(GameManager.instance.grenadePrefab, transform.position, Quaternion.identity);
            Grenade _grenade = grenade.GetComponentInChildren<Grenade>();
            _grenade.damage = _damage;
            _grenade.destination = pos;
            _grenade.originalPlayer = GameManager.instance.transform;
        }

        #endregion


        #region IEntity implementation

        public int GetShield() {
            return currentShield;
        }

        public int GetLife() {
            return currentLife;
        }

        public int GetMaxLife() {
            return maxLife;
        }

        public int GetMaxShield() {
            return maxShield;
        }

        public Transform GetTransform() {
            return transform;
        }

        public Transform GetAnchor() {
            return anchor;
        }

        public string GetDisplayedName() {
            return player.playerID + "|" + player.avatarName + "|" + player.playerName;
        }

        #endregion


        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {

            } else 
            if (stream.IsReading) {

            }
        }

        #endregion
    }
}
