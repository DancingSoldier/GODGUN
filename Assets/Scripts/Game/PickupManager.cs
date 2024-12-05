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

        if (chosenPickups != null && chosenPickups.Count > 0 && GameManager.manager.chosenPickups != null)
        {
            
            for (int i = 0; i < pickupPositions.Count; i++)
            {
                if (pickupPositions[i] != null && i < chosenPickups.Count)
                {
                    
                    GameObject pickup = Instantiate(chosenPickups[i]);
                    
                    pickup.transform.position = pickupPositions[i].transform.position;
                    spawnedPickups.Add(pickup);
                    pickup.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("No chosen pickups available or chosenPickups is null!");
        }

        positionSpawnTimes = GameManager.manager.pickupSpawnTimes;

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
        if (elapsedTime >= 200 && !GameManager.manager.godGunGained)
        {
            godGun.SetActive(true);
            GameManager.manager.godGunGained = true;

        }
        if(elapsedTime >= 69 && GameManager.manager.godGunGained)
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
