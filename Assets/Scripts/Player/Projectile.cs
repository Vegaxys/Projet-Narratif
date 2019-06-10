using UnityEngine;

public class Projectile : MonoBehaviour{

    public float speed;
    public float range;
    public Vector3 origin;

    public void Setup(Vector3 _origin, float _range) {
        origin = _origin;
        range = _range;
    }

    private void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(Vector3.Distance(origin, transform.position) > range) {
            gameObject.SetActive(false);
        }
    }
}
