using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionScreen : MonoBehaviour
{
    
    public int playerKills;
    
    public List<GameObject> shootingIcons = new List<GameObject>();
    public List<GameObject> chosenPickups = new List<GameObject>();
    public List<Image> pickupSlots = new List<Image>();
    void SetIcons(int kills)
    {
        shootingIcons.Clear();
        GameObject[] foundIcons = GameObject.FindGameObjectsWithTag("ShootingIcon");
        shootingIcons.AddRange(foundIcons);
        foreach (GameObject icon in shootingIcons)
        {
            Debug.Log("Found icon: " + icon.name);
            ShootingPickupHolder script = icon.GetComponent<ShootingPickupHolder>();
            if (script != null)
            {
                if (kills >= script.killsRequired)
                {
                    Debug.Log("Setting Active: " + icon.name);
                    script.SetActive();
                }
                else
                {
                    Debug.Log("Setting Inactive: " + icon.name);
                    script.SetInactive(kills);
                }
            }
        }
    }

    public void UpdatePickupSlotColors()
    {
        if(chosenPickups.Count != null)
        {
            for (int i = 0; i < chosenPickups.Count; i++)
            {
                Color shootNodeColor = chosenPickups[i].GetComponent<Pickup>().shootingPickup.pickupTextColor;
                Color utiliNodeColor = chosenPickups[i].GetComponent<Pickup>().utilityPickup.pickupTextColor;
                if (shootNodeColor != null)
                {
                    Debug.Log("shoot");
                }
                else if (utiliNodeColor != null)
                {
                    Debug.Log("utility");
                }
                else
                {
                    Debug.Log("none");
                }
            }
        }


    }

    
    void ConfirmPickupSelection()
    {
        if(chosenPickups != null)
        {
            GameManager.manager.chosenPickups.AddRange(chosenPickups);
        }
        
    }


    public void Combat()
    {
        ConfirmPickupSelection();
        SceneManager.LoadScene("Combat");
    }
    public void Quit()
    {

        Application.Quit();
        Debug.Log("Quitting");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    private void Start()
    {
        SetIcons(GameManager.manager.playerKillsTotal);
        //FindIndicators();
    }
}
