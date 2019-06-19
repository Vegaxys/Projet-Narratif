using System.Collections;
using UnityEngine.AI;
using UnityEngine;

namespace Vegaxys
{
    public class Companion_Drone :MonoBehaviour
    {
        public enum CompanionState
        {
            IDLE,
            HEALING,
            DAMAGING
        }

        public int tics;
        public float waiting;
        public int damageAmount;
        public int healAmount;

        private Transform target;
        private Transform companionContainer;
        [HideInInspector] public BaseCharacter character;

        public void Start() {
            companionContainer = GameObject.Find("Companions").transform;
            LaunchDrone(CompanionState.IDLE, character.transform);
        }

        public void LaunchDrone(CompanionState state, Transform _target) {
            target = _target;
            StartCoroutine(Travelling(state));
        }

        private IEnumerator Travelling(CompanionState state) {
            yield return MoveTo(target, 3);
            switch (state) {
                case CompanionState.HEALING:
                    for (int i = 0; i < tics; i++) {
                        print("healing");
                        transform.parent.GetComponent<BaseCharacter>().AddHealth(healAmount);
                        yield return new WaitForSeconds(waiting);
                    }
                    break;
                case CompanionState.DAMAGING:
                    for (int i = 0; i < tics; i++) {
                        print("damaging");
                        transform.parent.GetComponent<Ennemi>().TakeDamage(GameManager.instance.GetRandomDamage(damageAmount));
                        yield return new WaitForSeconds(waiting);
                    }
                    break;
            }
            yield return new WaitForSeconds(1);
            yield return MoveTo(character.transform, 1);
        }

        private IEnumerator MoveTo(Transform destination, float speed) {
            transform.parent = companionContainer;
            float t = 0;
            Vector3 oldPos2 = transform.position;
            while (t < 1) {
                transform.position = Vector3.Lerp(oldPos2, destination.position, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
            transform.parent = destination;
        }
    }
}
