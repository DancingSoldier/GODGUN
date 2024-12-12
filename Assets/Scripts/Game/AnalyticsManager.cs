using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    private static AnalyticsManager _instance;

    public static AnalyticsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject instance = new("Analytics Manager");
                instance.AddComponent<AnalyticsManager>();
            }

            return _instance;
        }
    }

    //// Different analytics events
    // Player death
    public void PlayerDeath()
    {
        AnalyticsService.Instance.RecordEvent("playerDeath");
        
    }

    public void GodgunGained()
    {
        AnalyticsService.Instance.RecordEvent("godgunGained");
        
    }

    public void TimeRecorded(float elapsedTime)
    {
        TimeOfDeathEvent deathEvent = new() { timeOfDeath = elapsedTime };
        AnalyticsService.Instance.RecordEvent(deathEvent);
        
    }
    public void StartAnalytics()
    {
        Debug.Log("Analytics Started");
        AnalyticsService.Instance.StartDataCollection();
        
    }
    public void StopAnalytics()
    {
        Debug.Log("Analytics Stopped");
        AnalyticsService.Instance.StopDataCollection();
        
    }
    // Get the connection to UGS
    private async void Awake()
    {
        _instance = this;

        await UnityServices.InitializeAsync();
        
    }
}

class TimeOfDeathEvent : Unity.Services.Analytics.Event
{
    public TimeOfDeathEvent()
        : base("timeOfDeath") { }

    public float timeOfDeath
    {
        set { SetParameter("timeOfDeathValue", value); }
    }
}
