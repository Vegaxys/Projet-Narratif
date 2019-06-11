using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon;
using Photon.Pun;

public class Ennemi :MonoBehaviourPun, IEntity{

    public Transform model;
    //[HideInInspector]
    public float maxSpeed, acceleration;
    public string avatarName;
    public GameObject entityBar;

    [Header("Variables Ennemi")]
    public int damageFire;
    public float fireRate;
    public float aggroRange;
    public int aggroValue;
    public Transform canon;
    private bool canShoot = true;

    [SerializeField] private int maxLife, currentLife;
    [SerializeField] private int maxShield, currentShield;

    [HideInInspector] public bool isStun;

    private NavMeshAgent navigation;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, aggroRange);
    }

    public virtual void Update() {
        if (navigation.isStopped && canShoot) {
            StartCoroutine(Fire());
        }
    }
    public virtual void GoTo(IEntity entity) {
        navigation.destination = entity.GetTransform().position;
    }
    public virtual IEnumerator Fire() {
        canShoot = false;
        GameObject bullet = GameManager_Dungeon.dungeon.GetBullet(avatarName + "_bullet", canon.position, canon.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    #region Entity Interface
    public void AddLife(int amount) {
        currentLife += amount;
        if (currentLife > maxLife) {
            currentLife = maxLife;
        }
    }

    public void AddShield(int amount) {
        currentShield += amount;
        if (currentShield > maxShield) {
            currentShield = maxShield;
        }
    }
    public void RemoveLife(int amount) {
        currentLife -= amount;
        if (currentLife <= 0) {
            // Death();
        }
    }

    public void RemoveShield(int amount) {
        currentShield -= amount;
        if (currentShield == 0) {
            currentShield = 0;
        }
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

    public int GetShield() {
        return currentShield;
    }
    public Transform GetTransform() {
        return transform;
    }
    #endregion

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Projectile") {
            other.gameObject.SetActive(false);
        }
    }
}
