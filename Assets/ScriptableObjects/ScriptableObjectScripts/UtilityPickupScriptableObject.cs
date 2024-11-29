using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Pickup", menuName = "Pickup/Utility Pickup", order = 7)]
public class UtilityPickupScriptableObject : ScriptableObject
{

    [Header("Pickup Properties")]
    public string pickupName;
    public string pickupDescription;
    public Color pickupTextColor;

    [Header("Time Floats")]
    public float cooldown;
    public float duration;

    [Header("Pickup Visuals")]
    public GameObject pickupModelPrefab;
    public Material material;
    public GameObject pickupEffect;

    [Header("Pickup Effect")]
    public GameObject effectPrefab;

}
