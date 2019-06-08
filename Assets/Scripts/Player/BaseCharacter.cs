using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseCharacter : MonoBehaviour, IEntity{
    public Transform cam;
    //[HideInInspector]
    public float maxSpeed, acceleration;
    public float camSpeed = 5;
    public string playerName;

    [SerializeField] private float maxLife, currentLife;
    [SerializeField] private float maxEnergy, currentEnergy;

    [HideInInspector] public bool isStun;

    private NavMeshAgent navigation;
    private Vector3 camOffset;

    // unity internal methods
    public virtual void Awake() {
        navigation = GetComponent<NavMeshAgent>();

        navigation.speed = maxSpeed;
        navigation.acceleration = acceleration;

        camOffset = cam.position + transform.position;
    }

    void Update (){
        cam.position = Vector3.Lerp(cam.position, transform.position + camOffset, camSpeed * Time.deltaTime);
        Movements();
    }
    #region Entity
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
    public void RemoveEnergy(float amount) {
        currentEnergy -= amount;
        if(currentEnergy == 0) {
            currentEnergy = 0;
        }
    }

    public void AddEnergy(float amount) {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy) {
            currentEnergy = maxEnergy;
        }
    }

    public float GetEnergy() {
        return currentEnergy;
    }

    public float GetLife() {
        return currentLife;
    }

    public float GetMaxLife() {
        return maxLife;
    }

    public float GetMaxEnergy() {
        return maxEnergy;
    }
    public string GetPlayerName() {
        return playerName;
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

    public virtual void Death() {

    }
    // private methods for internal use
    private IEnumerator Stun (float duration){
        isStun = true;
        yield return new WaitForSeconds(duration);
        isStun = false;
    }
}