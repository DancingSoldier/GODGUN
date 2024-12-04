using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MADBombExplosion : MonoBehaviour
{

    
    public Vector3 targetScale = new Vector3(100f, 100f, 100f);
    
    public int damage;
    public DamageTypes damageType;
    public AnimationCurve scaleCurve;
    Vector3 position;
    public float duration = 1;
    Transform explosion;
    public GameObject impactEffectEnemy;
    public GameObject impactEffectObelisk;

    AudioSource audioSource;
    private IEnumerator Explosion()
    {
        Vector3 startScale = explosion.localScale;  // Alkuperäinen skaala
        float elapsedTime = 0f;                        // Aika kulunut
        
        while (elapsedTime < duration)
        {

            float curveValue = scaleCurve.Evaluate(elapsedTime / duration);
            explosion.localScale = Vector3.LerpUnclamped(startScale, targetScale, curveValue);

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
            GameObject impactEffect = Instantiate(impactEffectEnemy, other.transform.position, Quaternion.identity);

            var enemyAI = other.gameObject.GetComponentInParent<EnemyAI>();
            damage = Mathf.FloorToInt(damage);
            enemyAI.TakeDamage(damage, damageType, false, 0, other.gameObject.transform.position);
            Destroy(impactEffect, 2f);
        }
        if (other.CompareTag("Obelisk"))
        {
            GameObject impactEffect = Instantiate(impactEffectObelisk, other.transform.position, Quaternion.identity);

            Destroy(impactEffect, 2f);
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        explosion = transform.GetComponent<Transform>();

        position = transform.position;

    }

    private void Start()
    {
        audioSource.PlayDelayed(.5f);
        StartCoroutine(Explosion());
    }

}

