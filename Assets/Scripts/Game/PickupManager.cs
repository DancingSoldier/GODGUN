using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> pickupPositions;
    [HideInInspector]
    public List<GameObject> spawnedPickups = new List<GameObject>();
    public List<int> positionSpawnTimes;
    public Transform godgunPos;
    public GameObject godGunPrefab;
    GameObject godGun;
    public void SpawnPickups(List<GameObject> chosenPickups, List<Transform> pickupPositions)
    {
        positionSpawnTimes.Clear();
        positionSpawnTimes.AddRange(GameManager.manager.pickupSpawnTimes);
        if (chosenPickups != null && chosenPickups.Count > 0 && GameManager.manager.chosenPickups != null)
        {
            
            for (int i = 0; i < pickupPositions.Count; i++)
            {
                if (pickupPositions[i] != null && i < chosenPickups.Count)
                {
                    
                    GameObject pickup = Instantiate(chosenPickups[i]);
                    float pickupCooldown = 0;
                    Color pickupColor = Color.white;
                    pickup.transform.position = pickupPositions[i].transform.position;
                    spawnedPickups.Add(pickup);
                    if(GetShootingPickupScript(pickup) != null)
                    {
                        ShootingPickupScriptableObject script = GetShootingPickupScript(pickup);
                        pickupCooldown = script.cooldown;
                        pickupColor = script.pickupTextColor;
                    }
                    else if(GetUtilityPickupScript(pickup) != null)
                    {
                        UtilityPickupScriptableObject script = GetUtilityPickupScript(pickup);
                        pickupCooldown = script.cooldown;
                        pickupColor = script.pickupTextColor;
                    }
                    PickupTimer timer = pickupPositions[i].GetComponent<PickupTimer>();
                    if(timer != null)
                    {
                        timer.cooldown = pickupCooldown;
                        timer.spawnTime = positionSpawnTimes[i];
                        timer.color = pickupColor;
                        Debug.Log($"Cooldown: {pickupCooldown}, {positionSpawnTimes[i]}");
                    }
                    
                    pickup.SetActive(false);
                }
            }


        }
        else
        {
            Debug.LogWarning("No chosen pickups available or chosenPickups is null!");
        }


    }


    ShootingPickupScriptableObject GetShootingPickupScript(GameObject pickup)
    {
        if (pickup.GetComponent<Pickup>().shootingPickup != null)
        {
            ShootingPickupScriptableObject script = pickup.GetComponent<Pickup>().shootingPickup;
            return script;
        }
        else
        {
            Debug.LogError("No script found");
            return null;
        }
    }
    UtilityPickupScriptableObject GetUtilityPickupScript(GameObject pickup)
    {
        if (pickup.GetComponent<Pickup>().utilityPickup != null)
        {
            UtilityPickupScriptableObject script = pickup.GetComponent<Pickup>().utilityPickup;
            return script;
        }
        else
        {
            Debug.LogError("No script found");
            return null;
        }
    }
    public void ActivatePickup(float elapsedTime)
    {
        for (int i = 0; i < spawnedPickups.Count; i++)
        {
            
            if (elapsedTime >= positionSpawnTimes[i])
            {
                
                spawnedPickups[i].SetActive(true);

            }
        }
    }

    public void ActivateGodGun(float elapsedTime)
    {
        if (elapsedTime >= GameManager.manager.godGunSpawnTime && !GameManager.manager.godGunGained)
        {
            godGun.SetActive(true);
            GameManager.manager.godGunGained = true;

        }
        if(elapsedTime >= GameManager.manager.godGunSecondSpawnTime && GameManager.manager.godGunGained)
        {
            godGun.SetActive(true);
        }
    }
    public void SetGodGun()
    {
        godGun = Instantiate(godGunPrefab, godgunPos.position, Quaternion.identity);
        godGun.SetActive(false);

    }

}
