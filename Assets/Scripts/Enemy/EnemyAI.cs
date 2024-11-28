using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{

    public EnemyScriptableObject enemyScriptableObject;
    
    public int currentHealth;

    float flashtime = .15f;

    public List<Material> damageEffects;
    SkinnedMeshRenderer meshRenderer;
    Rigidbody rb;

    Material origMaterial;
    NavMeshAgent enemy;



    public void TakeDamage(int damage, DamageTypes damageType, Vector3 projectileVector)
    {

        StartCoroutine(DamageVFX(damageType));
        currentHealth -= damage;
        float forceMagnitude = damage * .5f;
        rb.AddForce(projectileVector * forceMagnitude, ForceMode.Impulse);
        //Debug.Log(gameObject.name + " Taken Damage " + damage);
        if (currentHealth <= 0)
        {
            Destroyed();

        }
    }



    public void Destroyed()
    {
        Destroy(gameObject);
    }

    public Vector3 FindPlayer()
    {

        return GameObject.FindGameObjectWithTag("Player").transform.position;

    }

    private IEnumerator FindPlayerInterval()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(enemyScriptableObject.destinationInterval);
            enemy.destination = FindPlayer();
        }
    }


    public IEnumerator DamageVFX(DamageTypes damage)
    {

        int effectIndex = (int)damage;
        if (effectIndex >= 0 && effectIndex < damageEffects.Count)
        {
            meshRenderer.material = damageEffects[effectIndex];
        }
        else
        {
            Debug.Log("Damage type doesnt exist");
        }
        yield return new WaitForSeconds(flashtime);
        meshRenderer.material = origMaterial;
    }


    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<NavMeshAgent>();

        currentHealth = enemyScriptableObject.health;
        enemy.speed = enemyScriptableObject.moveSpeed;
        enemy.acceleration = enemyScriptableObject.acceleration;
        enemy.angularSpeed = enemyScriptableObject.angularSpeed;
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rb = GetComponent<Rigidbody>();
        origMaterial = meshRenderer.material;

        
        StartCoroutine(FindPlayerInterval());
    }

}