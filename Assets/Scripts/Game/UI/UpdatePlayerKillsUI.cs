using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdatePlayerKillsUI : MonoBehaviour
{

    int currentKills;
    TextMeshProUGUI killsText;
    void Start()
    {
        killsText = transform.GetChild(3).transform.GetComponent<TextMeshProUGUI>();
        killsText.gameObject.SetActive(true);
    }

    public void UpdateKills()
    {
        if(GameManager.manager.playerKillsTotal != currentKills)
        {
            int currentKills = GameManager.manager.playerKillsTotal;
            killsText.text = currentKills.ToString();
        }

    }

    private void Update()
    {
        UpdateKills();
    }

}
