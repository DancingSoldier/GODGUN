using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack", menuName = "Guns/Attack Configuration", order = 3)]
public class AttackScriptableObject : ScriptableObject
{


    [Header("Projectile Properties")]
    public int damage = 5;
    public int projectilesPerShot = 1;
    public int projectilePenetration = 1;
    public float projectileSpeed = 60;
    public float roundsPerMin = 100;
    public DamageTypes damageType = DamageTypes.Normal;
    public float spread = 1f;
    public bool hasKnockback = true;

    [Header("Animation Properties")]
    public GameObject projectilePrefab;
    public AudioSource projectileShooter;
    public float projectileLifetime = 2;
    public Gradient projectileColor;

    [Header("Audio")]
    public AttackAudioScriptableObject audioConfig;
    
    [Header("Player Effects")]
    public float playerMovementEffect = 1;



}
