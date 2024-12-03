using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner", menuName = "Characters/Spawner", order = 2)]
public class EnemySpawnerScriptableObject : ScriptableObject
{
    [Header("Spawner settings")]
    public float cooldown;
    public bool online;

    public List<EnemySpawnConfig> enemyConfigs = new List<EnemySpawnConfig>();
    public List<EnemySpawnConfig> spawnList = new List<EnemySpawnConfig>();


    

    public void SyncSpawnListWithConfig()
    {
        spawnList = spawnList
        .OrderBy(config => enemyConfigs.FindIndex(c => c.enemyPrefab == config.enemyPrefab))
        .ToList();
    }
public void ResetVariables()
    {
        foreach (var config in enemyConfigs)
        {
            config.currentAmount = config.initialAmount;
            config.lastIncreaseTime = 0f;
            config.lastSpawnTime = 0f;
            if (config.spawnListAddTime != 0)
            {
                config.onSpawnCooldown = true;
            }
            else
            {
                config.onSpawnCooldown = false;
            }

        }
        spawnList.Clear();
    }

    public List<GameObject> GetSpawnPoints()
    {


        List<GameObject> list = new List<GameObject>();
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        list.AddRange(spawnPoints);
        return list;
    }
    public List<GameObjectPool> GenerateSpawnPools()
    {
        List<GameObjectPool> pools = new List<GameObjectPool>();

        // Käydään läpi kaikki enemyConfigs ja luodaan poolit dynaamisesti
        foreach (var config in enemyConfigs)
        {
            // Oletetaan, että GameObjectPool tarvitsee enemyPrefab:in ja poolin koon
            GameObjectPool pool = new GameObjectPool(config.enemyPrefab, 50, 100);
            pools.Add(pool);
        }

        return pools;
    }

    public void AddToSpawnList(EnemySpawnConfig configToAdd)
    {
        if(!spawnList.Contains(configToAdd))
        {
            spawnList.Add(configToAdd);
            Debug.Log($"EnemyConfig {configToAdd.enemyPrefab.name} added to spawn list.");
        }

    }

    public void UpdateEnemyAmounts(float elapsedTime)
    {
        foreach (var config in enemyConfigs)
        {
            if (elapsedTime > config.spawnListAddTime)
            {
                AddToSpawnList(config);

            }
        }
        SyncSpawnListWithConfig();
        foreach (var config in spawnList)
        {
            if ((elapsedTime - config.lastIncreaseTime) >= config.increaseCooldown)
            {
                config.currentAmount += config.amountIncrease;
                config.lastIncreaseTime = elapsedTime;
                Debug.Log($"New Amount of {config.enemyPrefab.name} is {config.currentAmount}");
            }
            if ((elapsedTime - config.lastSpawnTime) >= config.spawnCooldown)
            {
                config.lastSpawnTime = elapsedTime;
                Debug.Log($"{config.enemyPrefab.name} is ready to spawn. SpawnTime: {config.lastSpawnTime}");
                config.onSpawnCooldown = false;
            }

        }

    }

    public void SpawnEnemiesFromPools(List<GameObjectPool> pools, List<GameObject> spawnPointList)
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            if (!spawnList[i].onSpawnCooldown)
            {
                spawnList[i].onSpawnCooldown=true;
                for (int j = 0; j < spawnList[i].currentAmount; j++)
                {
                    pools[i].GetObject(spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)].transform.position);
                }
            }
            
            
        }
        

    }

    public IEnumerator SpawnEnemies(List<GameObjectPool> pools, List<GameObject> spawnPointList)
    {

        
        while (online == true)
        {



            SpawnEnemiesFromPools(pools, spawnPointList);


            yield return new WaitForSeconds(cooldown);
        }

    }
    [System.Serializable]
    public class EnemySpawnConfig
    {
        public GameObject enemyPrefab;
        public int initialAmount;
        public int amountIncrease;
        public float increaseCooldown;
        public float spawnListAddTime;
        public float spawnCooldown;
        
        
        
        // Dynaamiset arvot
        [HideInInspector] public int currentAmount;
        [HideInInspector] public float lastIncreaseTime;
        [HideInInspector] public float lastSpawnTime;
        [HideInInspector] public bool onSpawnCooldown = false;
        [HideInInspector] public bool onIncreaseCooldown = false;
    }


}
