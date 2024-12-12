using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickupHolder : MonoBehaviour
{
    public GameObject pickup;
    public ShootingPickupScriptableObject pickupSO;
    public UtilityPickupScriptableObject pickupUO;
    public string pickupName;
    public string desc;
    public string cooldown;
    public string duration;
    public Color color;
    public int killsRequired;
    public Image pickupIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI killsText;
    SelectionScreen selectionManager;

    Toggle toggle;

    public void SetActive()
    {
        if (pickupSO != null || pickupUO != null)
        {
            killsText.enabled = false;
            pickupIcon.enabled = true;
            pickupIcon.color = color;
            nameText.color = color;
            nameText.text = pickupName;
            toggle.interactable = true;

        }


    }
    public void SetInactive(int kills)
    {
        if (pickupSO != null || pickupUO != null)
        {
            killsRequired = killsRequired - kills;
            killsText.enabled = true;
            pickupIcon.enabled = true;
            pickupIcon.color = Color.white;
            nameText.color = Color.white;
            nameText.text = $"Locked\n {pickupName}";
            killsText.color = Color.white;
            killsText.text = "Kills Required: " + killsRequired;
            toggle.interactable = false;

        }

    }


    void SetIconInfo()
    {
        toggle = transform.GetComponent<Toggle>();
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        pickupIcon = transform.GetChild(1).GetComponent<Image>();
        killsText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();


        if (pickup != null)
        {
            // Hae shootingPickup ja utilityPickup
            pickupSO = pickup.transform.GetComponent<Pickup>().shootingPickup;
            pickupUO = pickup.transform.GetComponent<Pickup>().utilityPickup;

            // Tarkista, onko pickupSO määritetty
            if (pickupSO != null)
            {
                pickupName = pickupSO.pickupName;
                desc = pickupSO.pickupDescription;
                cooldown = pickupSO.cooldown.ToString();
                duration = pickupSO.duration.ToString();
                color = pickupSO.pickupTextColor;
                killsRequired = pickupSO.killsRequired;
                Debug.Log("Loaded Shooting Pickup Scriptable Object");
            }
            else if (pickupUO != null)
            {
                // Jos pickupSO on null, mutta pickupUO on määritetty
                pickupName = pickupUO.pickupName;
                desc = pickupUO.pickupDescription;
                cooldown = pickupUO.cooldown.ToString();
                duration = pickupUO.duration.ToString();
                color = pickupUO.pickupTextColor;
                killsRequired = pickupUO.killsRequired;
                Debug.Log("Loaded Utility Pickup Scriptable Object");
            }
            else
            {
                // Molemmat ovat null, virheilmoitus
                Debug.LogError("Neither ShootingPickupScriptableObject nor UtilityPickupScriptableObject found!");
            }
        }
        else
        {
            // Jos pickup on null, tyhjennetään kaikki
            nameText.text = "";
            killsText.text = "";
            toggle.interactable = false;
            pickupIcon.enabled = false;
            pickupIcon.color = Color.white;
            Debug.LogWarning(gameObject.name + "Pickup object is null!");
        }
    }


    public void AddPickup()
    {
        if (selectionManager.chosenPickups.Count >= 5)
        {
            //StartCoroutine(BlinkRed()); // Aktivoi punainen vilkkumisen

            Debug.Log($"Max pickups reached {selectionManager.chosenPickups.Count}, cannot add more.");
        }
        else
        {
            selectionManager.chosenPickups.Add(pickup);
            Debug.Log("Adding Pickup" + pickup.name + " to list");
        }
    }

    public void RemovePickup()
    {
        selectionManager.chosenPickups.Remove(pickup);
        Debug.Log("Removing Pickup" + pickup.name + " from list");
    }


    public void SendMessage(bool value)
    {
        if (value)
        {
            AddPickup();


        }
        else
        {
            RemovePickup();


        }
        

    }
    private void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        if (isOn)
        {
            cb.normalColor = Color.red;
            cb.selectedColor = Color.red;
            cb.highlightedColor = cb.normalColor;


        }
        else if (!isOn && selectionManager.chosenPickups.Count <= 5)
        {
            cb.normalColor = Color.black;
            cb.selectedColor = Color.black;
            cb.highlightedColor = cb.normalColor;

        }
        toggle.colors = cb;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetIconInfo();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        selectionManager = GameObject.FindGameObjectsWithTag("SelectionManager")[0].transform.GetComponent<SelectionScreen>();
    }


}