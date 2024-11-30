using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup", menuName = "Pickup/Shooting Pickup", order = 6)]
public class ShootingPickupScriptableObject : ScriptableObject
{



    [Header("Pickup Properties")]
    public string pickupName;
    public string pickupDescription;
    public Color pickupTextColor = Color.white;


    [Header("Time Floats")]
    public float cooldown = 30f;
    public float duration = 10f;

    [Header("Pickup Visual Effects")]
    public GameObject pickupModelPrefab;
    public Material material;
    public GameObject pickupEffect;
    [Header("Pickup Audio Effects")]
    public AudioClip pickedupSoundEffect;
    public AudioClip spacialSoundEffect;
    public float pickedupSoundpitch = 0.5f;
    public float pickedupSoundvolume =0.5f;
    public float spacialSoundpitch = 0.5f;
    public float spacialSoundvolume = 0.5f;


    [Header("Shooting Pickup Effects")]
    public int damageBuff = 1;
    public float fireRateBuff = 1f;
    public float spreadChange = 1f;
    public int projectilesPerShotBuff = 1;
    public float projectileSpeedBuff = 1f;
    public int projectilePenetrationBuff = 1;
    public bool hasKnockback = true;
    public float knockbackMultiplier = .5f;
    public DamageTypes damageType = DamageTypes.Normal;
    public Gradient color;


    public void SetSpacialSoundSettings(AudioSource audiosource)
    {
        audiosource.clip = spacialSoundEffect;
        audiosource.pitch = spacialSoundpitch;
        audiosource.volume = spacialSoundvolume;
        audiosource.loop = true;
    }



}
