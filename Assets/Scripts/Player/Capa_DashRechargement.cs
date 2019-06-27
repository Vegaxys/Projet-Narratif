using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace Vegaxys
{
    public class Capa_DashRechargement :Capa
    {
        public float speedDash;

        private void Start() {
            capa_GizAOE_Range = 1f;
        }

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            StartCoroutine(Dash());
        }

        public override void Update() {
            base.Update();
            GameManager.instance.MousePositionWithoutWall(character.transform.GetChild(0), capa_Range, transform.position);
        }

        private IEnumerator Dash() {
            character.RPC_Reload();
            character.agent.enabled = false;
            Vector3 oldPos = transform.position;
            Vector3 newPos = GameManager.instance.MousePositionWithoutWall(character.transform.GetChild(0), capa_Range, transform.position);
            newPos.y = oldPos.y;
            float t = 0;
            while (t < 1) {
                character.transform.position = Vector3.Lerp(oldPos, newPos, t);
                t += Time.deltaTime * speedDash;
                yield return null;
            }
            character.agent.enabled = true;
            base.RPC_Virtual_Launch_Spell();
        }
    }
}
