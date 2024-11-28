using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup", menuName = "Pickup/Pickup Configuration", order = 6)]
public class PickupScriptableObject : ScriptableObject
{



    [Header("Pickup Properties")]
    public string pickupName;
    public string pickupDescription;
    public Color pickupTextColor;

    [Header("Time Floats")]
    public float cooldown;
    public float duration;

    [Header("Pickup Visuals")]
    public GameObject pickupModelPrefab;
    public Material material;
    public GameObject pickupEffect;

    [Header("Projectile Effects")]
    public int damageBuff;
    public float fireRateBuff;
    public float spreadChange;
    public int projectilesPerShotBuff;
    public float projectileSpeedBuff;
    public int projectilePenetrationBuff;
    [Header("Projectile Visuals")]
    public DamageTypes damageType;
    public Gradient color;
    

    [Header("Character Effects")]
    public int invulnerabilityDurationBuff;
    public int enemySlowDownBuff;
    

    [Header("Pickup Spawn")]
    public Transform spawnLocation;
    public float spawnTime;


}
