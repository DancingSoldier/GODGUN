using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<GameObject> earnedPickups = new List<GameObject>();
    public List<GameObject> chosenPickups = new List<GameObject>();
    public List<int> pickupSpawnTimes = new List<int>();
    PlayerScriptableObject playerScriptableObject;

    public int playerKillsTotal = 0;
    public bool godGunGained = false;
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
    public void Save(int kills)
    {
        playerKillsTotal = kills;
    }

    public void IncreaseKills()
    {
        playerKillsTotal += 1;
    }
    private void Start()
    {
        Save(250);
    }

}
