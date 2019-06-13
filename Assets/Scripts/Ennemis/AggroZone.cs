using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys
{
    public class AggroZone : MonoBehaviour
    {
        public Ennemi entity;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                entity.AddCharacter(other.GetComponent<BaseCharacter>());
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                entity.RemoveCharacter(other.GetComponent<BaseCharacter>());
            }
        }
    }
}
