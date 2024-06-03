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

    // Add a queue to store input events
    Queue<Action> inputQueue = new Queue<Action>();

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
        if (GameManager.instance.State == GameState.GameOver)
            return;

        if (GameManager.instance.State == GameState.Paused)
        {
            IsThrowPressed = false;
            return;
        }

        // Add the input event to the queue
        inputQueue.Enqueue(() => isHoldingInput = true);
    }

    private void HoldInputCanceled(InputAction.CallbackContext context)
    {
        if (GameManager.instance.State == GameState.GameOver)
            return;

        if (GameManager.instance.State == GameState.Paused)
        {
            IsThrowPressed = false;
            return;
        }
            
        // Add the input event to the queue
        inputQueue.Enqueue(() =>
        {
            isHoldingInput = false;
            IsThrowPressed = true;
        });
    }

    private void Update()
    {
        // Process one input event per frame
        if (inputQueue.Count > 0)
        {
            inputQueue.Dequeue().Invoke();
        }
    }

}
