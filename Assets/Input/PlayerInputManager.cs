using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    PlayerControls playerControls;

    [Header("Player Movement Inputs")]
    public Vector2 movementInput;
    public bool isHoldingInput = false;

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
            playerControls.PlayerMovement.Hold.performed += i => isHoldingInput = true;
            playerControls.PlayerMovement.Hold.canceled += i => isHoldingInput = false;
        }
    }
}
