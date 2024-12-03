using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetPickupUIActive : MonoBehaviour
{
    [SerializeField] public GameObject activeShootingPickup;
    [SerializeField] public GameObject activeUtilityPickup;
    [SerializeField] TextMeshProUGUI shootingPickupText;
    [SerializeField] TextMeshProUGUI utilityPickupText;

    public bool shootingUIActive = false;
    public bool utilityUIActive = false;

    void ChangeText(TextMeshProUGUI pickupText, string name, Color textColor, ref bool uiActive)
    {
        pickupText.color = textColor;
        pickupText.text = name;
        uiActive = true;


    }
    public IEnumerator SetShootingPickupTextActive(float pickupDuration, string name, Color textColor)
    {
        ChangeText(shootingPickupText, name, textColor, ref shootingUIActive);

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
        activeShootingPickup = transform.GetChild(4).gameObject;
        activeUtilityPickup = transform.GetChild(5).gameObject;

        shootingPickupText = activeShootingPickup.GetComponentInChildren<TextMeshProUGUI>();
        utilityPickupText = activeUtilityPickup.GetComponentInChildren<TextMeshProUGUI>();
        activeShootingPickup.SetActive(false);
        activeUtilityPickup.SetActive(false);
    }
}
