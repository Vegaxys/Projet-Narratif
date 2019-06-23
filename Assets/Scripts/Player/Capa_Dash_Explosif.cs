using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace Vegaxys {
    public class Capa_Dash_Explosif :Capa
    {
        public float speedDash;

        public int capa_Damage;

        public override void Start() {
            base.Start();
            capa_GizAOE_Range = 3f;
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
            DealDamage();
            base.RPC_Virtual_Launch_Spell();
        }
        private void DealDamage() {
            Collider[] colliders = Physics.OverlapSphere(transform.position, capa_GizAOE_Range);
            for (int i = 0; i < colliders.Length; i++) {
                int damage = GameManager.instance.GetRandomDamage(capa_Damage);
                switch (colliders[i].tag) {
                    case "Ennemi":
                        colliders[i].GetComponentInParent<Ennemi>().TakeDamage(damage);
                        break;
                    case "Player":
                        if (colliders[i].GetComponent<BaseCharacter>() != character)
                            colliders[i].GetComponent<BaseCharacter>().Virtual_TakeDamage(damage);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

