using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class ExpressionGhostAnimator : MonoBehaviour
{

    Animator animator;

    private float expressionValue;






    



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.enabled = true;
        expressionValue = Random.Range(0, 0.6f);


    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Expression", expressionValue);
    }
}
