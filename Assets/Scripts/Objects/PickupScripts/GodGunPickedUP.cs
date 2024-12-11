using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class GodGunPickedUP : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("GodGunGained");
            
            if(!GameManager.manager.godGunGained)
            {
                AnalyticsManager.Instance.GodgunGained();
                GameManager.manager.godGunGained = true;
                
            }
            if(!GameManager.manager.firstTimeGodGunGained)
            {
                GameManager.manager.firstTimeGodGunGained = true;
                Debug.Log("GodGunGained for the first time");
            }
            
            
        }
    }
}
