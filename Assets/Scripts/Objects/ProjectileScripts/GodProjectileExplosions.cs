using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodProjectileExplosions : MonoBehaviour
{
    public GameObject explosionPrefab;


    private void TriggerExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Destroy(explosion, .5f);
    }


    void OnTriggerEnter(Collider other)
    {
        TriggerExplosion();
    }

    void OnCollisionEnter(Collision collision)
    {
        TriggerExplosion();
    }

}
