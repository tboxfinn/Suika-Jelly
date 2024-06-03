using System;
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
    public Vector2 movement2Input;
    public bool isHoldingInput = false;
    public bool IsThrowPressed = false;

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
        playerController = FindFirstObjectByType<PlayerController>();
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

            playerControls.PlayerMovement.Movement.performed += i => movement2Input = i.ReadValue<Vector2>();

            playerControls.PlayerActions.PressHold.performed +=  HoldInputPerfomed;
            playerControls.PlayerActions.PressHold.canceled += HoldInputCanceled;

        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    
    private void HoldInputPerfomed(InputAction.CallbackContext context)
    {
        isHoldingInput = true;
    }

    private void HoldInputCanceled(InputAction.CallbackContext context)
    {
        isHoldingInput = false;
        
        IsThrowPressed = true;
    }

}
