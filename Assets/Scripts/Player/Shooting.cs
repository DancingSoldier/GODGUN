using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooting : MonoBehaviour
{


    
    PlayerManager playerManager;

    AnimatorManager animatorManager;
    
    public GunScriptableObject gunInUse;

    public Pickup pickupInUse;



    public bool readyToShoot;
    public bool shooting;
    public bool usedLeftAttack;


    

    private float lastShootTime;
    

    private void SetPlayerWeapon()
    {
        gunInUse = playerManager.gunBeingUsed;
    }

    private (float roundsPerMin, int projectilesPerShot, int damage, float projectileSpeed,
        float spread, DamageTypes type, int penetration, Gradient color)
        ApplyBuff(Pickup activePickup, AttackScriptableObject usedConfig)
    {
        if (activePickup != null)
        {
            
            return (
                MathF.Round(usedConfig.roundsPerMin * activePickup.pickupConfig.fireRateBuff),
                usedConfig.projectilesPerShot * activePickup.pickupConfig.projectilesPerShotBuff,
                (int)MathF.Floor(usedConfig.damage * activePickup.pickupConfig.damageBuff),
                MathF.Round(usedConfig.projectileSpeed * activePickup.pickupConfig.projectileSpeedBuff),
                usedConfig.spread * activePickup.pickupConfig.spreadChange,
                activePickup.pickupConfig.damageType,
                usedConfig.projectilePenetration + activePickup.pickupConfig.projectilePenetrationBuff,
                activePickup.pickupConfig.color
                
            );
        }

        // Palautetaan alkuper‰iset arvot, jos activePickup on null
        return (
            usedConfig.roundsPerMin,
            usedConfig.projectilesPerShot,
            usedConfig.damage,
            usedConfig.projectileSpeed,
            usedConfig.spread,
            usedConfig.damageType,
            usedConfig.projectilePenetration,
            usedConfig.projectileColor
        );
    }

    void Start()
    {

        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();

        readyToShoot = true;
        SetPlayerWeapon();
    }

    public void HandleShooting()
    {


        if (animatorManager.thisRig.weight >= 0.99f)            //est‰‰ asetta ampumasta maahan hiirt‰ painaessa
        {
            usedLeftAttack = gunInUse.usedMainAttack;
            if (readyToShoot && shooting && usedLeftAttack)
            {
                AttackScriptableObject usedConfig = gunInUse.mainAttackConfig;

                Shoot(usedConfig, pickupInUse);


            }
            else if (readyToShoot && shooting && !usedLeftAttack)
            {
                AttackScriptableObject usedConfig = gunInUse.altAttackConfig;

                Shoot(usedConfig, pickupInUse);
            }
        }


    }

    private void SetProjectileValues(ProjectileMove projectile, float projectileSpeed, int damage,
        int penetration, float projectileLifeTime, DamageTypes type)
    {

        if (projectile != null)
        {
            projectile.projectileSpeed = projectileSpeed;
            projectile.damage = damage;
            projectile.penetration = penetration;
            projectile.projectileLifeTime = projectileLifeTime;
            projectile.damageType = type;
        }
    }
    private void Shoot(AttackScriptableObject usedConfig, Pickup activePickup)
    {
        var (roundsPerMin, projectilesPerShot, damage, projectileSpeed, spread, type, penetration, color) 
        = ApplyBuff(activePickup, usedConfig);

        float projectileLifeTime = usedConfig.projectileLifetime;
        Transform projectileSpawnPoint = gunInUse.projectileSpawnPoint;
        GameObject projectilePrefab = usedConfig.projectilePrefab;

        //k‰yd‰‰n l‰pi kaikki partikkelisysteemit jos useita, ja asetetaan v‰rit vastaamaan haluttua
        ParticleSystem[] allParticleSystems = projectilePrefab.GetComponentsInChildren<ParticleSystem>();

        foreach (var particleSystem in allParticleSystems)
        {
            var mainModule = particleSystem.main;
            mainModule.startColor = color; 
        }
        TrailRenderer projectileTrail = projectilePrefab.GetComponentInChildren<TrailRenderer>();
        projectileTrail.colorGradient = color;

        if (Time.time > (60 / roundsPerMin) + lastShootTime)
        {
            lastShootTime = Time.time;


            for (int i = 0; i < projectilesPerShot; i++)
            {

                // Luodaan projektiili k‰ytt‰m‰ll‰ bulletSpread suuntaa
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, 
                    projectileSpawnPoint.rotation * Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.up));

                // Asetetaan projektiilin nopeus ja vahinko
                ProjectileMove projectileMove = projectile.GetComponent<ProjectileMove>();
                SetProjectileValues(projectileMove, projectileSpeed, damage, penetration, projectileLifeTime, type);

            }
        }
    }

}
