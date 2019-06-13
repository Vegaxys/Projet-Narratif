using UnityEngine;

public class Projectile :MonoBehaviour{

    public float speed;
    public float range;
    public int damage;

    [HideInInspector]
    public Vector3 origin;
    [HideInInspector]
    public Transform originalPlayer;

    public virtual void Setup(Transform _transform, float _range, int _damage) {
        range = _range;
        damage = _damage;
        originalPlayer = _transform;
        origin = _transform.position;
    }

    public virtual void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Vector3.Distance(origin, transform.position) > range) {
            Destroy(gameObject);
        }
    }
}
