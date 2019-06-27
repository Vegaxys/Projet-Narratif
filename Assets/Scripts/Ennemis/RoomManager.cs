﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Vegaxys {
    public class RoomManager :MonoBehaviour
    {
        public Vector3 center;
        public Vector3 size;
        public Color color;
        public List<Spawner> spawners = new List<Spawner>();

        private void OnDrawGizmos() {
            GetComponent<BoxCollider>().size = size;
            Gizmos.color = color;
            Gizmos.DrawWireCube(transform.position + center, size);
            
        }

        [ContextMenu("Pupulate")]
        public void Pupulate() {
            foreach (Transform child in transform) {
                spawners.Add(child.GetComponent<Spawner>());
            }
        }

        public Vector3 NewPosition() {
            Vector3 result = Vector3.zero;

            result.x = Random.Range(transform.position.x + center.x + (-size.x / 2), transform.position.x + center.x + (size.x / 2));
            result.y = 0;
            result.z = Random.Range(transform.position.z + center.z + (-size.z / 2), transform.position.z + center.z + (size.z / 2));

            return result;
        }
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                spawners.ForEach(x => x.Spawn());
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Ennemi")) {
                Vector3 newPos = NewPosition();
                other.GetComponentInParent<Ennemi>().GetNewPos(newPos);
            }
        }
    }
}
