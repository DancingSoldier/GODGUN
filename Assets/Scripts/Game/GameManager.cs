using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public float initialRecord = 49.99f;
    [HideInInspector]
    public int playerKillsTotal = 0;
    [HideInInspector]
    public bool godGunGained = false;
    public Guns chosenGun;
    public EnemySpawnerScriptableObject enemySpawner;
    public UtilityPickupScriptableObject godGun;
    public PlayerScriptableObject player;
    public List<GameObject> chosenPickups = new List<GameObject>();
    
    [Header("Game PickupSettings")]
    public List<int> pickupSpawnTimes = new List<int>();
    public float godGunSpawnTime = 300;
    public float godGunSecondSpawnTime = 100;
    public float godGunCooldown;
    public float recordTime = 0;
    [Header("Spawner")]
    public float waveFrequency;
    public bool spawnerOnline;

    public bool firstTimePlaying = true;
    public bool firstPickupEarned = false;
    public bool firstTimeChoosingGun = true;
    public bool firstTimeGodGunGained = false;
    private void Awake()
    {
        //luodaan singleton
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }



    public IEnumerator ClearText(int seconds, TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(seconds);
        text.text = "";
    }


    public void UpdateKills(int kills)
    {
        playerKillsTotal = kills;
    }

    public void IncreaseKills()
    {
        playerKillsTotal += 1;
    }
    public void UpdateRecord(float time)
    {
        if(time > recordTime)
        {
            recordTime = time;
        }

    }


    public void SaveData()
    {

        SaveSystem.SavePlayer(this);
    }

    public void ChangeWeapon(Guns gun)
    {
        chosenGun = gun;
        player.chosenGun = chosenGun;
    }
    
    public string LoadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        string godGunStatus;
        if(data != null)
        {
            playerKillsTotal = data.playerKillsTotal;
            godGunGained = data.godGunGained;
            recordTime = data.recordTime;
            firstPickupEarned = data.firstPickupEarned;
            firstTimePlaying = data.firstTimePlaying;
            firstTimeChoosingGun = data.firstTimeChoosingGun;
            firstTimeGodGunGained = data.firstTimeGodGunGained;
            if(godGunGained)
            {
                godGunStatus = "Gained";
            }
            else
            {
                godGunStatus = "Not Gained";
            }
            Debug.Log($"Player Stats\n Kills:{playerKillsTotal} Record: {recordTime} GodGunGained: {godGunGained}");
            string message = $"Save Data Loaded\n Player Stats\n Kills:{playerKillsTotal}\n Record: {recordTime}\n GodGun {godGunStatus} ";
            return message;
        }
        else
        {
            
            ResetData();
            return "No Data, creating a Save File";
        }
    }
    public void ResetData()
    {
        SaveSystem.ResetPlayer();
        
        Debug.Log($"Player Stats Reset!\n Kills:{playerKillsTotal} Record: {recordTime} GodGunGained: {godGunGained}");
    }
    public float LastRecord()
    {
        return recordTime;
    }
    private void Start()
    {

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if(recordTime <= initialRecord)
        {
            recordTime = initialRecord;
        }
        godGun.cooldown = godGunCooldown;
        enemySpawner.cooldown = waveFrequency;
        enemySpawner.online = spawnerOnline;
        
    }

}
[Serializable]
public class PlayerData
{
    public int playerKillsTotal;
    public bool godGunGained;
    public float recordTime;
    public bool firstTimePlaying;
    public bool firstPickupEarned;
    public bool firstTimeChoosingGun;
    public bool firstTimeGodGunGained;
}