using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupTimer : MonoBehaviour
{

    ShootingPickupScriptableObject sScript;
    UtilityPickupScriptableObject uScript;
    public SpriteRenderer pickupAreaEffect;
    public Color color;
    public TextMeshPro text;
    public float cooldown = 0f;
    float timer;
    public float spawnTime = 0f;
    bool firstTimeTaken = false;
    bool taken;
    float elapsedTime = 0;
    public Collider pickupCheck;



    //korjaa cooldownin resetointi koodiin!


private void HandlePickup(Collider other)
{
    PlayerManager player = other.GetComponent<PlayerManager>();
        
    if (player != null && !player.pickupTaken) // Tarkista, että pelaaja voi ottaa pickupin
    {
       
        

        if (!firstTimeTaken)
        {

            firstTimeTaken = true;
            taken = true;
                timer = cooldown;
                StartCoroutine(ActivateCollider(cooldown)); // Deaktivoi collider
                Debug.Log($"FirstTimeTaken: {firstTimeTaken}, Taken: {taken} Player Taken: {player.pickupTaken}");
        }

            player.pickupTaken = true; // Päivitä tila heti
        }
}



    private IEnumerator ActivateCollider(float waitTime)
    {
        pickupCheck.enabled = false; // Deaktivoi collider
        
        yield return new WaitForSeconds(waitTime); // Odota cooldownin ajan
        pickupCheck.enabled = true; // Aktivoi collider uudelleen
        taken = false; // Resetoi tila seuraavaa ottoa varten
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePickup(other);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        pickupCheck = GetComponent<Collider>();
        pickupCheck.enabled = false;
        
        if (gameObject.CompareTag("GodGunPosition") && !GameManager.manager.godGunGained)
        {
            spawnTime = GameManager.manager.godGunSpawnTime;
            cooldown = GameManager.manager.godGunCooldown;
            color = Color.yellow;
        }
        else if (gameObject.CompareTag("GodGunPosition") && GameManager.manager.godGunGained)
        {
            spawnTime = GameManager.manager.godGunSecondSpawnTime;
            cooldown = GameManager.manager.godGunCooldown;
            color = Color.yellow;
        }

        pickupAreaEffect = transform.GetComponentInChildren<SpriteRenderer>();
        if (pickupAreaEffect != null)
        {
            pickupAreaEffect.color = color;
        }

        text.color = color;
        timer = cooldown;
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTime > 0)
        {
            // Ensimmäisen kerran spawn-timer
            spawnTime -= Time.deltaTime;
            text.text = Mathf.Ceil(spawnTime).ToString();
        }
        else
        {
            text.text = "";

        }
    }
}
