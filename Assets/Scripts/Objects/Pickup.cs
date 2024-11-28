using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupScriptableObject pickupConfig;
    public GameObject pickupEffectPrefab;
    public GameObject pickupModelPrefab;
    

    GameObject instantiatedEffect;
    GameObject instantiatedModel;

    
    ParticleSystem pickupEffect;
    Collider pickupTrigger;
    Material pickupMaterial;
    SkinnedMeshRenderer modelRenderer;



    // Start is called before the first frame update
    void Start()
    {
        if (pickupConfig == null)
            Debug.LogError("No pickup scriptable object");
        pickupEffectPrefab = pickupConfig.pickupEffect;
        pickupModelPrefab = pickupConfig.pickupModelPrefab;
        PickupSpawn();
        pickupTrigger = GetComponentInChildren<Collider>();
        modelRenderer = instantiatedModel.GetComponent<SkinnedMeshRenderer>();
        
        pickupMaterial = pickupConfig.material;
        modelRenderer.material = pickupMaterial;    
        
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
        yield return new WaitForSeconds(pickupConfig.cooldown);
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

            if (playerManager != null && playerManager.activePickup == null)    //Est‰‰ pelaajaa nostamasta kahta pickuppia samaan aikaan
            {
                playerManager.activePickup = this; // Aseta poimittu pickup
                playerManager.StartCoroutine(playerManager.ActivatePickup()); // Aktivoi pickupin vaikutukset
                StartCoroutine(PickupCooldown());

            }


            
        }
    }


}
