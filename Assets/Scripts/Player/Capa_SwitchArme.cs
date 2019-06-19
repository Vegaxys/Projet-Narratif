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
        [SerializeField] private float fireRate_WeaponOne;
        [SerializeField] private float precision_WeaponOne;
        [SerializeField] private int damage_WeaponOne;

        [Header("Weapon Two")]
        [SerializeField] private GameObject bullet_WeaponTwo;
        [SerializeField] private float fireRate_WeaponTwo;
        [SerializeField] private float precision_WeaponTwo;
        [SerializeField] private int damage_WeaponTwo;
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
                    character.fireRate = fireRate_WeaponOne;
                    character.precision = precision_WeaponOne;
                    character.damageFire = damage_WeaponOne;
                    break;
                case WeaponType.WEAPON_TWO:
                    character.fireBullet = bullet_WeaponTwo;
                    character.fireRate = fireRate_WeaponTwo;
                    character.precision = precision_WeaponTwo;
                    character.damageFire = damage_WeaponTwo;
                    break;
            }
        }

        public override void Start() {
            base.Start();
            SwitchValue();
        }

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            base.RPC_Virtual_Launch_Spell();
            if (weaponType == WeaponType.WEAPON_ONE) {
                weaponType = WeaponType.WEAPON_TWO;
                SwitchValue();
                return;
            } else
                if (weaponType == WeaponType.WEAPON_TWO) {
                weaponType = WeaponType.WEAPON_ONE;
                SwitchValue();
                return;
            }
        }
    }
}
