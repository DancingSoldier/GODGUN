using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Analytics;

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
    [HideInInspector]
    public static ArenaManager manager;
    [HideInInspector]
    PickupTimerIcons timerIcons;
    public List<GameObject> spawnPointList;

    [Header("Pickup Slots")]
    public List<Transform> pickupPositions;

    //timer check at the beginning
    bool timerSetup = false;

    public EnemySpawnerScriptableObject spawnManager;

    //Check if Saved
    bool saved = false;

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
    public bool fog = true;


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
        GameManager.manager.UpdateRecord(Mathf.Round(elapsedTime * 100.0f) / 100f);
        
    }
    void Overrun()
    {
        gameOver.GameOverSetup(Mathf.Round(elapsedTime * 100.0f) / 100f);
        activePickupUi.activeShootingPickup.SetActive(false);
        activePickupUi.activeUtilityPickup.SetActive(false);
        HidePlayerModel();
        
        StartCoroutine(SlowTimeAndStop());
        
        
        if (lastRecordTime < elapsedTime)
        {
            NewRecordRecorded();
            
        }
        

    }

    private IEnumerator SlowTimeAndStop()
    {
        float duration = 2.0f; // Hidastumisen kesto
        float startScale = Time.timeScale;
        float targetScale = 0f;
        float elapsed = 0f;
        if(gameOver.saveIcon != null)
        {
            gameOver.saveIcon.SetActive(true);
            gameOver.animator.PlayInFixedTime("SavingAnimation");
        }
        

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // K�yt� unscaledDeltaTime�, jotta ajan hidastuminen ei vaikuta laskuriin
            Time.timeScale = Mathf.Lerp(startScale, targetScale, elapsed / duration);
            
            yield return null; // Odota seuraavaa framea
        }

        Time.timeScale = 0f; // Varmista ett� aika on t�ysin pys�htynyt
        
        if (!saved)
        {
            GameManager.manager.SaveData();
            saved = true;
            Debug.Log("Saved, sending data");
            float flooredTime = Mathf.Round(elapsedTime * 100f) * 0.01f;
            AnalyticsManager.Instance.TimeRecorded(flooredTime);
            AnalyticsManager.Instance.PlayerDeath();
            if (gameOver.saveIcon != null)
            {
                Destroy(gameOver.saveIcon);
            }
            
        }
        
 

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
            timerIcons.OnPickupTaken(player.activeShootingPickup.pickupName, player.activeShootingPickup);
            Debug.Log(player.activeShootingPickup.pickupName);
        }
        if (player.activeUtilityPickup != null && !activePickupUi.utilityUIActive)
        {
            activePickupUi.StartCoroutine(activePickupUi.SetUtilityPickupTextActive(
                    player.activeUtilityPickup.pickupDuration,
                    player.activeUtilityPickup.pickupName,
                    player.activeUtilityPickup.pickupTextColor
            ));
            timerIcons.OnPickupTaken(player.activeUtilityPickup.pickupName, player.activeUtilityPickup);

        }
        //lopetetaan pickuppien tarkistaminen kun kaikki on pawnattu
        if (elapsedTime < 600)
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

    public void FogToggle()
    {
        GameObject[] fogs = GameObject.FindGameObjectsWithTag("Fog");

        fog = !fog;
        foreach (GameObject f in fogs)
        {
            if(fogs.Length != 0)
            {
                f.SetActive(!fog);
            }
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
        timerIcons = gameUI.GetComponent<PickupTimerIcons>();
        manager = this;



    }
    private void Start()
    {
        lastRecordTime = GameManager.manager.LastRecord();
        pickupManager.SpawnPickups(GameManager.manager.chosenPickups, pickupPositions);
        pickupManager.SetGodGun();
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
        Cursor.visible = true;

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
