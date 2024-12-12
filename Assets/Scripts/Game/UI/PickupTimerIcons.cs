using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupTimerIcons : MonoBehaviour
{
    GameObject timerHolder;
    public GameObject iconPrefab;
    public List<float> cooldowns = new List<float>();
    public List<GameObject> icons = new List<GameObject>();

    void InstantiatePickupIcons()
    {
        // Ensure the timerHolder is set
        if (timerHolder == null)
        {
            Debug.LogError("Timer Holder is not assigned or found!");
            return;
        }

        // Clear any existing icons to avoid duplication
        foreach (Transform child in timerHolder.transform)
        {
            Destroy(child.gameObject);
        }

        // Loop through chosenPickups in GameManager
        foreach (GameObject pickup in GameManager.manager.chosenPickups)
        {
            // Check if the corresponding icon exists in the icons list
            if (pickup != null)
            {
                
                if (iconPrefab != null)
                {
                    // Instantiate the icon and parent it to timerHolder
                    GameObject instantiatedIcon = Instantiate(iconPrefab, timerHolder.transform);
                    instantiatedIcon.name = pickup.name;
                    Image image = instantiatedIcon.GetComponent<Image>();
                    float cooldown = 20f;
                    if (pickup.GetComponent<Pickup>() != null)
                    {
                        
                        if(pickup.GetComponent<Pickup>().shootingPickup != null)
                        {
                            cooldown = pickup.GetComponent<Pickup>().shootingPickup.cooldown;
                            image.color = pickup.GetComponent<Pickup>().shootingPickup.pickupTextColor;
                        }
                        else if(pickup.GetComponent<Pickup>().utilityPickup != null)
                        {
                            cooldown = pickup.GetComponent<Pickup>().utilityPickup.cooldown;
                            image.color = pickup.GetComponent<Pickup>().utilityPickup.pickupTextColor;
                        }
                        
                        cooldowns.Add(cooldown);
                        icons.Add(instantiatedIcon);
                    }
                    // Optional: Initialize additional properties like cooldown or display
                    TextMeshProUGUI cooldownText = instantiatedIcon.GetComponentInChildren<TextMeshProUGUI>();
                    if (cooldownText != null)
                    {
                        cooldownText.text = "";
                    }
                }
                else
                {
                    Debug.LogWarning($"No matching icon found for pickup: {pickup.name}");
                }
            }
        }
    }

    private IEnumerator StartPickupCooldown(GameObject pickupIcon, float cooldownDuration)
    {
        if (pickupIcon == null)
        {
            Debug.LogError("Pickup icon is null! Cannot start cooldown.");
            yield break;
        }

        TextMeshProUGUI cooldownText = pickupIcon.GetComponentInChildren<TextMeshProUGUI>();
        Image iconImage = pickupIcon.GetComponent<Image>();

        if (cooldownText == null || iconImage == null)
        {
            Debug.LogError("Cooldown text or icon image is missing on the pickup icon!");
            yield break;
        }

        float elapsedTime = 0;
        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = Mathf.Max(0, cooldownDuration - elapsedTime);
            cooldownText.text = $"{remainingTime:F1}s"; // Update remaining time display
            

            yield return null;
        }

        // Reset icon after cooldown
        cooldownText.text = "Ready";
        
    }

    public void OnPickupTaken(string pickupName, Pickup pickup)
    {
        string name = pickupName;
        int index = GameManager.manager.chosenPickups.FindIndex(p => p.name == name);
        
        if (index >= 0 && index < icons.Count)
        {
            GameObject correspondingIcon = icons[index];
            float cooldown = 20f;
            if(pickup.shootingPickup != null)
            {
                Debug.Log(pickup.shootingPickup);
                cooldown = pickup.shootingPickup.cooldown;
            }
            else
            {
                cooldown = pickup.utilityPickup.cooldown;
            }
            StartCoroutine(StartPickupCooldown(correspondingIcon, cooldown));
        }
        else
        {
            Debug.LogWarning($"No matching icon found for pickup: {name}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timerHolder = transform.GetChild(9).gameObject;
        InstantiatePickupIcons();
        foreach (var pickup in GameManager.manager.chosenPickups)
        {
            Debug.Log($"Chosen Pickup: {pickup.name}");
        }
        foreach (var icon in icons)
        {
            Debug.Log($"Icon: {icon.name}");
        }

    }

    // Update is called once per frameS
}
