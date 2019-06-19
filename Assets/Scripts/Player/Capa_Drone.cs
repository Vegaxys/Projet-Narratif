using UnityEngine;
using Photon.Pun;

namespace Vegaxys
{
    public class Capa_Drone :Capa
    {
        public GameObject drone;
        
        private Companion_Drone companion;

        public override void Start() {
            base.Start();
            Invoke("InstantiateDrone", 1);
        }

        private void InstantiateDrone() {
            GameObject _companion = Instantiate(drone, 
                transform.position - (Vector3.up + Vector3.forward), 
                Quaternion.identity, 
                GameObject.Find("Companions").transform);
            Companion_Drone _drone = _companion.GetComponent<Companion_Drone>();
            drone = _companion;
            companion = _drone;
            _drone.character = character;
        }

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            Vector3 position = GameManager.instance.MousePosition(capa_Range, transform.position);
            if (capa_Target.GetComponent<Ennemi>() != null) {
                companion.LaunchDrone(Companion_Drone.CompanionState.DAMAGING, capa_Target);
            } else {
                companion.LaunchDrone(Companion_Drone.CompanionState.HEALING, capa_Target);
            }
            base.RPC_Virtual_Launch_Spell();
        }
    }
}
