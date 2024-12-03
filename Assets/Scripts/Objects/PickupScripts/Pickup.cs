using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public ShootingPickupScriptableObject shootingPickup;
    public UtilityPickupScriptableObject utilityPickup;
    
    public GameObject pickupEffectPrefab;
    public GameObject pickupModelPrefab;
    public PickupType pickupType;

    GameObject instantiatedEffect;
    GameObject instantiatedModel;

    
    ParticleSystem pickupEffect;



    CapsuleCollider pickupTrigger;
    Material pickupMaterial;
    SkinnedMeshRenderer modelRenderer;
    public AudioSource pickedupSound;
    public AudioSource spacialAudio;
    public AudioClip pickupSound;
    public AudioClip spacialSoundEffect;
    float pickedupVolume;
    float pickedupPitch;
    float spacialPitch;
    float spacialVolume;
    //yhteiset pickupin tiedot
    public float pickupDuration;
    public float pickupCooldown;
    public string pickupName;
    public Color pickupTextColor;
    
    //lis‰‰ myˆhemmin toiminnallisuus efektille joka humisee pelaajan ollessa buffattuna
    

    // Start is called before the first frame update
    void Start()
    {
        if (shootingPickup == null && utilityPickup == null)
        {
            Debug.LogError("No pickup scriptable object");
            return;
        }

        SetPickupValues();

        // M‰‰ritet‰‰n pickup-tyyppi

        
        
        PickupSpawn();
        pickupTrigger = GetComponentInChildren<CapsuleCollider>();
        pickupTrigger.radius = 2.5f;
        if(instantiatedModel.GetComponent<SkinnedMeshRenderer>() != null)
        {
            modelRenderer = instantiatedModel.GetComponent<SkinnedMeshRenderer>();
        }

        if(modelRenderer.material != null)
        {
            modelRenderer.material = pickupMaterial;
        }

        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        if (audioSources.Length == 0)
        {
            Debug.LogWarning($"No Audio Sources on Pickup {pickupName}");
        }
        pickedupSound = audioSources[0];
        spacialAudio = audioSources[1];
        if (spacialAudio != null && spacialSoundEffect != null)
        {
            if(shootingPickup != null)
            {
                shootingPickup.SetSpacialSoundSettings(spacialAudio);
            }
            else
            {
                utilityPickup.SetSpacialSoundSettings(spacialAudio);
            }

            if (spacialAudio.clip != null)
            {
                spacialAudio.Play();
            }
            else
            {
                Debug.LogWarning($"No spacialSoundEffect assigned for {pickupName}");
            }
        }
    }
    void SetPickupValues()
    {
        if (shootingPickup != null)
        {
            pickupType = PickupType.ShootingPickup;
            pickupEffectPrefab = shootingPickup.pickupEffect;
            pickupModelPrefab = shootingPickup.pickupModelPrefab;
            pickupMaterial = shootingPickup.material;
            pickupDuration = shootingPickup.duration;
            pickupCooldown = shootingPickup.cooldown;
            pickupName = shootingPickup.pickupName;
            pickupTextColor = shootingPickup.pickupTextColor;
            pickupSound = shootingPickup.pickedupSoundEffect;
            spacialSoundEffect = shootingPickup.spacialSoundEffect;
            pickedupVolume = shootingPickup.pickedupSoundvolume;
            pickedupPitch = shootingPickup.pickedupSoundpitch;
            spacialVolume = shootingPickup.spacialSoundvolume;
            spacialPitch = shootingPickup.spacialSoundpitch;
        }
        else if (utilityPickup != null)
        {
            pickupType = PickupType.UtilityPickup;
            pickupEffectPrefab = utilityPickup.pickupEffect;
            pickupModelPrefab = utilityPickup.pickupModelPrefab;
            pickupMaterial = utilityPickup.material;
            pickupDuration = utilityPickup.duration;
            pickupCooldown = utilityPickup.cooldown;
            pickupName = utilityPickup.pickupName;
            pickupTextColor = utilityPickup.pickupTextColor;
            pickupSound = utilityPickup.pickedupSoundEffect;
            spacialSoundEffect = utilityPickup.spacialSoundEffect;
            pickedupVolume = utilityPickup.pickedupSoundvolume;
            pickedupPitch = utilityPickup.pickedupSoundpitch;
            spacialVolume = utilityPickup.spacialSoundvolume;
            spacialPitch = utilityPickup.spacialSoundpitch;
        }
        if(spacialAudio != null)
        {
            spacialAudio.clip = spacialSoundEffect;
            spacialAudio.volume = spacialVolume;
            spacialAudio.pitch = spacialPitch;
            spacialAudio.loop = true;
        }


    }
    void PickupSpawn()
    {
        

        instantiatedModel = Instantiate(pickupModelPrefab, transform);
        instantiatedModel.transform.localPosition = Vector3.zero;
        instantiatedModel.transform.localRotation = Quaternion.identity;

        instantiatedEffect = Instantiate(pickupEffectPrefab, transform);
        instantiatedEffect.transform.localPosition = Vector3.zero;
        instantiatedEffect.transform.localRotation = Quaternion.identity;
        pickupEffect = instantiatedEffect.GetComponent<ParticleSystem>();


    }
    private IEnumerator PickupCooldown()
    {
        PickedUp();

        yield return new WaitForSeconds(pickupCooldown);

        PickupRespawn();
    }



    void PlayActivationSound()
    {
        if(pickupSound != null)
        {
            pickedupSound.pitch = pickedupPitch;
            pickedupSound.PlayOneShot(pickupSound, pickedupVolume);
        }
    }
    private void PickedUp()
    {
        pickupEffect.Play();
        PlayActivationSound();
        instantiatedModel.SetActive(false);
        pickupTrigger.enabled = false;
        
    }

    private void PickupRespawn()
    {
        pickupEffect.Play();
        PlayActivationSound();
        instantiatedModel.SetActive(true);
        pickupTrigger.enabled = true;

    }

    private void OnTriggerStay(Collider player)
    {
        SetPickupValues();
        if(player.CompareTag("Player"))
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();

            if (playerManager != null && playerManager.activeShootingPickup == null && pickupType == PickupType.ShootingPickup)    //Est‰‰ pelaajaa nostamasta kahta pickuppia samaan aikaan
            {
                playerManager.activeShootingPickup = this; 
                playerManager.StartCoroutine(playerManager.ActivateShootingPickup()); // Aktivoi pickupin vaikutukset
                StartCoroutine(PickupCooldown());

            }
            if(playerManager != null && playerManager.activeUtilityPickup == null && pickupType == PickupType.UtilityPickup)
            {
                playerManager.activeUtilityPickup = this;
                playerManager.StartCoroutine(playerManager.ActivateUtilityPickup());
                StartCoroutine(PickupCooldown());
            }
        }
    }

    void Update()
    {
        if (pickupTrigger.enabled && spacialAudio.clip != null && !spacialAudio.isPlaying)
        {

            spacialAudio.Play();
        }
        else if (!pickupTrigger.enabled && spacialAudio.isPlaying)
        {
            spacialAudio.Pause();
        }
    }


}
