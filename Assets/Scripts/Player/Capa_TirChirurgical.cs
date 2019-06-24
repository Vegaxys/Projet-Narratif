﻿using Photon.Pun;
using UnityEngine;

namespace Vegaxys {
    public class Capa_TirChirurgical :Capa
    {
        public GameObject bullet_TirChirurgical;
        public int damageCapa;

        private int targetID;


        public override void Virtual_GetTarget() {
            if (Input.GetButtonDown("Select")) {
                IEntity entity = GameManager.instance.GetEntity(capa_Range, transform.position);
                if(capa_Target != null) {
                    GameManager.instance.DeselectTarget(capa_Target);
                }
                if (entity != null) {
                    capa_Target = entity.GetTransform();
                    targetID = capa_Target.GetComponent<PhotonView>().ViewID;
                }
                return;
            }
        }

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            base.RPC_Virtual_Launch_Spell();
            Transform target = GameManager.instance.GetObjectByViewID(targetID).transform;
            GameObject bullet = Instantiate(bullet_TirChirurgical, character.canon.position, character.canon.rotation);
            bullet.GetComponent<Projectile_Capa02_Exile>().Setup(target, damageCapa, 0, 0);
        }
    }
}