using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsSingleton : MonoBehaviour
{
    private static AnalyticsSingleton _instance;

    public static AnalyticsSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject instance = new("Analytics Manager");
                instance.AddComponent<AnalyticsSingleton>();
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
        AnalyticsService.Instance.RecordEvent("Godgun Gained");
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

    // Get the connection to UGS
    private async void Awake()
    {
        _instance = this;

        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }
}

