using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PickupListDisplay : MonoBehaviour
{
    
    public TextMeshProUGUI pickupListText;
    SelectionScreen selectionManager;
    public List<int> spawnTimes = new List<int>();
    public void UpdatePickupList(List<GameObject> chosenPickups)
    {
        
        pickupListText.text = "";

        
        if (chosenPickups == null || chosenPickups.Count == 0)
        {
            pickupListText.text = "No pickups selected.";
            return;
        }

        
        for (int i = 0; i < chosenPickups.Count; i++)
        {
            string pickupName = chosenPickups[i].name; // Ota pickupin nimi
            string spawnTime = spawnTimes[i].ToString();
            string textColored = $"<color=#{"C30000"}>{spawnTime} seconds: </color>";
            pickupListText.text += $"{textColored} {pickupName}\n"; // Lis‰‰ numero ja nimi
        }
    }


    private void Start()
    {
        spawnTimes.Clear();
        selectionManager = GameObject.FindGameObjectsWithTag("SelectionManager")[0].transform.GetComponent<SelectionScreen>();
        spawnTimes.AddRange(GameManager.manager.pickupSpawnTimes);
    }

    private void Update()
    {
        UpdatePickupList(selectionManager.chosenPickups);
    }
}
