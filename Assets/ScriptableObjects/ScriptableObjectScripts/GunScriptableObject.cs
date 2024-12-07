using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun Configuration", order = 2)]
public class GunScriptableObject : ScriptableObject
{
    public AttackScriptableObject mainAttackConfig;
    public AttackScriptableObject altAttackConfig;
    public AttackAudioScriptableObject mainAttackAudio;
    public AttackAudioScriptableObject altAttackAudio;
    public bool usedMainAttack;
    public GameObject gunModelPrefab;
    public Guns type;

    public Vector3 spawnPoint;
    public Vector3 spawnRotation;
    Transform gunParent;
    private GameObject model;

    
    public Transform projectileSpawnPoint;
    public void Spawn()
    {
        gunParent = GameObject.Find("GunHolder").transform;
        model = Instantiate(gunModelPrefab);
        model.transform.SetParent(gunParent, false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);


        projectileSpawnPoint = model.transform.GetChild(0);

        
        if (projectileSpawnPoint == null)
        {
            Debug.Log("ProjectileSpawnPoint not found in gun model prefab.");
        }


    }
}
