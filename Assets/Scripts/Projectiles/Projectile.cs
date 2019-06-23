using UnityEngine;

public class Projectile :MonoBehaviour{

    public float speed;
    public int damage;

    public int dot_Damage;
    public int dot_Row;

    [HideInInspector] public Vector3 origin;
    [HideInInspector] public Transform originalPlayer;
    [HideInInspector] public bool isHealing;


    public virtual void Setup(Transform _transform, int _damage, int dotDamage, int dotRow) {
        damage = _damage;
        originalPlayer = _transform;
        origin = _transform.position;

        dot_Damage = dotDamage;
        dot_Row = dotRow;
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
