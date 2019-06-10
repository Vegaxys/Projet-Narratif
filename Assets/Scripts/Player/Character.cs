using UnityEngine;

[CreateAssetMenu(fileName = "Character - ", menuName = "Heritage/Characters", order = 0)]
public class Character :ScriptableObject{

    public string avatarName;
    public float maxSpeed;
    public float acceleration;
    public int aggroValue;

    [Header("Tir")]
    public int damageFire;
    public int maxBulletInWeapon;
    public int maxBullet;
    public int currentBullet;
    public float fireRate;
    public float AA_range;
    public string bulletName;

    [Header("Life")]
    public int maxLife;
    public int currentLife;

    [Header("Shield")]
    public int maxShield;
    public int currentShield;

    [Header("Tir basique")]
    public string tir_Name;
    public string tir_Description;

    [Header("Capacité 1")]
    public string capa_01_Name;
    public string capa_01_Description;
    public int capa_01_Cooldown;
}
