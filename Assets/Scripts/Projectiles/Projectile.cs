using UnityEngine;

public class Projectile :MonoBehaviour{

    public float speed;
    public int damage;

    [HideInInspector]
    public Vector3 origin;
    [HideInInspector]
    public Transform originalPlayer;

    public virtual void Setup(Transform _transform, int _damage) {
        damage = _damage;
        originalPlayer = _transform;
        origin = _transform.position;
    }

    public virtual void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    public virtual void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Untagged")) {
            Destroy(gameObject);
            print("destroyed on Unttaged");
        }

    }
}
