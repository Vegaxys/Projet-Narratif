using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Capa02_Exile :Projectile{

    private Transform target;

    public override void Update() {
        
    }
    public override void Setup(Transform _target, float _range, int _damage) {
        range = _range;
        damage = _damage;
        target = _target;
        StartCoroutine(GoToTarget());
    }
    private IEnumerator GoToTarget() {
        Vector3 initPos = transform.position;
        float t = 0;
        while(Vector3.Distance(transform.position, target.position) > 0) {
            transform.LookAt(target);
            transform.position = Vector3.Lerp(initPos, target.position, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        Destroy(gameObject);
    }
}
