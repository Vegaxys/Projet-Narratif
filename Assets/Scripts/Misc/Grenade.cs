using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class Grenade :Projectile
    {
        private float xx, yy, zz;
        public Vector3 destination;
        private bool amorced;

        private void Start() {
            xx = Random.Range(-10, 10);
            yy = Random.Range(-10, 10);
            zz = Random.Range(-10, 10);
            StartCoroutine(GoToPosition());
        }

        public override void Update() {
            transform.Rotate(new Vector3(xx, yy, zz));
        }
        private IEnumerator GoToPosition() {
            float t = 0;
            Vector3 oltPos = transform.parent.position;
            while (t < 1) {
                t += Time.deltaTime;
                transform.parent.position = Vector3.Lerp(oltPos, destination, t);
                yield return null;
            }
        }
        public override void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Untagged")) {
                    GetComponent<SphereCollider>().radius = 2.4f;
                    Destroy(transform.parent.gameObject, .1f);
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                transform.tag = "Grenade";
            }
        }
    }
}
