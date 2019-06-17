using UnityEngine;
using Photon.Pun;

namespace Vegaxys
{
    public class Character_Exile :BaseCharacter, IPunObservable
    {
        [Header("Sniper")]
        [SerializeField] private GameObject bulletSniper;
        [SerializeField] private float fireRateSniper;
        [SerializeField] private float precision_Sniper;
        [SerializeField] private int damageSniper;

        [Header("Arc")]
        [SerializeField] private GameObject bulletArc;
        [SerializeField] private float fireRateArc;
        [SerializeField] private float precision_Arc;
        [SerializeField] private int damageArc;

        [Header("Tir (capa 02)")]
        [SerializeField] private GameObject bulleCapa02;
        [SerializeField] private int damageCapa02;
        [SerializeField] private bool capa02Activated;

        public enum WeaponType
        {
            ARC,
            SNIPER
        }
        public WeaponType weaponType;

        public override void Start() {
            Capa01_SwitchValue();
            base.Start();
        }

        public override void Virtual_Character_Capa01() {
            base.Virtual_Character_Capa01();
            view.RPC("RPC_Capa01_SwitchWeapon", RpcTarget.AllBuffered);
            StartCoroutine(RecoverCapa01());
        }

        public override void Virtual_Character_Capa02() {
            base.Virtual_Character_Capa02();
            int targetID = targets_Capa[0].GetComponent<PhotonView>().ViewID;
            int _damage = GameManager.instance.GetRandomDamage(damageCapa02);
            view.RPC("RPC_Capa02_TirCaitlyn", RpcTarget.AllBuffered, targetID, _damage);
            StartCoroutine(RecoverCapa02());
        }

        [PunRPC]
        private void RPC_Capa01_SwitchWeapon() {
            if (weaponType == WeaponType.SNIPER) {
                weaponType = WeaponType.ARC;
                Capa01_SwitchValue();
                return;
            } else
            if (weaponType == WeaponType.ARC) {
                weaponType = WeaponType.SNIPER;
                Capa01_SwitchValue();
                return;
            }
        }

        [PunRPC]
        private void RPC_Capa02_TirCaitlyn(int targetID, int _damage) {
            GameObject target = GameManager.instance.GetObjectByViewID(targetID);
            GameObject bullet = Instantiate(bulleCapa02, canon.position, canon.rotation);
            bullet.GetComponent<Projectile_Capa02_Exile>().Setup(target.transform, _damage);
            base.Virtual_DeselectAllTargets();
        }

        private void Capa01_SwitchValue() {
            switch (weaponType) {
                case WeaponType.ARC:
                    fireBullet = bulletArc;
                    fireRate = fireRateArc;
                    damageFire = damageArc;
                    precision = precision_Arc;
                    break;
                case WeaponType.SNIPER:
                    fireBullet = bulletSniper;
                    fireRate = fireRateSniper;
                    damageFire = damageSniper;
                    precision = precision_Sniper;
                    break;
            }
        }
    }
}
