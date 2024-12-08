using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetPickupUIActive : MonoBehaviour
{
    [SerializeField] public GameObject activeShootingPickup;
    [SerializeField] public GameObject activeUtilityPickup;
    [SerializeField] TextMeshProUGUI durationText;
    [SerializeField] TextMeshProUGUI shootingPickupText;
    [SerializeField] TextMeshProUGUI utilityPickupText;

    public bool shootingUIActive = false;
    public bool utilityUIActive = false;
    private float timeRemaining1;
    private float timeRemaining2;

    void ChangeText(TextMeshProUGUI pickupText, string name, Color textColor, ref bool uiActive)
    {
        pickupText.color = textColor;
        pickupText.text = name;
        uiActive = true;
    }

    void UpdateDurationText(float duration)
    {
        // Muuta aika sekunneista muotoon mm:ss tai ss
        int minutes = Mathf.FloorToInt(duration / 60); // Laske minuutit
        int seconds = Mathf.FloorToInt(duration % 60); // Laske sekunnit

        // Päivitä TextMeshPro-teksti
        durationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private IEnumerator CountDown(float duration, Color textColor)
    {
        float timeRemaining = duration;
        durationText.enabled = true;
        durationText.color = textColor;
        while (timeRemaining > 0)
        {
            UpdateDurationText(timeRemaining); // Päivitä jäljellä oleva aika
            timeRemaining -= Time.deltaTime;
            yield return null; // Odota seuraavaan frameen
        }

        UpdateDurationText(0); // Aika loppui, näytä nollat
        durationText.enabled = false;
    }


    public IEnumerator SetShootingPickupTextActive(float pickupDuration, string name, Color textColor)
    {
        ChangeText(shootingPickupText, name, textColor, ref shootingUIActive);
        StartCoroutine(CountDown(pickupDuration, textColor));
        activeShootingPickup.SetActive(true);
        yield return new WaitForSeconds(pickupDuration);
        activeShootingPickup.SetActive(false);
        shootingUIActive = false;
    }
    public IEnumerator SetUtilityPickupTextActive(float pickupDuration, string name, Color textColor)
    {
        ChangeText(utilityPickupText, name, textColor, ref utilityUIActive);
        activeUtilityPickup.SetActive(true);
        yield return new WaitForSeconds(pickupDuration);
        activeUtilityPickup.SetActive(false);
        utilityUIActive = false;

    }
    private void Start()
    {
        activeShootingPickup = transform.GetChild(5).gameObject;
        activeUtilityPickup = transform.GetChild(6).gameObject;

        shootingPickupText = activeShootingPickup.GetComponentInChildren<TextMeshProUGUI>();
        utilityPickupText = activeUtilityPickup.GetComponentInChildren<TextMeshProUGUI>();
        activeShootingPickup.SetActive(false);
        activeUtilityPickup.SetActive(false);
        durationText.color = Color.white;
        durationText.enabled = false;
    }
}
