using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionScreen : MonoBehaviour
{
    public Canvas PickupLoadoutCanvas;
    public Canvas WeaponSelectionCanvas;
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
            PickupHolder script = icon.GetComponent<PickupHolder>();
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



    
    void ConfirmPickupSelection()
    {
        if(chosenPickups != null)
        {
            GameManager.manager.chosenPickups.AddRange(chosenPickups);
        }
        
    }

    public void ReturnToLoadout()
    {
        PickupLoadoutCanvas.enabled = true;
        WeaponSelectionCanvas.enabled = false;
    }

    public void ConfirmPickups()
    {
        ConfirmPickupSelection();
        PickupLoadoutCanvas.enabled = false;
        WeaponSelectionCanvas.enabled = true;
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
        GameManager.manager.chosenPickups.Clear();
        PickupLoadoutCanvas.enabled = true;
        WeaponSelectionCanvas.enabled = false;
        
    }
}
