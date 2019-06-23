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

        [PunRPC]
        private void RPC_SwitchValue(WeaponType type) {
            weaponType = type;
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
            //SwitchValue();
        }

        [PunRPC]
        public override void RPC_Virtual_Launch_Spell() {
            base.RPC_Virtual_Launch_Spell();
            character.UploadAutoAttack();
            if (weaponType == WeaponType.WEAPON_ONE) {
                weaponType = WeaponType.WEAPON_TWO;
                view.RPC("RPC_SwitchValue", RpcTarget.AllBuffered, weaponType);
                // SwitchValue();
            } else
                if (weaponType == WeaponType.WEAPON_TWO) {
                weaponType = WeaponType.WEAPON_ONE;
                view.RPC("RPC_SwitchValue", RpcTarget.AllBuffered, weaponType);
                // SwitchValue();
            }
            character.UpdateAutoAttack();
            if (photonView.IsMine) {
                HUD_Manager.manager.Update_Chargeur(
                    character.currentBulletInWeapon,
                    character.maxBulletInWeapon,
                    character.maxBulletInPlayer);
            }
        }
    }
}
