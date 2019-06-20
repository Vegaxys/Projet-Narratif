using UnityEngine;
using Photon.Pun;

namespace Vegaxys
{
    public class Capa_Chien :Capa
    {
        public GameObject dog;
        private Companion_Dog companion;

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            if (capa_Target.GetComponent<Ennemi>() != null) {
                companion.LaunchDog(Companion_Dog.DogState.ATTACK, capa_Target);
            } else {
                companion.LaunchDog(Companion_Dog.DogState.DEFENSE, capa_Target);
            }
            base.RPC_Virtual_Launch_Spell();
        }
        private void InstantiateDog() {
            GameObject _companion = Instantiate(dog,
                transform.position + (Vector3.up + Vector3.forward),
                Quaternion.identity,
                GameObject.Find("Companions").transform);
            Companion_Dog _dog = _companion.GetComponent<Companion_Dog>();
            dog = _companion;
            companion = _dog;
            _dog.character = character;
        }
    }
}
