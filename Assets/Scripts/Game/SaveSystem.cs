
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;


public static class SaveSystem
{
    public static void SavePlayer(GameManager manager)
    {


        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/godgunInfo.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();
        data.playerKillsTotal = manager.playerKillsTotal;
        data.godGunGained = manager.godGunGained;
        data.recordTime = manager.recordTime;
        data.firstTimePlaying = manager.firstTimePlaying;
        data.firstPickupEarned = manager.firstPickupEarned;
        data.firstTimeChoosingGun = manager.firstTimeChoosingGun;
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Data Saved Succesfully");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/godgunInfo.dat";
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
                    Debug.Log("Data Loaded Succesfully");
                    return data;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to load data: " + ex.Message);
                return null;
            }
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }


    public static void ResetPlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/godgunInfo.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();
        data.playerKillsTotal = 0;
        data.godGunGained = false;
        data.recordTime = 0;
        data.firstTimePlaying = true;
        data.firstPickupEarned = false;
        data.firstTimeChoosingGun = true;
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Data Reset");
    }
}
