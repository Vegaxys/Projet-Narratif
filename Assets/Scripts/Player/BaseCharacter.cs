using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using Knife.PostProcessing;

namespace Vegaxys
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseCharacter :MonoBehaviourPunCallbacks, IEntity , IPunObservable
    {
        #region Public Fields

        public GameObject fireBullet;
        public GameObject playerUI;
        public Transform canon;
        public Transform anchor;
        [HideInInspector] public Camera _cam;
        [HideInInspector] public PhotonView view;
        [Tooltip("General Values")]
        public float camSpeed = 5;
        [Range(0, 100)] public int aggroValue;
        public List<Transform> targets_Capa = new List<Transform>();

        [Header("Auto Attack")]
        public float fireRate;
        public int damageFire;
        public float precision;
        public float reloadingSpeed;
        public int currentBulletInWeapon;
        public int maxBulletInWeapon;
        public int maxBulletInPlayer;

        [Header("Capa 01")]
        public GameObject gizmos_Capa01;
        public float cooldown_Capa01;
        public float loading_Capa_01;
        public float range_Capa01;
        public bool hasGizmos_Capa_01;
        public bool needTarget_Capa01;
        public int targetNeeded_Capa01;

        [Header("Capa 02")]
        public GameObject gizmos_Capa02;
        public float cooldown_Capa02;
        public float loading_Capa_02;
        public float range_Capa02;
        public bool hasGizmos_Capa_02;
        public bool needTarget_Capa02;
        public int targetNeeded_Capa02;

        [Header("Health & Shield")]
        public int currentLife;
        public int currentShield;
        public int maxLife;
        public int maxShield;

        #endregion


        #region Private Fields

        private NavMeshAgent navigation;
        private HUD_Manager hud;
        private EntityBar bar;
        private Transform model;
        private float timmingFire;
        private float timmingCapa01;
        private float timmingCapa02;
        private bool fireReady = true;
        private bool capa_01_Ready = true;
        private bool capa_02_Ready = true;

        #endregion


        #region MonoBehaviour CallBacks

        public virtual void Start() {
            model = transform.GetChild(0);
            navigation = GetComponent<NavMeshAgent>();
            view = GetComponent<PhotonView>();
            _cam = transform.parent.GetComponentInChildren<Camera>();
            hud = HUD_Manager.manager;
            hud.character = this;

            transform.parent.GetComponentInChildren<Camera>().gameObject.SetActive(photonView.IsMine);

            GameObject _ui = Instantiate(playerUI, GameObject.Find("HUD").transform);
            bar = _ui.GetComponent<EntityBar>();
            _ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        public virtual void Update() {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
                return;
            }
            _cam.transform.parent.position = Vector3.Lerp(_cam.transform.parent.position, transform.position, camSpeed * Time.deltaTime);
            Movements();
            PlayerRotation();
            #region Capa01
            if (Input.GetButtonDown("Capa01") && capa_01_Ready) {
                if (hasGizmos_Capa_01) {
                    gizmos_Capa01.SetActive(true);
                    LaunchGizmosCapa01();
                }
            }
            if (Input.GetButton("Capa01")) {
                GetTargets(ref targets_Capa, targetNeeded_Capa01);
            }
            if (Input.GetButtonUp("Capa01") && capa_01_Ready) {
                if (hasGizmos_Capa_01) gizmos_Capa01.SetActive(false);
                capa_01_Ready = false;
                if (targetNeeded_Capa01 == targets_Capa.Count) {
                    Character_Capa01();
                    capa_01_Ready = false;
                } else if (needTarget_Capa01 == false) {
                    Character_Capa01();
                    capa_01_Ready = false;
                } else {
                    print("Cancelled capa01");
                }
            }
            #endregion
            #region Capa02
            if (Input.GetButtonDown("Capa02") && capa_02_Ready) {
                if (hasGizmos_Capa_02) {
                    gizmos_Capa02.SetActive(true);
                    LaunchGizmosCapa02();
                }
            }
            if (Input.GetButton("Capa02") && capa_02_Ready) {
                GetTargets(ref targets_Capa, targetNeeded_Capa02);
            }
            if (Input.GetButtonUp("Capa02") && capa_02_Ready) {
                if (hasGizmos_Capa_02) gizmos_Capa02.SetActive(false);
                if (targetNeeded_Capa02 == targets_Capa.Count) {
                    Character_Capa02();
                    capa_02_Ready = false;
                } else if (needTarget_Capa02 == false) {
                    Character_Capa02();
                    capa_02_Ready = false;
                } else {
                    print("Cancelled capa02");
                }
            }
            #endregion
            #region Fire
            if (Input.GetButton("Fire")) {
                Fire();
            }
            if (Input.GetButtonUp("Reload")) {
                if (maxBulletInPlayer >= maxBulletInWeapon) {
                    StartCoroutine(RefreshHUD());
                    view.RPC("RPC_Reload", RpcTarget.AllBuffered);
                }
            }
            #endregion
        }

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Projectile")) {
                Projectile projectile = other.GetComponent<Projectile>();
                if(projectile.originalPlayer == transform) {
                    return;
                }
                TakeDamage(projectile.damage);
                Destroy(other.gameObject);
            }
        }

        #endregion


        #region Virtuals Methods

        public virtual void Movements() {
            Vector3 direction = Vector3.zero;
            direction += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            navigation.SetDestination(transform.position + direction);
        }

        public virtual void PlayerRotation() {
            Vector3 lookingAt = Vector3.zero;
            MousePosition(out lookingAt);
            model.LookAt(lookingAt);
            Quaternion rotation = model.rotation;
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            model.rotation = rotation;
        }

        public virtual void Fire() {
            timmingFire -= Time.deltaTime;
            if(timmingFire <= 0 && currentBulletInWeapon > 0) {
                timmingFire = fireRate;
                Quaternion rot = GameManager.instance.GetRandomPrecision(canon.rotation, precision);
                HUD_Manager.manager.Update_Chargeur(currentBulletInWeapon, maxBulletInWeapon);
                view.RPC("RPC_Character_Shoot", RpcTarget.AllBuffered, rot);
                currentBulletInWeapon--;
            }
        }

        public virtual void TakeDamage(int amount) {
            currentLife -= amount;
            print(PhotonNetwork.NickName + "'health is " + currentLife);
            if (currentLife <= 0) {
                GameManager.instance.LeaveRoom();
            }
        }

        public virtual void LaunchGizmosCapa01() {
            print(PhotonNetwork.NickName + "has launch gizmos for capa 01");
        }

        public virtual void LaunchGizmosCapa02() {
            print(PhotonNetwork.NickName + "has launch gizmos for capa 02");
        }

        public virtual void GetTargets(ref List<Transform> entities, int amountNeeded) {
            print("getting targets...");
            if (Input.GetButtonDown("Select")) {
                IEntity entity = GameManager.instance.GetEntity(range_Capa02, transform.position);
                if (entity != null) {
                    if(targets_Capa.Count == amountNeeded){
                        GameManager.instance.DeselectTarget(targets_Capa[0]);
                        targets_Capa.RemoveAt(0);
                    }
                    targets_Capa.Add(entity.GetTransform());
                    print("get target");
                }
            }
            if(targets_Capa.Count == amountNeeded) {
                print("get all targets");
            }
        }

        public virtual void DeselectAllTargets() {
            foreach (var item in targets_Capa) {
                GameManager.instance.DeselectTarget(item);
            }
            targets_Capa.Clear();
            print("all targets cleared");
        }


        public virtual void Character_Capa01() {
            print("capa01 Launched from " + PhotonNetwork.NickName);
        }

        public virtual void Character_Capa02() {
            print("capa02 Launched from " + PhotonNetwork.NickName);
        }

        #endregion


        #region Private Methods

        private void MousePosition(out Vector3 result) {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            float point = 0f;

            if (plane.Raycast(ray, out point)) {
                result = ray.GetPoint(point);
            } else {
                result = Vector3.zero;
            }
        }

        private IEnumerator Reload() {
            yield return new WaitForSeconds(reloadingSpeed);
            currentBulletInWeapon = maxBulletInWeapon;
            maxBulletInPlayer -= maxBulletInWeapon;
        }

        private IEnumerator RefreshHUD() {
            yield return new WaitForSeconds(reloadingSpeed + .1f);
            HUD_Manager.manager.Update_Chargeur(currentBulletInWeapon, maxBulletInWeapon);
        }

        #endregion


        #region Public Methods

        public IEnumerator RecoverCapa01() {
            float t = 0;
            while (t < cooldown_Capa01) {
                t += Time.deltaTime;
                hud.Update_Capa01_Cooldown(t / cooldown_Capa01);
                yield return null;
            }
            capa_01_Ready = true;
        }

        public IEnumerator RecoverCapa02() {
            float t = 0;
            while (t < cooldown_Capa02) {
                t += Time.deltaTime;
                hud.Update_Capa02_Cooldown(t / cooldown_Capa02);
                yield return null;
            }
            capa_02_Ready = true;
        }

        #endregion


        #region RPC Methods

        [PunRPC]
        public virtual void RPC_Character_Shoot(Quaternion rot) {
            GameObject bullet = Instantiate(fireBullet, canon.position, rot);
            bullet.GetComponent<Projectile>().Setup(transform, damageFire);
        }

        [PunRPC]
        public virtual void RPC_Reload() {
            StartCoroutine(bar.Reloading(reloadingSpeed));
            StartCoroutine(Reload());
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
            return PhotonNetwork.NickName;
        }

        #endregion


        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(currentBulletInWeapon);
                stream.SendNext(maxBulletInWeapon);
                stream.SendNext(maxBulletInPlayer);
            } else 
            if (stream.IsReading) {
                currentBulletInWeapon = (int)stream.ReceiveNext();
                maxBulletInWeapon = (int)stream.ReceiveNext();
                maxBulletInPlayer = (int)stream.ReceiveNext();
            }
        }

        #endregion
    }
}
/*
    #region Variables
    public float camSpeed = 5;
    public bool canShoot = true;
    public bool isStun;

    public GameObject entityBar;
    public GameObject fireBullet;
    public Transform cam;
    public Transform model;
    public Transform canon;

    public Character _char;

    [Header("Gizmos & Tests")]
    public Mesh ring_AA;

    private NavMeshAgent navigation;
    private Camera _cam;
    private Vector3 camOffset;

    private int maxBullet, currentBullet, maxBulletInWeapon;
    #endregion

    #region Testing Methods
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawMesh(ring_AA, 0, transform.position, Quaternion.identity, Vector3.one * _char.AA_range);
    }
    #endregion

    // unity internal methods
    public virtual void Awake() {
        navigation = GetComponent<NavMeshAgent>();
        _cam = cam.GetComponent<Camera>();

        navigation.speed = _char.maxSpeed;
        navigation.acceleration = _char.acceleration;

        maxBullet = _char.maxBullet;
        currentBullet = _char.currentBullet;
        maxBulletInWeapon = _char.maxBulletInWeapon;

        camOffset = cam.position + transform.position;
        GameObject bar = Instantiate(entityBar, GameObject.Find("HUD").transform);
        bar.GetComponent<EntityBar>().target = this.transform;
    }

    public virtual void Update (){
        cam.position = Vector3.Lerp(cam.position, transform.position + camOffset, camSpeed * Time.deltaTime);
        Movements();
        PlayerRotation();

        if (Input.GetButtonDown("Capa01") && _char.capa_01_Loaded) {
            _char.capa_01_Loaded = false;
        }
        if (Input.GetButtonDown("Capa02") && _char.capa_02_Loaded) {
            _char.capa_02_Loaded = false;
        }
        if (Input.GetButtonDown("Fire") && canShoot) {
            StartCoroutine(Fire());
        }
        if (Input.GetButtonUp("Fire") && currentBullet > 0) {
            StopCoroutine(Fire());
        }
        if (Input.GetButtonUp("Reload")) {
            Reload();
        }
    }

    #region Entity Interface
    //*********************
    public virtual void RemoveLife (int amount) {
        if (photonView.IsMine) {
            photonView.RPC("TakeDamage", RpcTarget.All, amount);
        }
    }
    [PunRPC]
    private void TakeDamage(int amount) {
        _char.currentLife -= amount;
        if (_char.currentLife <= 0) {
            Death();
        }
    }
    //*********************

    //*********************
    public virtual void AddLife (int amount){
        if (photonView.IsMine) {
            photonView.RPC("GetHeal", RpcTarget.All, amount);
        }
    }
    [PunRPC]
    private void GetHeal(int amount) {
        _char.currentLife += amount;
        if (_char.currentLife > _char.maxLife) {
            _char.currentLife = _char.maxLife;
        }
    }
    //*********************

    //*********************
    public void RemoveShield(int amount) {
        if (photonView.IsMine) {
            photonView.RPC("GetDamageOnShield", RpcTarget.All, amount);
        }
    }
    [PunRPC]
    private void GetDamageOnShield(int amount) {
        _char.currentShield -= amount;
        if (_char.currentShield == 0) {
            _char.currentShield = 0;
        }
    }
    //*********************

    //*********************
    public void AddShield(int amount) {
        if (photonView.IsMine) {
            photonView.RPC("GetHealOnShield", RpcTarget.All, amount);
        }
    }
    [PunRPC]
    private void GetHealOnShield(int amount) {
        _char.currentShield += amount;
        if (_char.currentShield > _char.maxShield) {
            _char.currentShield = _char.maxShield;
        }
    }
    //*********************

    public int GetShield() {
        return _char.currentShield;
    }

    public int GetLife() {
        return _char.currentLife;
    }

    public int GetMaxLife() {
        return _char.maxLife;
    }

    public int GetMaxShield() {
        return _char.maxShield;
    }
    public Transform GetTransform() {
        return transform;
    }
    #endregion


    public virtual void GetStun (float duration){
        StartCoroutine(Stun(duration));
    }

    public virtual void Movements (){
        Vector3 direction=Vector3.zero;
        direction+= new Vector3( Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        navigation.SetDestination(transform.position + direction);
    }

    private void PlayerRotation() {
        Vector3 lookingAt = Vector3.zero;
       // GameManager.gm.MousePosition(out lookingAt);
        model.LookAt(lookingAt);
        Quaternion rotation = model.rotation;
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        model.rotation = rotation;
    }

    public virtual void Death() {

    }
    //****************************
    public virtual IEnumerator Fire() {
        if (photonView.IsMine) {
            canShoot = false;
            photonView.RPC("FireRPC", RpcTarget.All);
            HUD_Manager.manager.RefreshMunitionDisplay(currentBullet, maxBullet);
            yield return new WaitForSeconds(_char.fireRate);
            if (currentBullet > 0) {
                canShoot = true;
                if (Input.GetButton("Fire")) {
                    StartCoroutine(Fire());
                }
            }
        }
    }
    [PunRPC]
    public void FireRPC() {
        GameObject bullet = Instantiate(fireBullet, canon.position, canon.rotation);
        bullet.GetComponent<Projectile>().Setup(transform.position, _char.AA_range, _char.damageFire);
        currentBullet--;
    }
    //****************************

    #region Capacités
    [PunRPC]
    public void LaunchCapa01() {
        print("hi");
        StartCoroutine(Capa_01());
    }
    public virtual IEnumerator Capa_01() {
        print("capa01 launched !");
        yield return new WaitForSeconds(_char.capa_01_Cooldown);
        _char.capa_01_Loaded = true;
        print("capa01 loaded !");
    }
    [PunRPC]
    public void LaunchCapa02() {
        StartCoroutine(Capa_02());
    }
    public virtual IEnumerator Capa_02() {
        print("capa02 launched !");
        yield return new WaitForSeconds(_char.capa_02_Cooldown);
        _char.capa_02_Loaded = true;
        print("capa02 loaded !");
    }
    #endregion

    public void Reload() {
        if (maxBullet - maxBulletInWeapon >= 0) {
            currentBullet = maxBulletInWeapon;
            maxBullet -= maxBulletInWeapon;
            canShoot = true;
            HUD_Manager.manager.RefreshMunitionDisplay(currentBullet, maxBullet);
        }
    }

    private IEnumerator Stun (float duration){
        isStun = true;
        yield return new WaitForSeconds(duration);
        isStun = false;
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_char.currentLife);
            stream.SendNext(_char.currentShield);
        } else if (stream.IsReading) {
            _char.currentLife = (int)stream.ReceiveNext();
            _char.currentShield = (int)stream.ReceiveNext();
        }
    }
}*/
