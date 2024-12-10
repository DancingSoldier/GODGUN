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
    // Shot events


    // Pickup choices
    public void PickupChosen(string pickupName)
    {
        switch (pickupName)
        {
            case "pickup_Heavy Hits":
                AnalyticsService.Instance.RecordEvent("Heavy Hits");
                return;
            case "pickup_Spread Shot":
                AnalyticsService.Instance.RecordEvent("Spread Shot");
                return;
            case "pickup_Fast Fire":
                AnalyticsService.Instance.RecordEvent("Fast Fire");
                return;
            case "pickup_Buckshot":
                AnalyticsService.Instance.RecordEvent("Buckshot");
                return;
            case "pickup_Piercing Shots":
                AnalyticsService.Instance.RecordEvent("Piercing Shots");
                return;
            case "pickup_Rainbow Shots":
                AnalyticsService.Instance.RecordEvent("Rainbow Shots");
                return;
            case "pickup_Quad Damage":
                AnalyticsService.Instance.RecordEvent("Quad Damage");
                return;
            case "pickup_Upgraded Fast Fire":
                AnalyticsService.Instance.RecordEvent("Upgraded Fast Fire");
                return;

        }
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
