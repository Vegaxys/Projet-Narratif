using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Vegaxys {
    public class Character_Epsilon :BaseCharacter
    {

        [Header("Capa01 Dash")]
        private bool isDashing;

        [Header("Capa02 Camouflage")]
        public Material onFufu;
        public Material offFufu;
        public float duration;

        IEnumerator TimingFuFu() {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = onFufu;
            yield return new WaitForSeconds(duration);
            transform.GetChild(0).GetComponent<MeshRenderer>().material = offFufu;
        }


        // Capa 01


        public override void Virtual_Movements() {
            if (!isDashing) {
                base.Virtual_Movements();
            }
        }

        [PunRPC]
        private void RPC_Capa01_Dash(Vector3 position) {
            StartCoroutine(Capa01_Dash(position));
        }

        private IEnumerator Capa01_Dash(Vector3 destination) {
            navigation.SetDestination(destination);
            isDashing = true;
            while (!navigation.isStopped) {
                yield return null;
            }
            isDashing = false;
        }
    }
}
