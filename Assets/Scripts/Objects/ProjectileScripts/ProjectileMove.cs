
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

    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();

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
        passedThroughTriggers  = passedThroughTriggers + amount;

    }





    private void Destroyed()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject hitObject = collider.gameObject;

        if (hitObjects.Contains(hitObject))
        {
            // Jos kohde on jo osuttu, ei tehd‰ mit‰‰n
            return;
        }

        if (collider.CompareTag("Enemy"))
        {
            if(Random.Range(1,1) == 1)
            {
                GameObject impactEffect = Instantiate(hitPrefab, transform.position, Quaternion.identity);
                Destroy(impactEffect, 1f);
            }
           

            var enemyAI = hitObject.GetComponentInParent<EnemyAI>();
            Vector3 projectileVector = transform.forward;

            if (enemyAI != null)
            {
                PenetrationCount(1);
                enemyAI.TakeDamage(damage, damageType, hasKnockback, knockbackMultiplier, projectileVector);
            }
        }

        if (collider.CompareTag("Obelisk"))
        {
            GameObject impactEffect = Instantiate(obeliskHitPrefab, transform.position, Quaternion.identity);
            Destroy(impactEffect, impactEffect.GetComponent<ParticleSystem>().main.duration + impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constantMax);

            PenetrationCount(3);
           
        }

        // Lis‰‰ kohde osuttujen joukkoon
        hitObjects.Add(hitObject);

        // Tuhoa luoti, jos sen l‰p‰isyraja on saavutettu
        if (passedThroughTriggers >= penetration)
        {
            Destroyed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obelisk"))
        {
            GameObject impactEffect = Instantiate(obeliskHitPrefab, transform.position, Quaternion.identity);
            Destroy(impactEffect, impactEffect.GetComponent<ParticleSystem>().main.duration + impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constantMax);

            PenetrationCount(3);

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

