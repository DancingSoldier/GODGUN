using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorManager : MonoBehaviour
{

    Animator animator;
    Shooting weapons;

    int horizontal;
    int vertical;

    public Rig thisRig;
    public float animationChangeSpeed;
  
    private void Awake()
    {
        animator = GetComponent<Animator>();
        weapons = GetComponent<Shooting>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        
    }



    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        //Debug.Log("Horizontal: " + horizontalMovement);
        animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
        //Debug.Log("Vertical: " + verticalMovement);




        float targetWeight = weapons.shooting ? 1f : 0f;


        thisRig.weight = Mathf.Lerp(thisRig.weight, targetWeight, animationChangeSpeed);




    }
}
