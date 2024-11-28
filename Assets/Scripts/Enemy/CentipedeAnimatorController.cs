using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class CentipedeAnimatorController : MonoBehaviour
{

    public Rig rig;
    public RigBuilder rigBuilder;


    private IEnumerator PreventSpawnFuckery()
    {
        rig.weight = 0.0f;
        rigBuilder.Build();
        yield return new WaitForSecondsRealtime(1f);

        rig.weight = 1.0f;
    }

    private void Awake()
    {
        rigBuilder = GetComponentInChildren<RigBuilder>();
        rig.weight = 0f;

        
    }
    // Update is called once per frame
    private void Start()
    {
            StartCoroutine(PreventSpawnFuckery());
    }
}
