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
    public AttackAudioScriptableObject attackAudio;
    public AudioSource shootingAudioSource;
    public Pickup pickupInUse;
    float originalKnockbackMultiplier = 0.5f;
    public float knockbackMultiplier;

    public bool readyToShoot;
    public bool shooting;
    public bool usedLeftAttack;


    

    private float lastShootTime;
    

    private void SetPlayerWeapon()
    {
        gunInUse = playerManager.gunBeingUsed;
    }

    private (float roundsPerMin, int projectilesPerShot, int damage, float projectileSpeed,
        float spread, DamageTypes type, int penetration, bool hasKnockback, Gradient color)
        ApplyShootingBuff(Pickup activePickup, AttackScriptableObject usedConfig)
    {
        if (activePickup != null && activePickup.pickupType == PickupType.ShootingPickup)
        {
            knockbackMultiplier = activePickup.shootingPickup.knockbackMultiplier + originalKnockbackMultiplier;
            
            return (
                MathF.Round(usedConfig.roundsPerMin * activePickup.shootingPickup.fireRateBuff),
                usedConfig.projectilesPerShot * activePickup.shootingPickup.projectilesPerShotBuff,
                (int)MathF.Floor(usedConfig.damage * activePickup.shootingPickup.damageBuff),
                MathF.Round(usedConfig.projectileSpeed * activePickup.shootingPickup.projectileSpeedBuff),
                usedConfig.spread * activePickup.shootingPickup.spreadChange,
                activePickup.shootingPickup.damageType,
                usedConfig.projectilePenetration + activePickup.shootingPickup.projectilePenetrationBuff,
                activePickup.shootingPickup.hasKnockback,
                activePickup.shootingPickup.color
                
            );
            
        }
        // Palautetaan alkuperäiset arvot jos pickuppia ei ole käytössä, tai pickup ei ole Shooting Pickup
        knockbackMultiplier = originalKnockbackMultiplier;
        
        return (
            usedConfig.roundsPerMin,
            usedConfig.projectilesPerShot,
            usedConfig.damage,
            usedConfig.projectileSpeed,
            usedConfig.spread,
            usedConfig.damageType,
            usedConfig.projectilePenetration,
            usedConfig.hasKnockback,
            usedConfig.projectileColor
        );

    }

    
    void Start()
    {

        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        shootingAudioSource = audioSources[0];

        


        readyToShoot = true;
        SetPlayerWeapon();
    }

    public void HandleShooting()
    {


        if (animatorManager.thisRig.weight >= 0.3f)            //estää asetta ampumasta maahan hiirtä painaessa
        {
            usedLeftAttack = gunInUse.usedMainAttack;
            if (readyToShoot && shooting && usedLeftAttack)
            {
                AttackScriptableObject usedConfig = gunInUse.mainAttackConfig;
                attackAudio = gunInUse.mainAttackAudio;
                Shoot(usedConfig, pickupInUse);


            }
            else if (readyToShoot && shooting && !usedLeftAttack)
            {
                AttackScriptableObject usedConfig = gunInUse.altAttackConfig;
                attackAudio = gunInUse.altAttackAudio;
                Shoot(usedConfig, pickupInUse);
            }
        }


    }

    public void SetProjectileValues(ProjectileMove projectile, float projectileSpeed, int damage,
        int penetration, bool hasKnockback, float knockBackMultiplier, float projectileLifeTime, DamageTypes type)
    {

        if (projectile != null)
        {
            projectile.projectileSpeed = projectileSpeed;
            projectile.damage = damage;
            projectile.penetration = penetration;
            projectile.projectileLifeTime = projectileLifeTime;
            projectile.damageType = type;
            projectile.hasKnockback = hasKnockback;
            projectile.knockbackMultiplier = knockBackMultiplier;
        }
    }
    public void Shoot(AttackScriptableObject usedConfig, Pickup activePickup)
    {
        var (roundsPerMin, projectilesPerShot, damage, projectileSpeed, spread, type, penetration, hasKnockback, color) 
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
            if(attackAudio != null)
            {
                
                attackAudio.PlayAudio(shootingAudioSource);

            }
            
            
            for (int i = 0; i < projectilesPerShot; i++)
            {
                
                // Luodaan projektiili käyttämällä bulletSpread suuntaa
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, 
                    projectileSpawnPoint.rotation * Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.up));

                // Asetetaan projektiilin nopeus ja vahinko
                ProjectileMove projectileMove = projectile.GetComponent<ProjectileMove>();
                SetProjectileValues(projectileMove, projectileSpeed, damage, penetration, hasKnockback, knockbackMultiplier, projectileLifeTime, type);

            }
        }
    }

}
