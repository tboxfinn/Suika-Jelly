using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private BoxCollider2D _boundaries;
    [SerializeField] private Transform _fruitThrowTransform;

    private Bounds _bounds;

    private float _leftBound;
    private float _rightBound;

    private float _startingLeftBound;
    private float _startingRightBound;

    private float _offset;

    private void Awake()
    {
        _bounds = _boundaries.bounds;

        _offset = transform.position.x - _fruitThrowTransform.position.x;
        
        _leftBound = _bounds.min.x + _offset;
        _rightBound = _bounds.max.x + _offset;

        _startingLeftBound = _leftBound;
        _startingRightBound = _rightBound;

    }

    private void Update()
    {
        if (GameManager.instance.State == GameState.GameOver)
            return;
            
        HandleMovement();

    }

    public void HandleMovement()
    {
        if (PlayerInputManager.instance.isHoldingInput)
        {
            Vector3 newPosition = transform.position + new Vector3(PlayerInputManager.instance.movementInput.x * _moveSpeed * Time.deltaTime, 0, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, _leftBound, _rightBound);

            transform.position = newPosition;
        }
        
    }

    public void ChangeBoundary(float extraWidth)
    {
        _leftBound = _startingLeftBound;
        _rightBound = _startingRightBound;

        _leftBound += ThrowFruitController.instance.Bounds.extents.x + extraWidth;
        _rightBound -= ThrowFruitController.instance.Bounds.extents.x + extraWidth;
    }
    
}
