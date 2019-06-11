using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Capa02_Exile :Projectile{

    private Transform target;

    public override void Update() {
        
    }
    public override void Setup(Vector3 _origin, float _range, int _damage) {
        target = SelectionManager.selection.selectionTransform;
        base.Setup(_origin, _range, _damage);
        if (photonView.IsMine) {
            StartCoroutine(GoToTarget());
        }
    }
    private IEnumerator GoToTarget() {
        Vector3 initPos = transform.position;
        float t = 0;
        while(Vector3.Distance(transform.position, target.position) > 0) {
            print(t);
            transform.LookAt(target);
            transform.position = Vector3.Lerp(initPos, target.position, t);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
