using UnityEngine;
using Photon.Pun;

namespace Vegaxys
{
    public class Character_Omega :BaseCharacter
    {
        [Header("Lethal")]
        [SerializeField] private GameObject bullet_Lethal;
        [SerializeField] private float fireRate_Lethal;
        [SerializeField] private float precision_Lethal;
        [SerializeField] private int damage_Lethal;

        [Header("Soin")]
        [SerializeField] private GameObject bullet_Soin;
        [SerializeField] private float fireRate_Soin;
        [SerializeField] private float precision_Soin;
        [SerializeField] private int damage_Soin;

        [Header("Drone (capa 02)")]
        [SerializeField] private GameObject drone;

        public enum WeaponType
        {
            LETHAL,
            SOIN
        }
        public WeaponType weaponType;

        public override void Start() {
            Capa01_SwitchValue();
            base.Start();
        }

        [PunRPC]
        private void RPC_Capa01_SwitchWeapon() {
            if (weaponType == WeaponType.LETHAL) {
                weaponType = WeaponType.SOIN;
                Capa01_SwitchValue();
                return;
            } else
            if (weaponType == WeaponType.SOIN) {
                weaponType = WeaponType.LETHAL;
                Capa01_SwitchValue();
                return;
            }
        }

        [PunRPC]
        private void RPC_Capa02_Drone(int targetID, int _damage) {

        }

        private void Capa01_SwitchValue() {
            switch (weaponType) {
                case WeaponType.LETHAL:
                    fireBullet = bullet_Lethal;
                    fireRate = fireRate_Lethal;
                    damageFire = damage_Lethal;
                    precision = precision_Lethal;
                    break;
                case WeaponType.SOIN:
                    fireBullet = bullet_Soin;
                    fireRate = fireRate_Soin;
                    damageFire = -damage_Soin;
                    precision = precision_Soin;
                    break;
            }
        }
    }
}
