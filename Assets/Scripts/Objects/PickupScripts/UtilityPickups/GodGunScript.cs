using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GodGunScript : MonoBehaviour
{
    
    public LayerMask groundMask;
    PlayerManager player;
    Shooting shooting;
    public GameObject gunHolder;
    public GameObject projectileSpawnPoint;
    public AttackScriptableObject usedConfig;
    private float fireRate; // Ampumisen perusnopeus
    private float lastShootTime = 0f;
    private float fireRateIncreasePerFrame = 20; // Nopeuden kasvutaso (frames)
    private float currentFireRate;

    Transform animatableObject;

    private Vector3 initialPosition;
    private Quaternion initialRotation;



    void CameraTargeting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            gunHolder.transform.LookAt(hitInfo.point);

            Vector3 euler = gunHolder.transform.eulerAngles;

            // Rajoita pystykulma (esim. X-akseli)
            euler.z = Mathf.Clamp(euler.y, -360f, 360f); // Lukitsee pyörimisen




            // Päivitä rajoitetut kulmat takaisin
            gunHolder.transform.eulerAngles = euler;
        }

    }


    private void InstantiateProjectile(AttackScriptableObject usedConfig)
    {
        
        // Kasvata tulinopeutta joka framella
        fireRate += fireRateIncreasePerFrame * Time.deltaTime;

        // Suojaa tulinopeus negatiivisilta arvoilta
        fireRate = Mathf.Max(0, fireRate);

        currentFireRate = 60 / Mathf.Max(1, (usedConfig.roundsPerMin + fireRate));
        Debug.Log("FireRate: " + fireRate);
        // Tarkista, onko aika ampua
        if (Time.time > currentFireRate + lastShootTime)
        {
            lastShootTime = Time.time;

            // Luo projektiilit
            for (int i = 0; i < usedConfig.projectilesPerShot; i++)
            {
                GameObject godGunProjectile = Instantiate(usedConfig.projectilePrefab,
                    projectileSpawnPoint.transform.position,
                    projectileSpawnPoint.transform.rotation *
                    Quaternion.AngleAxis(Random.Range(-usedConfig.spread, usedConfig.spread), Vector3.up));

                ProjectileMove projectileMove = godGunProjectile.GetComponent<ProjectileMove>();
                shooting.SetProjectileValues(projectileMove, usedConfig.projectileSpeed,
                    usedConfig.damage,
                    usedConfig.projectilePenetration,
                    false, 0f,
                    usedConfig.projectileLifetime,
                    usedConfig.damageType);
            }
        }
    }


    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        shooting = player.GetComponent<Shooting>();

        animatableObject = gameObject.transform.GetChild(0).transform.GetChild(0);
        projectileSpawnPoint = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject;
        initialPosition = animatableObject.localPosition;
        initialRotation = animatableObject.localRotation;
    }

    private void RestoreOriginalVectors()
    {
        animatableObject.localPosition = initialPosition;
        animatableObject.localRotation = initialRotation;
    }

    private void WeaponAnimationMiniGunSpin(float turnSpeed, bool online)
    {

        if (online)
        {
            animatableObject.Rotate(0, turnSpeed, 0);
        }
        else
        {
            RestoreOriginalVectors();
        }
    }



    void ShootingGodGun()
    {
        if(shooting.shooting)
        {
            InstantiateProjectile(usedConfig);
        }
    }

    private void Update()
    {
       

        CameraTargeting();
        ShootingGodGun();
        WeaponAnimationMiniGunSpin(fireRate / 15, shooting.shooting);
        
        
    }
}
