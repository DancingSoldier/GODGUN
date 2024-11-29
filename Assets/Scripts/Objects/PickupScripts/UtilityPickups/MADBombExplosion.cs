using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class MADBombExplosion : MonoBehaviour
{

    public Vector3 targetScale = new Vector3(100f, 100f, 100f);
    public float duration = 1f;
    public int damage;
    public DamageTypes damageType;
    Vector3 position;

    Transform explosion;

    private IEnumerator Explosion()
    {
        Vector3 startScale = explosion.localScale;  // Alkuperäinen skaala
        float elapsedTime = 0f;                        // Aika kulunut

        while (elapsedTime < duration)
        {

            explosion.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);

            // Lisää kulunut aika
            elapsedTime += Time.deltaTime;

            yield return null;  
        }


        explosion.localScale = targetScale;


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {

            var enemyAI = other.gameObject.GetComponentInParent<EnemyAI>();
            enemyAI.TakeDamage(damage, damageType, other.gameObject.transform.position - position);
        }
    }

    private void Awake()
    {
        explosion = transform.GetComponent<Transform>();

        position = transform.position;

    }

    private void Start()
    {
        StartCoroutine(Explosion());
    }

}

