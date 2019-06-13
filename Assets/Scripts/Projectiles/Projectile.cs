﻿using UnityEngine;
using Photon.Pun;

public class Projectile :MonoBehaviourPun{

    public float speed;
    public float range;
    public int damage;

    [HideInInspector]
    public Vector3 origin;

    public virtual void Setup(Vector3 _origin, float _range, int _damage) {
        origin = _origin;
        range = _range;
        damage = _damage;
    }

    public virtual void Update() {
        if (photonView.IsMine) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (Vector3.Distance(origin, transform.position) > range) {
                gameObject.SetActive(false);
            }
        }
    }
}
