using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MADBombExplosion : MonoBehaviour
{

    public UtilityPickupScriptableObject script;
    public Vector3 targetScale = new Vector3(100f, 100f, 100f);
    
    public int damage;
    public DamageTypes damageType;
    public AnimationCurve scaleCurve;
    Vector3 position;

    Transform explosion;

    private IEnumerator Explosion()
    {
        Vector3 startScale = explosion.localScale;  // Alkuperäinen skaala
        float elapsedTime = 0f;                        // Aika kulunut
        float duration = script.duration;
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

            var enemyAI = other.gameObject.GetComponentInParent<EnemyAI>();
            damage = Mathf.FloorToInt(script.effectFloat1);
            enemyAI.TakeDamage(damage, damageType, false, 0, other.gameObject.transform.position);
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

