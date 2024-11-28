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
    
    
    public Pickup activePickup;

    public int hp;
    public int currentHP;
    public int earnedpoints;
    public float moveSpeed;
    public bool invincible;
    public bool overrun;
    private void SetPlayerStats()
    {
        hp = player.health;
        currentHP = player.health;
        earnedpoints = player.earnedPoints;
        moveSpeed = player.moveSpeed;
    }

    public IEnumerator ActivatePickup()
    {

        
        weapons.pickupInUse = activePickup;
        Debug.Log("Buff Gained: " + activePickup.pickupConfig.name);


        // Buffin vaikutuksen kesto
        yield return new WaitForSeconds(activePickup.pickupConfig.duration);

        // Palauta alkuperäiset arvot
        Debug.Log("Buff Ending");
        weapons.pickupInUse = null;
        activePickup = null;
        
    }
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        weapons = GetComponent<Shooting>();

        invincible = false;
        overrun = false;
        SetPlayerStats();
        gunBeingUsed = player.ChooseGun(player.chosenGun, player.availableGuns);
        gunBeingUsed.Spawn();
    }
    private IEnumerator InvincibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(1);
        invincible = false;
    }

    public void PlayerDeath()
    {
        Debug.Log("Overrun");
        overrun = true;
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
