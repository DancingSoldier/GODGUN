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

    public GameObject popup1;
    public GameObject popup2;
    public GameObject popup3;
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

    public void DisableToggleInteractions()
    {
        foreach (GameObject icon in shootingIcons)
        {
            // Hae PickupHolder-komponentti
            PickupHolder pickupHolder = icon.GetComponent<PickupHolder>();

            if (pickupHolder != null)
            {
                // Tarkista, onko pickup mukana chosenPickups-listassa
                if (!chosenPickups.Contains(pickupHolder.pickup))
                {
                    // Disabloi toggle-nappi
                    Toggle toggle = icon.GetComponent<Toggle>();
                    if (toggle != null)
                    {
                        toggle.interactable = false;
                    }
                }
                else
                {
                    // Varmista, ett� valittujen pickupien napit pysyv�t aktiivisina
                    Toggle toggle = icon.GetComponent<Toggle>();
                    if (toggle != null)
                    {
                        toggle.interactable = true;
                    }
                }
            }
        }
    }

    void CheckIcons()
    {
        if(chosenPickups.Count == 5)
        {
            DisableToggleInteractions();
        }
    }

    //odottaa framen est��kseen bugin buildiss�
    private IEnumerator InitializeIcons()
    {
        yield return null;
        SetIcons(GameManager.manager.playerKillsTotal);
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
        GameManager.manager.chosenPickups.Clear();
    }

    public void ConfirmPickups()
    {
        ConfirmPickupSelection();
        PickupLoadoutCanvas.enabled = false;
        WeaponSelectionCanvas.enabled = true;
        FirstTimeChoosingGun();
    }

    public void ResetPickups()
    {
        GameManager.manager.chosenPickups.Clear();
        SceneManager.LoadScene("Loadout");
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

    public void PlayFirstTime()
    {
        if (GameManager.manager.firstTimePlaying)
        {
            DialogueScript script = popup1.GetComponent<DialogueScript>();
            if (script != null)
            {
                script.StartDialogue();
                GameManager.manager.firstTimePlaying = false;
            }
        }
    }

    public void FirstPickup()
    {
        if (!GameManager.manager.firstPickupEarned)
        {
            DialogueScript script = popup2.GetComponent<DialogueScript>();
            if (script != null && !GameManager.manager.firstPickupEarned)
            {
                script.StartDialogue();
                GameManager.manager.firstPickupEarned = true;
            }
        }
    }
    public void FirstTimeChoosingGun()
    {
        if (GameManager.manager.firstTimeChoosingGun)
        {
            DialogueScript script = popup3.GetComponent<DialogueScript>();
            if (script != null)
            {
                script.StartDialogue();
                GameManager.manager.firstTimeChoosingGun = false;
            }
        }
    }

    private IEnumerator FirstTimePopupDelayed()
    {
        yield return new WaitForSeconds(1);
        PlayFirstTime();
    }
    private void Start()
    {
        StartCoroutine(InitializeIcons());
        GameManager.manager.chosenPickups.Clear();
        PickupLoadoutCanvas.enabled = true;
        WeaponSelectionCanvas.enabled = false;
        StartCoroutine(FirstTimePopupDelayed());
    }
    private void Update()
    {
        CheckIcons();
    }
}
