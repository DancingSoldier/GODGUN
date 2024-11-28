using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Characters/Player", order = 1)]
public class PlayerScriptableObject : ScriptableObject
{
    [Header("Player Stats")]

    public float moveSpeed;
    public int health;
    public int earnedPoints;
    public float bestTime;
    [Header("Animations")]

    public float turnThreshold;
    public float turnSpeed;


    [Header("Guns")]
    public List<GunScriptableObject> availableGuns;
    public Guns chosenGun;


    public GunScriptableObject ChooseGun(Guns chosenGun, List<GunScriptableObject> availableGuns)
  
    {
            // k‰yd‰‰n aseet l‰pi
            foreach (var gun in availableGuns)
            {
                // Tarkistetaan, vastaako aseen tyyppi valittua enum-arvoa
            if (gun.type == chosenGun)
                {
                    // Palautetaan valittu ase
                return gun;
                }
            }


        Debug.LogWarning("No gun of the chosen type found!");
        return null;
    }


    }
