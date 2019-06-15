using Photon.Pun;
using UnityEngine;
namespace Vegaxys {
    public class RoomManager :MonoBehaviour
    {
        public Vector3 center;
        public Vector3 size;
        public Color color;

        private void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawWireCube(transform.position + center, size);
        }

        public Vector3 NewPosition() {
            Vector3 result = Vector3.zero;

            result.x = Random.Range(transform.position.x + center.x + (-size.x / 2), transform.position.x + center.x + (size.x / 2));
            result.y = 0;
            result.z = Random.Range(transform.position.z + center.z + (-size.z / 2), transform.position.z + center.z + (size.z / 2));

            return result;
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Ennemi")) {
                Vector3 newPos = NewPosition();
                other.GetComponentInParent<Ennemi>().view.RPC("RPC_GetNewPos", RpcTarget.AllBuffered, newPos);
            }
        }
    }
}
