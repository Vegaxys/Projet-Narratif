using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon;
using Photon.Pun;

public class BaseCharacter : MonoBehaviourPun, IEntity{
    
    public float camSpeed = 5;
    public bool canShoot = true;
    public bool isStun;

    public GameObject entityBar;
    public Transform cam;
    public Transform model;
    public Transform canon;

    public Character _char;

    [Header("Gizmos & Tests")]
    public Mesh ring_AA;

    private NavMeshAgent navigation;
    private Camera _cam;
    private Vector3 camOffset;

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

        if (photonView.IsMine) {
            camOffset = cam.position + transform.position;
        } else {
            cam.gameObject.SetActive(false);
        }
        GameObject bar = Instantiate(entityBar, GameObject.Find("HUD").transform);
        bar.GetComponent<EntityBar>().target = this.transform;
    }

    void Update (){
        if (photonView.IsMine) {
            cam.position = Vector3.Lerp(cam.position, transform.position + camOffset, camSpeed * Time.deltaTime);
            Movements();
            PlayerRotation();

            if (Input.GetButtonDown("Capa01")) {
                Capa_01();
            }
            if (Input.GetButtonDown("Capa02")) {
                Capa_02();
            }
            if (Input.GetButtonDown("Fire") && canShoot) {
                StartCoroutine(Fire());
            }
            if (Input.GetButtonUp("Fire") && _char.currentBullet > 0) {
                StopCoroutine(Fire());
            }
            if (Input.GetButtonUp("Reload")) {
                Reload();
            }
        }
    }

    #region Entity Interface
    public virtual void RemoveLife (int amount) {
        _char.currentLife -= amount;
        if(_char.currentLife <= 0) {
            Death();
        }
    }

    public virtual void AddLife (int amount){
        _char.currentLife += amount;
        if(_char.currentLife > _char.maxLife) {
            _char.currentLife = _char.maxLife;
        }
    }
    public void RemoveShield(int amount) {
        _char.currentShield -= amount;
        if(_char.currentShield == 0) {
            _char.currentShield = 0;
        }
    }

    public void AddShield(int amount) {
        _char.currentShield += amount;
        if (_char.currentShield > _char.maxShield) {
            _char.currentShield = _char.maxShield;
        }
    }

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
        navigation.SetDestination(transform.position+direction);
    }

    private void PlayerRotation() {
        model.LookAt(GameManager_Dungeon.dungeon.MousePosition());
        Quaternion rotation = model.rotation;
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        model.rotation = rotation;
    }

    public virtual void Death() {

    }
    public virtual IEnumerator Fire() {
        print("fire");
        canShoot = false;
        GameManager_Dungeon.dungeon
            .GetBullet(_char.bulletName, canon.position, canon.rotation)
            .GetComponent<Projectile>()
            .Setup(transform.position, _char.AA_range);
        _char.currentBullet--;
        HUD_Manager.manager.RefreshMunitionDisplay(_char.currentBullet, _char.maxBullet);
        yield return new WaitForSeconds(_char.fireRate);
        if (_char.currentBullet > 0) {
            canShoot = true;
        }
    }
    public virtual void Capa_01() {
        print("capa01");
    }
    public virtual void Capa_02() {
        print("capa02");
    }
    public void Reload() {
        if (_char.maxBullet - _char.maxBulletInWeapon > 0) {
            _char.currentBullet = _char.maxBulletInWeapon;
            HUD_Manager.manager.RefreshMunitionDisplay(_char.currentBullet, _char.maxBullet);
            _char.maxBullet -= _char.maxBulletInWeapon;
        }
    }

    private IEnumerator Stun (float duration){
        isStun = true;
        yield return new WaitForSeconds(duration);
        isStun = false;
    }
}