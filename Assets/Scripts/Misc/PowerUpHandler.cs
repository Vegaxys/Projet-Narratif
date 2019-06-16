using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys
{
    public enum PowerUpType
    {
        SHIELD,
        HEALTH,
        GRENADE,
        MUNITION
    }
    public class PowerUpHandler :MonoBehaviour
    {
        public GameObject powerUpPrefab;
        public Mesh ring_Bonus;
        public SphereCollider sphere;
        public Transform pivotPoint;
        public Vector3 rot;
        public float latence;

        [Range(1, 10f)] public float rangeAction;
        [Header("0 = no respawn")]
        public float spawnTimer;
        public PowerUpType type;

        public Transform powerUpTransform;
       
        private void Start() {
            powerUpTransform = transform.parent.GetChild(2);
        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(0, 0, 0.7f, 1);
            Gizmos.DrawMesh(ring_Bonus, transform.position, Quaternion.identity, Vector3.one * rangeAction);
            sphere.radius = rangeAction;
        }

        private void Update() {
            if(powerUpTransform != null) {
                powerUpTransform.Rotate(rot * Time.deltaTime);
            }
        }

        public IEnumerator PickPowerUp(Transform goal) {
            print(goal.name);
            float t = 0;
            while(t < 1) {
                t += Time.deltaTime * latence;
                if (powerUpTransform != null)
                    powerUpTransform.position = Vector3.Lerp(pivotPoint.position, goal.position, t);
                yield return null;
            }
            StartCoroutine(SpawnNewOne());
        }

        private IEnumerator SpawnNewOne() {
            yield return new WaitForSeconds(spawnTimer);
            powerUpTransform = Instantiate(powerUpPrefab, transform.parent).transform;
        }
    }
}
