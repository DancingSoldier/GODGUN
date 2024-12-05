using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Pickup", menuName = "Pickup/Utility Pickup", order = 7)]
public class UtilityPickupScriptableObject : ScriptableObject
{

    [Header("Pickup Properties")]
    public string pickupName;
    public string pickupDescription;
    public Color pickupTextColor = Color.white;
    public int killsRequired;

    [Header("Time Floats")]
    public float cooldown = 30f;
    public float duration = 10f;

    [Header("Pickup Audio Visuals")]
    public GameObject pickupModelPrefab;
    public Material material;
    public GameObject pickupEffect;
    [Header("Pickup Audio Effects")]
    public AudioClip pickedupSoundEffect;
    public AudioClip spacialSoundEffect;
    public float pickedupSoundpitch = 0.5f;
    public float pickedupSoundvolume = 0.5f;
    public float spacialSoundpitch = 0.5f;
    public float spacialSoundvolume = 0.5f;

    [Header("Pickup Effect")]
    public GameObject effectPrefab;
    public float effectFloat1 = 10;
    public float effectFloat2 = 10;

    public void SetSpacialSoundSettings(AudioSource audiosource)
    {
        audiosource.clip = spacialSoundEffect;
        audiosource.pitch = spacialSoundpitch;
        audiosource.volume = spacialSoundvolume;
        audiosource.loop = true;
    }

}
