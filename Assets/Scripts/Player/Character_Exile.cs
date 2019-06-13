using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Vegaxys
{
    public class Character_Exile :BaseCharacter
    {
        [Header("Sniper")]
        [SerializeField] private GameObject bulletSniper;
        [SerializeField] private float rangeSniper;
        [SerializeField] private float fireRateSniper;
        [SerializeField] private int damageSniper;

        [Header("Arc")]
        [SerializeField] private GameObject bulletArc;
        [SerializeField] private float rangeArc;
        [SerializeField] private float fireRateArc;
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
            base.Start();
            Capa01_SwitchValue();
        }
        [PunRPC]
        public override void RPC_Capa01() {
            base.RPC_Capa01();
            Capa01_SwitchWeapon();
        }

        private void Capa01_SwitchWeapon() {
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
        private void Capa01_SwitchValue() {
            switch (weaponType) {
                case WeaponType.ARC:
                    fireBullet = bulletArc;
                    AA_range = rangeArc;
                    fireRate = fireRateArc;
                    damageFire = damageArc;
                    break;
                case WeaponType.SNIPER:
                    fireBullet = bulletSniper;
                    AA_range = rangeSniper;
                    fireRate = fireRateSniper;
                    damageFire = damageSniper;
                    break;
            }
        }
    }
}
/*
    [Header("Switch Arme (capa 01)")]
    [SerializeField] private GameObject bulletArc;
    [SerializeField] private GameObject bulletSniper;
    [SerializeField] private float rangeArc, rangeSniper;
    [SerializeField] private float fireRateArc, fireRateSniper;
    [SerializeField] private int damageArc, damageSniper;

    [Header("Tir (capa 02)")]
    [SerializeField] private int damageCapa02;
    [SerializeField] private bool capa02Activated;

    public enum WeaponType
    {
        ARC,
        SNIPER
    }
    public WeaponType weaponType;

    private void Start() {
        fireBullet = bulletSniper;
        _char.AA_range = rangeSniper;
        _char.fireRate = fireRateSniper;
        _char.damageFire = damageSniper;
    }
    public override void Update() {
        if (!capa02Activated) {
            base.Update();
        }
    }
    public override IEnumerator Capa_01() {
        if(weaponType == WeaponType.ARC) {
            weaponType = WeaponType.SNIPER;
        } else {
            weaponType = WeaponType.ARC;
        }
        SwitchWeapon();
        yield return base.Capa_01();
    }
    public void SwitchWeapon() {
        switch (weaponType) {
            case WeaponType.ARC:
                fireBullet = bulletArc;
                _char.AA_range = rangeArc;
                _char.fireRate = fireRateArc;
                _char.damageFire = damageArc;
                break;
            case WeaponType.SNIPER:
                fireBullet = bulletSniper;
                _char.AA_range = rangeSniper;
                _char.fireRate = fireRateSniper;
                _char.damageFire = damageSniper;
                break;
        }
    }

    public override IEnumerator Capa_02() {
        //loading capa
        if (SelectionManager.selection.selectionTransform != null) {
            capa02Activated = true;
            yield return new WaitForSeconds(_char.capa_02_Loading);
            Transform target = SelectionManager.selection.selectionTransform;
            GameObject bullet = PhotonNetwork.Instantiate("Capa02_Exile", Vector3.zero, Quaternion.identity, 0);
            bullet.transform.position = canon.position;
            bullet.transform.rotation = canon.rotation;
            bullet.GetComponent<Projectile_Capa02_Exile>().Setup(target.position, 10000, damageCapa02);
            capa02Activated = false;
            yield return base.Capa_02();
        }
        yield return null;
    }
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        base.OnPhotonSerializeView(stream, info);
        if (stream.IsWriting) {
            stream.SendNext(weaponType);
        } else if (stream.IsReading) {
            weaponType = (WeaponType)stream.ReceiveNext();
        }
    }
}*/
