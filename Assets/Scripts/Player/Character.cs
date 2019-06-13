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
    public float capa_01_Loading;
    public bool capa_01_Loaded;

    [Header("Capacité 2")]
    public string capa_02_Name;
    public string capa_02_Description;
    public int capa_02_Cooldown;
    public float capa_02_Loading;
    public bool capa_02_Loaded;
}

