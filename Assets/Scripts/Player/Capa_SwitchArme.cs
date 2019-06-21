using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Vegaxys
{
    public class Capa_SwitchArme :Capa
    {
        [Header("Weapon One")]
        [SerializeField] private GameObject bullet_WeaponOne;
        [SerializeField] private AutoAttack weaponOne;

        [Header("Weapon Two")]
        [SerializeField] private GameObject bullet_WeaponTwo;
        [SerializeField] private AutoAttack weaponTwo;

        public enum WeaponType
        {
            WEAPON_ONE,
            WEAPON_TWO
        }
        public WeaponType weaponType;

        private void SwitchValue() {
            switch (weaponType) {
                case WeaponType.WEAPON_ONE:
                    character.fireBullet = bullet_WeaponOne;
                    character.currentAttack = weaponOne;
                    break;
                case WeaponType.WEAPON_TWO:
                    character.fireBullet = bullet_WeaponTwo;
                    character.currentAttack = weaponTwo;
                    break;
            }
        }

        private void Awake() {
            character = GetComponentInParent<BaseCharacter>();
            weaponType = WeaponType.WEAPON_ONE;
            SwitchValue();
        }

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            base.RPC_Virtual_Launch_Spell();
            if (weaponType == WeaponType.WEAPON_ONE) {
                weaponType = WeaponType.WEAPON_TWO;
                SwitchValue();
            } else
                if (weaponType == WeaponType.WEAPON_TWO) {
                weaponType = WeaponType.WEAPON_ONE;
                SwitchValue();
            }
            character.UpdateAutoAttack();
        }
    }
}
