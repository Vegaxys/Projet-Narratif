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

        public override void Virtual_Character_Capa02() {
            base.Virtual_Character_Capa02();
            StartCoroutine(TimingFuFu());
        }

        IEnumerator TimingFuFu() {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = onFufu;
            yield return new WaitForSeconds(duration);
            transform.GetChild(0).GetComponent<MeshRenderer>().material = offFufu;
            StartCoroutine(RecoverCapa02());
        }


        // Capa 01


        public override void Virtual_Character_Capa01() {
            base.Virtual_Character_Capa01();
            Reload(0);
            view.RPC("RPC_Capa01_Dash", RpcTarget.AllBuffered);
            StartCoroutine(RecoverCapa01());
        }

        public override void Virtual_GetAOE() {
            base.Virtual_GetAOE();
            gizmos_Capa02.transform.position = GameManager.instance.MousePosition(range_Capa01, transform.position);
        }

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
