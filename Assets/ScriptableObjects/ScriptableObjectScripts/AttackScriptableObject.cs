using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack", menuName = "Guns/Attack Configuration", order = 3)]
public class AttackScriptableObject : ScriptableObject
{


    [Header("Projectile Properties")]
    public int damage;
    public int projectilesPerShot;
    public int projectilePenetration;
    public float projectileSpeed;
    public float roundsPerMin;
    public DamageTypes damageType;
    public float spread;

    [Header("Animation Properties")]
    public GameObject projectilePrefab;
    public float projectileLifetime;
    public Gradient projectileColor;

    [Header("Player Effects")]
    public float playerMovementEffect = 1;



}
