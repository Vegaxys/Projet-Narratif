using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace Vegaxys
{
    public class Capa_Fufu :Capa
    {
        public float duration;
        public Material furtiv;
        public Material notFurtiv;

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            StartCoroutine(Furtiv());
        }
        private IEnumerator Furtiv() {
            character.furtiv = true;
            character.canon.GetComponent<MeshRenderer>().material = furtiv;
            float t = 0;
            while (t < duration) {
                t += Time.deltaTime;
                if (Input.GetButton("Fire")) {
                    break;
                }
                yield return null;
            }
            character.canon.GetComponent<MeshRenderer>().material = notFurtiv;
            character.furtiv = false;
            base.RPC_Virtual_Launch_Spell();
        }
    }
}
