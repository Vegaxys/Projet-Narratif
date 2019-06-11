using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Exile :BaseCharacter{

    [Header("Switch Arme (capa 01)")]
    [SerializeField] private string bulletNameArc;
    [SerializeField] private string bulletNameSniper;
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
        switch (weaponType) {
            case WeaponType.ARC:
                _char.bulletName = bulletNameArc;
                _char.AA_range = rangeArc;
                _char.fireRate = fireRateArc;
                _char.damageFire = damageArc;
                break;
            case WeaponType.SNIPER:
                _char.bulletName = bulletNameSniper;
                _char.AA_range = rangeSniper;
                _char.fireRate = fireRateSniper;
                _char.damageFire = damageSniper;
                break;
        }
        yield return base.Capa_01();
    }
    public override IEnumerator Capa_02() {
        //loading capa
        capa02Activated = true;
        yield return new WaitForSeconds(_char.capa_02_Loading);
        Transform target = SelectionManager.selection.selectionTransform;
        GameObject bullet = GameManager_Dungeon.dungeon.GetBullet("Exile_capa02", transform.position, Quaternion.identity);
        bullet.GetComponent<Projectile_Capa02_Exile>().Setup(target.position, 10000, damageCapa02);
        capa02Activated = false;
        yield return base.Capa_02();
    }
}
