using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveBuff : MonoBehaviour
{
    [SerializeField] GameObject activePickup;
    [SerializeField] TextMeshProUGUI pickupText;

    public bool pickupActive = false;
    public IEnumerator SetPickupTextActive(float buffDuration, string name, Color textColor)
    {
        pickupActive = true;
        pickupText.color = textColor;
        pickupText.text = name;

        activePickup.SetActive(true);
        yield return new WaitForSeconds(buffDuration);
        activePickup.SetActive(false);
        pickupActive = false;
    }
    private void Start()
    {
        activePickup = transform.GetChild(4).gameObject;
        pickupText = activePickup.GetComponentInChildren<TextMeshProUGUI>();

        activePickup.SetActive(false);
    }
}
