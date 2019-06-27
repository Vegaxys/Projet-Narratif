using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoAttack", menuName = "Héritage", order = 1)]
public class AutoAttack : ScriptableObject
{
    public string title;
    public string description;

    [Header("Auto Attack")]
    public float fireRate;
    public int damageFire;
    public float precision;
    public float reloadingSpeed;
    public int currentBulletInWeapon;
    public int maxBulletInWeapon;
    public int maxBulletInPlayer;

    [Tooltip("corps à corps")]
    public bool cac;
    [Tooltip("Besoin de recharger l'arme ou pas")]
    public bool needReload;
    [Tooltip("Est ce que l'arme est automatique")]
    public bool automaticWeapon;
    [Tooltip("Est ce que l'arme fait des dot")]
    public bool damageOverTime;
    [Tooltip("Est ce que l'arme heal")]
    public bool healingBullet;

    public int damageDOT;
    public int row;
    public int weaponIndex;

    [Tooltip("Save currentBulletInWeapon")]
    public int save_currentBulletInWeapon;
    [Tooltip("Save maxBulletInWeapon")]
    public int save_maxBulletInWeapon;
    [Tooltip("Save maxBulletInPlayer")]
    public int save_maxBulletInPlayer;

}
