using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class Capa_Dash :Capa
    {
        public float speedDash;

        public override void Start() {
            base.Start();
            capa_GizAOE_Range = 0.8f;
        }

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            StartCoroutine(Dash());
        }

        private IEnumerator Dash() {
            character.agent.enabled = false;
            Vector3 oldPos = transform.position;
            Vector3 newPos = GameManager.instance.MousePosition(capa_Range, transform.position);
            newPos.y = oldPos.y;
            float t = 0;
            while (t < 1) {
                character.transform.position = Vector3.Lerp(oldPos, newPos, t);
                print(transform.position);
                t += Time.deltaTime * speedDash;
                yield return null;
            }
            character.agent.enabled = true;
            base.RPC_Virtual_Launch_Spell();
        }
    }
}

