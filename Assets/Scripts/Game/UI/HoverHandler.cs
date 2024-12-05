using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler
{
    private string pickupName;
    private string desc;
    private string cooldown;
    private string duration;
    private Color textColor;

    public TextMeshProUGUI infoScreen;
    
    // Kutsutaan, kun hiiri menee elementin p‰‰lle
    public void OnPointerEnter(PointerEventData eventData)
    {
        PickupHolder script = gameObject.GetComponent<PickupHolder>();
        int layerMask = LayerMask.NameToLayer("PickupToggles"); 

        if (script != null && gameObject.layer == layerMask)  // Vertailu oikeaan layeriin
        {
            pickupName = script.pickupName;
            desc = script.desc;
            cooldown = script.cooldown;
            duration = script.duration;
            textColor = script.color;
            
            UpdateInfoScreen();
        }
        else
        {
            Debug.LogWarning("ShootingPickupHolder component not found or object is not in the correct layer!");
        }
    }



    private void UpdateInfoScreen()
    {
        if (infoScreen != null)
        {
            string displayName = string.IsNullOrEmpty(pickupName) ? "Unknown Name" : pickupName;
            string displayDesc = string.IsNullOrEmpty(desc) ? "No Description Available" : desc;
            string displayDuration = string.IsNullOrEmpty(duration) ? "N/A" : duration;
            string displayCooldown = string.IsNullOrEmpty(cooldown) ? "N/A" : cooldown;

            
            Color displayColor = textColor == null ? Color.white : textColor;


            infoScreen.text = $"Name: {displayName}\nDescription: {displayDesc}\nDuration: {displayDuration} sec\nCooldown: {displayCooldown} sec";
            infoScreen.color = displayColor;
        }
        else
        {
            Debug.Log("Place the Text Object Dummy!");
        }

    }

}
