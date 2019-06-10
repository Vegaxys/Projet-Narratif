using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon;
using Photon.Pun;

public class BaseCharacter : MonoBehaviourPun, IEntity{
    public Transform cam, model;
    //[HideInInspector]
    public float maxSpeed, acceleration;
    public float camSpeed = 5;
    public string avatarName;
    public GameObject entityBar;

    [Header("Variables Personnage")]
    public int damageFire;
    public float fireRate;
    public GameObject prefabBullet;
    public Transform canon;

    [SerializeField] private float maxLife, currentLife;
    [SerializeField] private float maxEnergy, currentEnergy;

    [HideInInspector] public bool isStun;

    private NavMeshAgent navigation;
    private Camera _cam;
    private Vector3 camOffset;

    // unity internal methods
    public virtual void Awake() {
        navigation = GetComponent<NavMeshAgent>();
        _cam = cam.GetComponent<Camera>();

        navigation.speed = maxSpeed;
        navigation.acceleration = acceleration;

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
            if (Input.GetButtonDown("Fire")) {
                Fire();
            }
        }
    }

    #region Entity Interface
    public virtual void RemoveLife (float amount) {
        currentLife -= amount;
        if(currentLife <= 0) {
            Death();
        }
    }

    public virtual void AddLife (float amount){
        currentLife += amount;
        if(currentLife > maxLife) {
            currentLife = maxLife;
        }
    }
    public void RemoveShield(float amount) {
        currentEnergy -= amount;
        if(currentEnergy == 0) {
            currentEnergy = 0;
        }
    }

    public void AddShield(float amount) {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy) {
            currentEnergy = maxEnergy;
        }
    }

    public float GetShield() {
        return currentEnergy;
    }

    public float GetLife() {
        return currentLife;
    }

    public float GetMaxLife() {
        return maxLife;
    }

    public float GetMaxShield() {
        return maxEnergy;
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
        GameObject bullet = GameManager_Dungeon.dungeon.GetBullet(avatarName + "_bullet", canon.position, canon.rotation);

        yield return new WaitForSeconds(fireRate);
    }
    public virtual void Capa_01() {
        print("capa01");
    }
    public virtual void Capa_02() {
        print("capa02");
    }

    private IEnumerator Stun (float duration){
        isStun = true;
        yield return new WaitForSeconds(duration);
        isStun = false;
    }
}