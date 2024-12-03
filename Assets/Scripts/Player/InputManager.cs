using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    Shooting weapons;
    PlayerManager playerManager;

    public Rig thisRig;
    public float animationChangeSpeed;

    public Vector2 movementInput;
    private float moveAmount;
    public float clickValueLeft;
    public float clickValueRight;
    public float horizontalInput;
    public float verticalInput;



    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        weapons = GetComponent<Shooting>();
        playerManager = GetComponent<PlayerManager>();

    }
    private void OnEnable()
    {
       if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.ShootLeft.performed += i => clickValueLeft = i.ReadValue<float>();
            playerControls.PlayerMovement.ShootLeft.canceled += i => clickValueLeft = i.ReadValue<float>();
            playerControls.PlayerMovement.ShootRight.performed += i => clickValueRight = i.ReadValue<float>();
            playerControls.PlayerMovement.ShootRight.canceled += i => clickValueRight = i.ReadValue<float>();


        }

        playerControls.Enable();
    }


    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void HandleAllInputs()
    {
        if(!playerManager.touched && !playerManager.overrun)
        {
            HandleMovementInput();
            HandleMouseClicks();
            
        }
        else if(playerManager.touched && !playerManager.overrun)
        {
            HandleMovementInput();
            HandleMouseClicks();
        }
        else if(playerManager.overrun)
        {
            return;
        }

    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));


        animatorManager.UpdateAnimatorValues(0, moveAmount);

    }

    private void HandleMouseClicks()
    {
        if (clickValueLeft == 1 && clickValueRight != 1)
        {

            weapons.shooting = true;
            playerManager.gunBeingUsed.usedMainAttack = true;
            weapons.HandleShooting();

        }
        else if (clickValueLeft != 1 && clickValueRight == 1)
        {

            weapons.shooting = true;
            playerManager.gunBeingUsed.usedMainAttack = false;
            weapons.HandleShooting();

        }
        else
        {
            weapons.shooting = false;
        }

    }
}
