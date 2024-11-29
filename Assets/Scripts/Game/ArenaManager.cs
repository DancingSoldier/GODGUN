using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{

    //Pelaaja ja UI

    public PlayerManager player;
    public GameObject playerPrefab;
    public GameObject gameUI;


    //pickupit

    public List<Transform> pickupPositions;
    public List<GameObject> pickups;
    public List<int> positionSpawnTimes;
    //UI

    public Timer timer;
    public GameOver gameOver;
    bool timerSetup = false;
    public SetPickupUIActive activePickupUi;

    //Stats

    public float elapsedTime;
    public float lastRecordTime;


    [Header("Centipede Spawning")]
    public int initialCentipedeAmount;
    public int centipedeIncrease;

    [Header("Elder Ghost Spawning")]
    public int initialElderGhostAmount;
    public int elderGhostIncrease;

    [Header("Ghost Spawning")]
    public int initialGhostAmount;
    public int ghostIncrease;

    //tänne mitä spawnataan ja milloin
    public List<GameObject> enemyList;
    public List<GameObject> spawnPointList;
    
    [Header("Waves")]
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private bool online;
    [SerializeField]
    private int difficultyIncreaseRate;
    private GameObjectPool centipedePool;
    private GameObjectPool ghostPool;
    private GameObjectPool elderPool;


    //Pelaajan Spawn

    void PlayerSpawn()
    {
        Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
    }

    //Pelaajan kuolema
    void NewRecordRecorded()
    {
        player.player.bestTime = Mathf.Round(elapsedTime *100.0f) / 100f;
    }
    void Overrun()
    {
        Time.timeScale = 0;

        if(lastRecordTime < elapsedTime)
        {
            NewRecordRecorded();
        }

        gameOver.GameOverSetup(Mathf.Round(elapsedTime * 100.0f) / 100f);
    }
    
    //Pickuppien spawnaus

    private void PickupSpawn()
    {
        pickups.Clear();
        for (int i = 0; i < pickupPositions.Count; i++)
        {
            if (pickupPositions[i] != null && player.player.chosenPickups != null && i < player.player.chosenPickups.Count)
            {

                GameObject pickup = Instantiate(player.player.chosenPickups[i]);
                pickup.transform.position = pickupPositions[i].transform.position;
                pickups.Add(pickup);
                pickup.SetActive(false);
            }
        }
    }

    private void PickupActivation()
    {
        for (int i = 0; i < pickups.Count; i++)
        {
            if(elapsedTime >= positionSpawnTimes[i])
            {
                pickups[i].SetActive(true);
            }
        }
    }

    //Vihollisten Spawnaus

    private void EnemyAmounts(GameObjectPool enemyToSpawn, int enemyWave)
    {


        for (int i = 0; i < enemyWave; i++) 
        {
            enemyToSpawn.GetObject(spawnPointList[Random.Range(0, 4)].transform.position);
        }
    }
    IEnumerator SpawnEnemies(bool online)
    {

        
        while (online == true)
        {
            
            if (Mathf.FloorToInt(elapsedTime) % difficultyIncreaseRate == 0 && Mathf.FloorToInt(elapsedTime) != 0)
            {
                IncreaseDifficulty(ref initialCentipedeAmount, centipedeIncrease);
                IncreaseDifficulty(ref initialGhostAmount, ghostIncrease);
                IncreaseDifficulty(ref initialElderGhostAmount, elderGhostIncrease);
            }
            EnemyAmounts(centipedePool, initialCentipedeAmount);
            EnemyAmounts(ghostPool, initialGhostAmount);
            EnemyAmounts(elderPool, initialElderGhostAmount);
            yield return new WaitForSeconds(cooldown);
        }

    }

    private void IncreaseDifficulty(ref int initialAmount, int increase)
    {
        // Kasvatetaan vihollisten määrää
        initialAmount += increase;

        
    }
    //funktiot joita kutsutaan updatessa

    //pickuppien UI asetus ja Spawnaus
    private void HandlePickups()
    {
        if (player.activeShootingPickup != null && !activePickupUi.shootingUIActive)
        {
            activePickupUi.StartCoroutine(activePickupUi.SetShootingPickupTextActive(
                    player.activeShootingPickup.pickupDuration,
                    player.activeShootingPickup.pickupName,
                    player.activeShootingPickup.pickupTextColor
            ));
        }
        if (player.activeUtilityPickup != null && !activePickupUi.utilityUIActive)
        {
            activePickupUi.StartCoroutine(activePickupUi.SetUtilityPickupTextActive(
                    player.activeUtilityPickup.pickupDuration,
                    player.activeUtilityPickup.pickupName,
                    player.activeUtilityPickup.pickupTextColor
            ));


        }
        //lopetetaan pickuppien tarkistaminen kun kaikki on spawnattu
        if (elapsedTime < 300)
        {
            PickupActivation();
        }
    }

    private void HandlePlayerDeath()
    {
        if (player.overrun)
        {

            Overrun();
        }
    }

    private void HandleTime()
    {
        if (timerSetup == false)
        {
            timer.TimerSetup(lastRecordTime);
            timerSetup = true;
        }

        elapsedTime += Time.deltaTime;

        timer.SetTimerText(elapsedTime, lastRecordTime);
    }



    private void Awake()
    {
        PlayerSpawn();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        gameUI = GameObject.FindGameObjectWithTag("GameUI");
        timer = gameUI.GetComponent<Timer>();
        gameOver = gameUI.GetComponent<GameOver>();
        activePickupUi = gameUI.GetComponent<SetPickupUIActive>();

        lastRecordTime = player.player.bestTime;
        PickupSpawn();


    }
    private void Start()
    {

        //luodaan vihollisten poolit
        
        centipedePool = new GameObjectPool(enemyList[0], 50, 100);
        ghostPool = new GameObjectPool(enemyList[1], 50, 100);
        elderPool = new GameObjectPool(enemyList[2], 50, 100);
        StartCoroutine(SpawnEnemies(online));
        Time.timeScale = 1.0f;


    }

    private void Update()
    {

        //ajanlasku
        HandleTime();
        
        //Pelaajan kuolema
        HandlePlayerDeath();

        //Pickupin nosto

        HandlePickups();
        
    }

}
