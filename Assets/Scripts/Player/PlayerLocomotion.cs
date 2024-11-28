using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class PlayerLocomotion : MonoBehaviour
{

    PlayerManager playerManager;
    InputManager inputManager;
    Shooting weapons;


    Vector3 targetDirection;
    Vector3 targetVelocity;

    Transform cameraObject;
    Camera mainCam;
    public GameObject cursorTarget;
    Rigidbody playerRigidbody;


    MultiAimConstraint gunAimConstraint;
    MultiAimConstraint headAimConstraint;
    MultiAimConstraint bodyAimConstraint;
    GameObject[] targetArray;
    RigBuilder rigs;
    public LayerMask groundMask;


    public Vector3 mousePos;


    // nämä kaksi funktiota voisivat asettaa playerille source objektit, mutta koodit eivät jostain syystä toimi tälläö hetkellä, joten pitää asettaa manuaalisesti.
    private void SetWeightedTransform(MultiAimConstraint aimConstraint, Transform target)
    {
        var data = aimConstraint.data.sourceObjects;
        data.Clear(); // Poista vanhat kohteet
        data.Add(new WeightedTransform(target, 1.0f)); // Lisää uusi kohde painolla 1
        aimConstraint.data.sourceObjects = data; // Päivitä data
    }


    
    private void SetAimConstraints()    //asettaa constraintit jos voi
    {
        if (gunAimConstraint != null)
        {
            SetWeightedTransform(gunAimConstraint, cursorTarget.transform);
        }
        if (headAimConstraint != null)
        {
            SetWeightedTransform(headAimConstraint, cursorTarget.transform);
        }
        if (bodyAimConstraint != null)
        {
            SetWeightedTransform(bodyAimConstraint, cursorTarget.transform);
        }

        rigs.Build();
    }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        mainCam = cameraObject.GetComponent<Camera>();
        weapons = GetComponent<Shooting>();

        cursorTarget = GameObject.FindWithTag("CursorTarget");
        targetArray = GameObject.FindGameObjectsWithTag("CursorTarget");
        // etsitään MultiAimConstraint-komponentit
        gunAimConstraint = GameObject.FindWithTag("PlayerGunRig").GetComponent<MultiAimConstraint>();
        headAimConstraint = GameObject.FindWithTag("PlayerHeadRig").GetComponent<MultiAimConstraint>();
        bodyAimConstraint = GameObject.FindWithTag("PlayerBodyRig").GetComponent<MultiAimConstraint>();

        rigs = GetComponent<RigBuilder>();

        if (gunAimConstraint == null || headAimConstraint == null || bodyAimConstraint == null)
        {
            Debug.LogError("Tarkista MultiAimConstraintit.");
        }

        SetAimConstraints();

    }
    private void HandleMovement()
    {
        float movementEffector = 1;


        float horizontal = inputManager.horizontalInput;
        float vertical = inputManager.verticalInput;
        if (weapons.gunInUse.usedMainAttack && weapons.shooting)
        {
            movementEffector = weapons.gunInUse.mainAttackConfig.playerMovementEffect;
        }
        else if (!weapons.gunInUse.usedMainAttack && weapons.shooting)
        {
            movementEffector = weapons.gunInUse.altAttackConfig.playerMovementEffect;
        }

        targetDirection = new Vector3 (horizontal, 0, vertical).normalized;

        targetDirection = targetDirection * playerManager.player.moveSpeed * movementEffector;


        playerRigidbody.AddForce(new Vector3(targetDirection.x,0, targetDirection.z));


    }


    public Vector3 GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask);

        mousePos = hitInfo.point;
        cursorTarget.transform.position = new Vector3(hitInfo.point.x, 1f, hitInfo.point.z);

        return mousePos;

    }
    public void PlayerAim()
    {
        Vector3 mousePos = GetMousePosition();

        Vector3 lookDir = mousePos - transform.position;

        lookDir.y = 0;

        float angleToMouse = Vector3.Angle(transform.forward, lookDir);


        if (angleToMouse > playerManager.player.turnThreshold)
        {


            Quaternion targetRotation = Quaternion.LookRotation(lookDir);

            // Pyöritä kohti targetRotationia tasaiseen tahtiin
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerManager.player.turnSpeed);

        }



    }

    public void HandleAllMovement()
    {
        HandleMovement();
        GetMousePosition();
        PlayerAim();
    }
}
