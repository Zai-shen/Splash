using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _jump;
    private InputAction _movement;
    private Vector2 _moveValue;

    private Rigidbody2D _rb;

    private bool _jumping;
    public float speed;
    public float jumpHeight;
    public float jumpWidth;
    private float jumpForce = 100f;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _movement = _playerInput.Player.Move;
        _movement.Enable();

        _jump = _playerInput.Player.Jump;
        _jump.performed += Jump;
        _jump.Enable();
    }

    private void OnDisable()
    {
        _movement.Disable();
        _jump.Disable();
    }

    private void FixedUpdate()
    {
        if (!_jumping)
        {
            Move();
        }
    }

    private void Move()
    {
        _moveValue = _movement.ReadValue<Vector2>();
        
        if (_moveValue != Vector2.zero)
        {
            _rb.velocity = (_moveValue * speed * Time.fixedDeltaTime);
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        _jumping = true;    
        Debug.Log($"jumping! with {obj}");
        _rb.AddForce(new Vector2(jumpWidth, jumpHeight) * jumpForce);
    }

    //TODOOOOOOOOO
    private void OnCollisionEnter2D(Collision2D col)
    {
        _jumping = false;
    }
}
