using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    //yhteiset pickupin tiedot
    public float pickupDuration;
    public float pickupCooldown;
    public string pickupName;
    public Color pickupTextColor;


    // Start is called before the first frame update
    void Start()
    {
        if (shootingPickup == null && utilityPickup == null)
        {
            Debug.LogError("No pickup scriptable object");
            return;
        }

        // M‰‰ritet‰‰n pickup-tyyppi
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
        }
        
        
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

    private void PickedUp()
    {
        pickupEffect.Play();
        instantiatedModel.SetActive(false);
        pickupTrigger.enabled = false;

    }

    private void PickupRespawn()
    {
        pickupEffect.Play();
        instantiatedModel.SetActive(true);
        pickupTrigger.enabled = true;
    }

    private void OnTriggerEnter(Collider player)
    {
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


}
