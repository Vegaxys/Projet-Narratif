using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon;
using Photon.Pun;

public class Ennemi :MonoBehaviourPun, IEntity, IPunObservable{

    public Transform model;
    //[HideInInspector]
    public float maxSpeed, acceleration;
    public string avatarName;
    public GameObject entityBar;

    [Header("Variables Ennemi")]
    public int damageFire;
    public float fireRate;
    public float aggroRange;
    public Transform canon;
    private bool canShoot = true;
    [Header("Gizmos & Tests")]
    public Mesh ringAggro;

    [SerializeField] private int maxLife, currentLife;
    [SerializeField] private int maxShield, currentShield;

    [HideInInspector] public bool isStun;

    private NavMeshAgent navigation;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawMesh(ringAggro, 0, transform.position, Quaternion.identity, Vector3.one * aggroRange);
    }

    public virtual void Awake() {
        navigation = GetComponent<NavMeshAgent>();
        GameObject bar = Instantiate(entityBar, GameObject.Find("HUD").transform);
       // bar.GetComponent<EntityBar>().target = this.transform;
    }

    public virtual void Update() {
        if (navigation.isStopped && canShoot) {
            StartCoroutine(Fire());
        }
    }
    public virtual void GoTo(IEntity entity) {
     //   navigation.destination = entity.GetTransform().position;
    }
    public virtual IEnumerator Fire() {
        canShoot = false;
        //GameObject bullet = GameManager_Dungeon.dungeon.GetBullet(avatarName + "_bullet", canon.position, canon.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    #region Entity Interface
    //*********************
    public virtual void RemoveLife(int amount) {
        if (photonView.IsMine) {
            photonView.RPC("TakeDamage", RpcTarget.All, amount);
        }
    }
    [PunRPC]
    private void TakeDamage(int amount) {
        currentLife -= amount;
        if (currentLife <= 0) {
         //   Death();
        }
    }
    //*********************

    //*********************
    public virtual void AddLife(int amount) {
        if (photonView.IsMine) {
            photonView.RPC("GetHeal", RpcTarget.All, amount);
        }
    }
    [PunRPC]
    private void GetHeal(int amount) {
        currentLife += amount;
        if (currentLife > maxLife) {
            currentLife = maxLife;
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
        currentShield -= amount;
        if (currentShield == 0) {
            currentShield = 0;
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
        currentShield += amount;
        if (currentShield > maxShield) {
            currentShield = maxShield;
        }
    }
    //*********************

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
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(currentLife);
            stream.SendNext(currentShield);
        } else if (stream.IsReading) {
            currentLife = (int)stream.ReceiveNext();
            currentShield = (int)stream.ReceiveNext();
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerBullet") {
            RemoveLife(other.GetComponent<Projectile>().damage);
            other.gameObject.SetActive(false);
        }
    }
}
