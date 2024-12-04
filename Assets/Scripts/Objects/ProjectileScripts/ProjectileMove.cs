
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{

    public GameObject hitPrefab;
    public GameObject obeliskHitPrefab;
    public float projectileSpeed = 10;
    public float projectileLifeTime = 1;
    public int damage, penetration;
    public DamageTypes damageType;
    private int passedThroughTriggers = 0;
    public bool hasKnockback;
    public float knockbackMultiplier;
    public void MoveProjectile()
    {
        if (projectileSpeed != 0)
        {
            transform.Translate(Vector3.forward * (projectileSpeed * Time.deltaTime));
        }
        else
        {
            Debug.Log("Projectile Speed is 0.");
        }
    }

    private void PenetrationCount(int amount)
    {
        passedThroughTriggers += amount;

    }





    private void Destroyed()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {

            GameObject impactEffect = Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(impactEffect, 1f);
            PenetrationCount(1);
            var enemyAI = collider.gameObject.GetComponentInParent<EnemyAI>();
            Vector3 projectileVector = transform.forward;
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage, damageType, hasKnockback, knockbackMultiplier, projectileVector);
            }
            if (passedThroughTriggers >= penetration)
            {
                Destroyed();
            }
        }
        if(collider.CompareTag("Obelisk") && passedThroughTriggers < 3)
        {
            GameObject impactEffect = Instantiate(obeliskHitPrefab, transform.position, Quaternion.identity);
            Destroy(impactEffect, 1f);
            PenetrationCount(2);
            Debug.Log("Hit Obelisk");
            if (passedThroughTriggers >= penetration)
            {
                Destroyed();
            }
        }
        else
        {
            Destroyed();
        }
    }

    private IEnumerator ProjectileLifeTime()
    {
        //MuzzleEffect(); // Play the muzzle effect when the projectile is created
        yield return new WaitForSeconds(projectileLifeTime);
        Destroyed();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ProjectileLifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }
}

