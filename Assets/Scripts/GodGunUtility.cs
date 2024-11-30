using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodGunUtility : MonoBehaviour
{
    public GameObject godGunPrefab;
    public UtilityPickupScriptableObject pickup;
    Transform placement;
    public GameObject godGunInstance;


    private void Start()
    {
        GameObject godGunInstance = Instantiate(godGunPrefab);
        placement = GameObject.FindGameObjectWithTag("GODGUN").transform;
        godGunInstance.transform.SetParent(placement);
        godGunInstance.transform.position = placement.position;
        godGunInstance.transform.rotation = Quaternion.identity;
        godGunInstance.SetActive(true);
        Destroy(godGunInstance, pickup.duration);
        Destroy(gameObject, pickup.duration);
    }
    private IEnumerator Duration(GameObject godGunInstance)
    {
        yield return new WaitForSeconds(pickup.duration);
        Destroy(godGunInstance);
        Destroy(gameObject);
    }

}
