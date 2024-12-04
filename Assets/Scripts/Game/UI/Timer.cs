using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI lastPersonalBest;
    
    
    private void Start()
    {
        timerText = transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>();
        lastPersonalBest = transform.GetChild(2).transform.GetComponent<TextMeshProUGUI>();
        //testinä hardcode Personal Best;
        

    }


    public void SetTimerText(float elapsedTime, float lastRecord)
    {
        
        float flooredTime = Mathf.Round(elapsedTime * 100.0f) / 100f;
        timerText.text = "Time: " + flooredTime.ToString();
        if (elapsedTime > lastRecord)
        {
            UpdateRecordText(elapsedTime, lastRecord);
        }
    }
    public void UpdateRecordText(float elapsedTime, float lastRecord)
    {
        lastRecord = elapsedTime;

        float flooredTime = Mathf.Round(lastRecord * 100.0f) / 100f;
        lastPersonalBest.text = "Record: " + flooredTime.ToString();
        lastPersonalBest.color = Color.red;
    }
    public void TimerSetup(float lastRecord)
    {
        float flooredTime = Mathf.Round(lastRecord * 100.0f) / 100f;
        lastPersonalBest.text = "Record: " + flooredTime.ToString();
    }

}
