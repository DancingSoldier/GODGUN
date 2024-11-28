using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    Collider killBox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().ReduceHealth();
        }
    }


    private void Start()
    {
        
        killBox = GetComponent<Collider>();
    }
}
