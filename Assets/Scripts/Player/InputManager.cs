using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    Shooting weapons;
    PlayerManager playerManager;
    public GameObject settingsMenu;
    public Rig thisRig;
    public float animationChangeSpeed;

    public Vector2 movementInput;
    private float moveAmount;
    public float clickValueLeft;
    public float clickValueRight;
    public float horizontalInput;
    public float verticalInput;

    bool paused = false; // Hallinnoi pelin pause-tilaa

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

            // Bindataan tauotusnappiin
            playerControls.PlayerMovement.Pause.performed += _ => TogglePause();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        if (!playerManager.touched && !playerManager.overrun)
        {
            HandleMovementInput();
            HandleMouseClicks();
        }
        else if (playerManager.touched && !playerManager.overrun)
        {
            HandleMovementInput();
            HandleMouseClicks();
        }
        else if (playerManager.overrun)
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

    public void TogglePause()
    {
        GameObject pauseMenu = GameObject.FindGameObjectsWithTag("GameUI")[0].transform.GetChild(7).gameObject;
        
        if (!paused && !playerManager.touched)
        {
            Debug.Log("Game Paused");
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else if(!playerManager.touched)
        {
            settingsMenu = pauseMenu.transform.GetChild(2).gameObject;
            Debug.Log("Game Unpaused");
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
            }
        }
        
        
        
        paused = !paused; // Vaihdetaan paused-tila
    }

    private void HandleMouseClicks()
    {
        if (clickValueLeft == 1 && clickValueRight != 1 && !paused)
        {
            weapons.shooting = true;
            playerManager.gunBeingUsed.usedMainAttack = true;
            weapons.HandleShooting();
        }
        else if (clickValueLeft != 1 && clickValueRight == 1 && !paused)
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
