using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{

    //Piilotetut arvot
    [HideInInspector]
    public PlayerManager player;
    [HideInInspector]
    public GameObject playerPrefab;
    [HideInInspector]
    public GameObject gameUI;
    [HideInInspector]
    PickupManager pickupManager;
    [HideInInspector]
    public Timer timer;
    [HideInInspector]
    public GameOver gameOver;
    [HideInInspector]
    public SetPickupUIActive activePickupUi;
    [HideInInspector]
    public float elapsedTime;
    [HideInInspector]
    public float lastRecordTime;
    
    public List<GameObject> spawnPointList;

    [Header("Pickup Slots")]
    public List<Transform> pickupPositions;

    //timer check at the beginning
    bool timerSetup = false;

    public EnemySpawnerScriptableObject spawnManager;


    
    [Header("Waves")]
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private bool online;
    [SerializeField]
    private int difficultyIncreaseRate;
    private GameObjectPool centipedePool0;
    private GameObjectPool centipedePool1;
    private GameObjectPool ghostPool0;
    private GameObjectPool ghostPool1;
    List<GameObjectPool> pools = new List<GameObjectPool>();



    //Pelaajan Spawn

    void PlayerSpawn()
    {
        Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
    }

    private void HidePlayerModel()
    {
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>(); // Hakee kaikki Renderer-komponentit lapsista
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false; // Piilottaa pelaajan mallin
        }
    }

    //Pelaajan kuolema
    void NewRecordRecorded()
    {
        player.player.bestTime = Mathf.Round(elapsedTime *100.0f) / 100f;
    }
    void Overrun()
    {
        gameOver.GameOverSetup(Mathf.Round(elapsedTime * 100.0f) / 100f);
        activePickupUi.activeShootingPickup.SetActive(false);
        activePickupUi.activeUtilityPickup.SetActive(false);
        HidePlayerModel();
        if (lastRecordTime < elapsedTime)
        {
            NewRecordRecorded();
        }

        StartCoroutine(SlowTimeAndStop());

        
    }

    private IEnumerator SlowTimeAndStop()
    {
        float duration = 2.0f; // Hidastumisen kesto
        float startScale = Time.timeScale;
        float targetScale = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Käytä unscaledDeltaTimeä, jotta ajan hidastuminen ei vaikuta laskuriin
            Time.timeScale = Mathf.Lerp(startScale, targetScale, elapsed / duration);
            yield return null; // Odota seuraavaa framea
        }

        Time.timeScale = 0f; // Varmista että aika on täysin pysähtynyt
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
        //lopetetaan pickuppien tarkistaminen kun kaikki on pawnattu
        if (elapsedTime < 280)
        {
            //PickupActivation();
            pickupManager.ActivatePickup(elapsedTime);
            pickupManager.ActivateGodGun(elapsedTime);
        }
    }

    private void HandlePlayerDeath()
    {
        if (player.touched)
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
        pickupManager = transform.GetComponent<PickupManager>();
        activePickupUi = gameUI.GetComponent<SetPickupUIActive>();

        lastRecordTime = player.player.bestTime;
        pickupManager.SpawnPickups(GameManager.manager.chosenPickups, pickupPositions);
        pickupManager.SetGodGun();


    }
    private void Start()
    {

        //luodaan vihollisten poolit
        spawnManager.ResetVariables();
        
        spawnPointList = spawnManager.GetSpawnPoints();
        if (spawnPointList.Count ==0)
        {
            Debug.Log("No Spawnpoints!");
        }
        pools = spawnManager.GenerateSpawnPools();
        if (pools.Count == 0)
        {
            Debug.Log("No Pools!");
        }

        spawnManager.UpdateEnemyAmounts(elapsedTime);
        //StartCoroutine(spawnManager.SpawnEnemies(pools, spawnPointList));
        StartCoroutine(spawnManager.SpawnEnemies(pools, spawnPointList));
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

        spawnManager.UpdateEnemyAmounts(elapsedTime);
    }

}
