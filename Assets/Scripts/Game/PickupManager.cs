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


    public void SpawnPickups(List<GameObject> chosenPickups, List<Transform> pickupPositions)
    {
        
        for (int i = 0; i < pickupPositions.Count; i++)
        {
            if (pickupPositions[i] != null && chosenPickups != null && i < chosenPickups.Count)
            {

                GameObject pickup = Instantiate(chosenPickups[i]);
                pickup.transform.position = pickupPositions[i].transform.position;
                spawnedPickups.Add(pickup);
                pickup.SetActive(false);
            }
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

}
