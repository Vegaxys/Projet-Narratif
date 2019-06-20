using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Vegaxys
{
    public class Companion_Dog :MonoBehaviour
    {
        public Transform target;
        public int damageAmount;

        [HideInInspector] public BaseCharacter character;
        private NavMeshAgent agent;   

        public enum DogState
        {
            IDLE,
            ATTACK,
            DEFENSE
        }
        private DogState dogState;

        private void Start() {
            agent = GetComponent<NavMeshAgent>();
            StartCoroutine(UpdateDestination());
        }

        public void LaunchDog(DogState state, Transform _target) {
            target = _target;
            dogState = state;
        }

        private IEnumerator UpdateDestination() {
            agent.SetDestination(GameManager.instance.GetRandomPositionInTorus(target.position, 3, 1, true));
            switch (dogState) {
                case DogState.IDLE:
                    break;
                case DogState.ATTACK:
                    Attack();
                    break;
                case DogState.DEFENSE:
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(.5f);
            StartCoroutine(UpdateDestination());
        }

        private void Attack() {
            target.GetComponent<Ennemi>().TakeDamage(damageAmount);
        }
        /// <summary>
        /// Update position par rapport au maitre(void)
        /// Update target void
        /// Update position par rapport au target
        /// Do state
        /// 
        /// </summary>


        




    }
}
