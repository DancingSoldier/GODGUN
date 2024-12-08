using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Koodi rakenne:

//AnimatorManager-------> PlayerLocomotion<---------InputManager
//                                  ^-------------PlayerManager(Update)
public class PlayerManager : MonoBehaviour
{

    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    Shooting weapons;

    
    
    public GunScriptableObject gunBeingUsed;
    public PlayerScriptableObject player;
    
    
    public Pickup activeShootingPickup;
    public Pickup activeUtilityPickup;
    public int hp;
    public int currentHP;
    public int earnedpoints;
    public float moveSpeed;
    public bool invincible;
    public bool overrun;
    public bool touched;

    public bool pickupTaken = false;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        weapons = GetComponent<Shooting>();

        invincible = false;
        overrun = false;
        touched = false;
        SetPlayerStats();
        gunBeingUsed = player.ChooseGun(player.chosenGun, player.availableGuns);
        gunBeingUsed.Spawn();
    }

    private void SetPlayerStats()
    {
        hp = player.health;
        currentHP = player.health;
        earnedpoints = player.earnedPoints;
        moveSpeed = player.moveSpeed;
    }


    public IEnumerator ActivateShootingPickup()
    {
        weapons.pickupInUse = activeShootingPickup;
        Debug.Log("Buff Gained: " + activeShootingPickup.pickupName);

        // Buffin vaikutuksen kesto
        yield return new WaitForSeconds(activeShootingPickup.pickupDuration);

        // Nollaa tila vaikutuksen p‰‰ttyess‰
        pickupTaken = false;
        Debug.Log("pickupTaken reset: " + pickupTaken);

        // Palauta alkuper‰iset arvot
        Debug.Log("Buff Ending");
        weapons.pickupInUse = null;
        activeShootingPickup = null;
    }


    public IEnumerator ActivateUtilityPickup()
    {
        ApplyUtilityPickup();
        Debug.Log("Utility pickup activated.");

        yield return new WaitForSeconds(activeUtilityPickup.pickupDuration);

        // Nollaa tila vaikutuksen p‰‰ttyess‰
        pickupTaken = false;
        Debug.Log("pickupTaken reset: " + pickupTaken);

        activeUtilityPickup = null;
    }

    private void ApplyUtilityPickup()
    {
        GameObject utilityObject = Instantiate(activeUtilityPickup.utilityPickup.effectPrefab, transform.position, Quaternion.identity);
        Destroy(utilityObject, activeUtilityPickup.pickupDuration);

    }

    private IEnumerator InvincibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(1);
        invincible = false;
    }

    private IEnumerator LastStand()
    {
        touched = true;
        yield return new WaitForSeconds(2);
        Debug.Log("Overrun");
        overrun = true;
    }

    public void PlayerDeath()
    {
        StartCoroutine(LastStand());
    }


    public void ReduceHealth()
    {
        if (!invincible)
        {
            currentHP -= 1;
            StartCoroutine(InvincibilityFrames());
            if (currentHP <= 0)
            {
                PlayerDeath();
            }
        }
    }


    private void Update()
    {
        inputManager.HandleAllInputs();

    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
        
    }
}
