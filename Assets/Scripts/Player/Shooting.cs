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
        ApplyShootingBuff(Pickup activePickup, AttackScriptableObject usedConfig)
    {
        if (activePickup != null && activePickup.pickupType == PickupType.ShootingPickup)
        {
            
            return (
                MathF.Round(usedConfig.roundsPerMin * activePickup.shootingPickup.fireRateBuff),
                usedConfig.projectilesPerShot * activePickup.shootingPickup.projectilesPerShotBuff,
                (int)MathF.Floor(usedConfig.damage * activePickup.shootingPickup.damageBuff),
                MathF.Round(usedConfig.projectileSpeed * activePickup.shootingPickup.projectileSpeedBuff),
                usedConfig.spread * activePickup.shootingPickup.spreadChange,
                activePickup.shootingPickup.damageType,
                usedConfig.projectilePenetration + activePickup.shootingPickup.projectilePenetrationBuff,
                activePickup.shootingPickup.color
                
            );
        }

        // Palautetaan alkuperäiset arvot jos pickuppia ei ole käytössä, tai pickup ei ole Shooting Pickup
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


        if (animatorManager.thisRig.weight >= 0.99f)            //estää asetta ampumasta maahan hiirtä painaessa
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
        = ApplyShootingBuff(activePickup, usedConfig);

        float projectileLifeTime = usedConfig.projectileLifetime;
        Transform projectileSpawnPoint = gunInUse.projectileSpawnPoint;
        GameObject projectilePrefab = usedConfig.projectilePrefab;

        //käydään läpi kaikki partikkelisysteemit jos useita, ja asetetaan värit vastaamaan haluttua
        ParticleSystem[] allParticleSystems = projectilePrefab.GetComponentsInChildren<ParticleSystem>();

        foreach (var particleSystem in allParticleSystems)
        {
            var mainModule = particleSystem.main;
            mainModule.startColor = color; 
        }
        TrailRenderer projectileTrail = projectilePrefab.GetComponentInChildren<TrailRenderer>();
        if(projectileTrail != null)
        {
            projectileTrail.colorGradient = color;
        }
        

        if (Time.time > (60 / roundsPerMin) + lastShootTime)
        {
            lastShootTime = Time.time;


            for (int i = 0; i < projectilesPerShot; i++)
            {

                // Luodaan projektiili käyttämällä bulletSpread suuntaa
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, 
                    projectileSpawnPoint.rotation * Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.up));

                // Asetetaan projektiilin nopeus ja vahinko
                ProjectileMove projectileMove = projectile.GetComponent<ProjectileMove>();
                SetProjectileValues(projectileMove, projectileSpeed, damage, penetration, projectileLifeTime, type);

            }
        }
    }

}
