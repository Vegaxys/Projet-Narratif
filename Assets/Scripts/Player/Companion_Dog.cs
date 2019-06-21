using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Vegaxys
{
    public class Companion_Dog :MonoBehaviour
    {
        public Transform target;
        public Mesh ring;
        [Range(1, 50)] public float stoppingDistance;
        [Range(1, 50)] public float aggroDistance;
        public int damageAmount;

        [HideInInspector] public BaseCharacter character;
        [HideInInspector] public Capa_Chien capa;

        private NavMeshAgent agent;

        public enum DogState
        {
            IDLE,
            ATTACK,
            DEFENSE
        }
        private DogState dogState;

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawMesh(ring, 0, transform.position, transform.rotation, Vector3.one * stoppingDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(ring, 0, transform.position, transform.rotation, Vector3.one * aggroDistance);
        }

        private void Start() {
            agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = stoppingDistance;
            StartCoroutine(UpdateDestination());
        }

        public void LaunchDog(DogState state, Transform _target) {
            StopAllCoroutines();
            target = _target;
            dogState = state;
            StartCoroutine(UpdateDestination());
        }

        private IEnumerator UpdateDestination() {
            switch (dogState) {
                case DogState.IDLE:
                    if (Vector3.Distance(transform.position, target.position) > aggroDistance -1) {
                        agent.SetDestination(GameManager.instance.GetRandomPositionInTorus(target.position, aggroDistance / 3, stoppingDistance, true));
                    }
                    yield return new WaitForSeconds(.1f);
                    break;
                case DogState.ATTACK:
                    agent.SetDestination(target.position);
                    yield return new WaitUntil(() => Vector3.Distance(transform.position, target.position) < stoppingDistance + 1.3f);
                    target.GetComponent<Ennemi>().TakeDamage(damageAmount);
                    yield return new WaitForSeconds(.4f);
                    break;
                case DogState.DEFENSE:
                    print("hi");
                    if(Vector3.Distance(transform.position, target.position) > capa.capa_Range){
                        agent.SetDestination(GameManager.instance.GetRandomPositionInTorus(target.position, aggroDistance / 3, stoppingDistance, true));
                    }
                    yield return new WaitForSeconds(.5f);
                    break;
            }
            if (Vector3.Distance(transform.position, character.transform.position) > capa.capa_Range) {
                target = character.transform;
                dogState = DogState.IDLE;
            }
            StartCoroutine(UpdateDestination());
        }
    }
}
