using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public PlayerController playerController;

    PlayerControls playerControls;

    [Header("Player Movement Inputs")]
    public Vector2 movementInput;
    public bool holdInput = false;
    public bool isThrowPressed = false;
    public Vector2 _initialTouchPosition;
    public Vector2 _currentTouchPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (playerControls != null)
        {
            playerControls.Enable();
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Move.performed += i => movementInput = i.ReadValue<Vector2>();

            //HOLDS
        // Holding the button
        playerControls.PlayerActions.PressHold.performed += i => 
        {
            holdInput = true;
            _initialTouchPosition = Pointer.current.position.ReadValue();
            ThrowFruitController.instance.CanThrow = false;
        };
        // Releasing the button
        playerControls.PlayerActions.PressHold.canceled += i => 
        {
            holdInput = false;
            isThrowPressed = true;
            _currentTouchPosition = Pointer.current.position.ReadValue();
            ThrowFruitController.instance.CanThrow = true;
        };

        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandleHoldInput();
    }

    private void HandleHoldInput()
    {
        if (holdInput)
        {
            playerController.HandleMovement();
        }
    }

}
