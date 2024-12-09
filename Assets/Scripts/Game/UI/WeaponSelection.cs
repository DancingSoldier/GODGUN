using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSelection : MonoBehaviour
{
    public List<GameObject> weaponList = new List<GameObject>();
    public List<GameObject> weaponSelectionModels = new List<GameObject>();
    public List<string> weaponInfos = new List<string>();
    public Guns currentGun;
    public Transform gunHolderPosition;
    public TextMeshProUGUI weaponInfo;
    private int currentIndex = 0; // Pit�� yll� valitun aseen indeksi�

    public void Combat()
    {
        SceneManager.LoadScene("Combat");
    }



    public void ChangeSelection(bool next)
    {
        // P�ivit� indeksi k�ytt�j�n sy�tteen mukaan
        if (next)
        {
            currentIndex = (currentIndex + 1) % weaponList.Count; // Siirry seuraavaan (kiert�v� lista)
            
        }
        else
        {
            currentIndex = (currentIndex - 1 + weaponList.Count) % weaponList.Count; // Siirry edelliseen (kiert�v� lista)
        }
        if (currentIndex >= 0 && currentIndex < weaponInfos.Count)
        {
            weaponInfo.text = weaponInfos[currentIndex];
        }
        else
        {
            weaponInfo.text = "No weapon info available";
        }
        // P�ivit� nykyinen ase ja n�kyvyys
        UpdateSelection();
    }

    void UpdateSelection()
    {
        // P�ivit� n�kyvyys kaikille aseiden malleille
        for (int i = 0; i < weaponSelectionModels.Count; i++)
        {
            bool isActive = (i == currentIndex); // N�yt� vain nykyinen ase
            MeshRenderer[] renderers = weaponSelectionModels[i].GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in renderers)
            {
                renderer.enabled = isActive;
            }
                

        }


            // P�ivit� nykyinen ase (enum)
            currentGun = (Guns)currentIndex;
        GameManager.manager.ChangeWeapon(currentGun);

        Debug.Log("Selected weapon: " + GameManager.manager.chosenGun);
    }

    void Start()
    {
        // Luo aseiden mallit
        foreach (GameObject gun in weaponList)
        {
            GameObject gunInstance = Instantiate(gun, gunHolderPosition.position, Quaternion.identity);
            gunInstance.transform.parent = gunHolderPosition;
            MeshRenderer[] renderers = gunInstance.GetComponentsInChildren<MeshRenderer>();
            weaponSelectionModels.Add(gunInstance);

            foreach (var renderer in renderers)
            {
                renderer.enabled = false; // kaikki piilossa aluksi
            }
            
            Debug.Log("Spawned gunmodel: " + gun.name);
        }


        // ensimm�inen n�kyv�ksi
        if (weaponSelectionModels.Count > 0)
        {
            UpdateSelection();
        }
    }

    void Update()
    {
        // Debug-testit ja mahdolliset nuolin�pp�imet
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelection(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelection(false);
        }
    }
}
