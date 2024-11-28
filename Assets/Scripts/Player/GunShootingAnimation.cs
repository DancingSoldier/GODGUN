using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class GunShootingAnimation : MonoBehaviour
{



    Shooting weapons;
    Transform animatableObject;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    void Start()
    {
        weapons = transform.parent.parent.parent.GetComponent<Shooting>();

        if ( weapons == null )
        {
            Debug.Log("No Weapons Script found for Gun Animation");
        }
        animatableObject = gameObject.transform.GetChild(1);

        initialPosition = animatableObject.localPosition;
        initialRotation = animatableObject.localRotation;

    }

    private void RestoreOriginalVectors()
    {
        animatableObject.localPosition = initialPosition;
        animatableObject.localRotation = initialRotation;
    }
    private void WeaponAnimationMiniGunSpin(float turnSpeed, bool online)
    {

        if (online)
        {
            animatableObject.Rotate(0, turnSpeed, 0);
        }
        else
        {
            RestoreOriginalVectors();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (weapons.usedLeftAttack && weapons.shooting)
        {
            
            WeaponAnimationMiniGunSpin(weapons.gunInUse.mainAttackConfig.roundsPerMin /60, weapons.shooting);
        }
        else if(weapons.shooting)
        {
            WeaponAnimationMiniGunSpin(weapons.gunInUse.altAttackConfig.roundsPerMin / 60, weapons.shooting);
        }

    }
}
