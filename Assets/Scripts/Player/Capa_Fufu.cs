using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace Vegaxys
{
    public class Capa_Fufu :Capa
    {
        public float duration;

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
          //  StartCoroutine(Furtiv());
        }

     /*   private IEnumerator Furtiv() {
            character.GetComponent<CapsuleCollider>().enabled = false;
        }*/
    }
}
