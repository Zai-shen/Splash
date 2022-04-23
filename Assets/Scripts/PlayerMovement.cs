using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum Direction
{
    LEFT = -1,
    RIGHT = 1
}

struct MoveDirection
{
    public Vector2 lastMove;

    public Direction GetDir()
    {
        return lastMove.x < 0 ? Direction.LEFT : Direction.RIGHT;
    }
}
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _jump;
    
    private MoveDirection _moveDirection;
    private InputAction _movement;
    private Vector2 _moveValue;
    private bool _isMoving;
    public float speed;
    
    private Rigidbody2D _rb;
    private Collider2D _coll;

    private bool _isGrounded = true;
    private bool _isJumping = false;
    public float quickJumpHeight = 5f;
    public float quickJumpWidth = 12f;
    public float quickJumpForce = 100f;
    public float chargedJumpHeight = 20f;
    public float chargedJumpWidth = 14f;
    public float chargedJumpForceIncrement = 100f;
    public float chargedJumpForceMaximum = 400f;
    private float chargeJumpForce = 0f;
    private bool doChargeJump = false;
    private Vector2 _preCollisionVelocity;
    public float wallBounceCoefficient = 80f;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _movement = _playerInput.Player.Move;
        _movement.Enable();

        _jump = _playerInput.Player.Jump;
        _jump.started += StartJump;
        _jump.canceled += PerformJump;
        _jump.Enable();
    }

    private void OnDisable()
    {
        _movement.Disable();
        _jump.Disable();
    }
    
    private void FixedUpdate()
    {
        CheckIsGrounded();

        if (Mathf.Abs(_rb.velocity.x) > 0.1f)
        {
            _preCollisionVelocity = _rb.velocity;
        }

        if (doChargeJump && chargeJumpForce != chargedJumpForceMaximum)
        {
            chargeJumpForce += chargedJumpForceIncrement * Time.fixedDeltaTime;
            chargeJumpForce = Mathf.Clamp(chargeJumpForce, 0f, chargedJumpForceMaximum);
        }
        
        if (!_isJumping && _isGrounded)
        {
            Move();
        }
    }

    private void Move()
    {
        _moveValue = _movement.ReadValue<Vector2>();
        
        if (_moveValue != Vector2.zero)
        {
            _isMoving = true;
            _rb.velocity = (_moveValue * speed * Time.fixedDeltaTime);
            _moveDirection.lastMove = _moveValue;
        }
        else
        {
            _isMoving = false;
        }
    }

    private void StartJump(InputAction.CallbackContext obj)
    {
        if (!_isGrounded)
            return;

        _isJumping = true;

        if (_isMoving)
        {
            Vector2 quickJump = new Vector2(quickJumpWidth * (float) _moveDirection.GetDir(), quickJumpHeight) * quickJumpForce;
            _rb.AddForce(quickJump);
        }
        else
        {
            doChargeJump = true;
        }
    }
    
    private void PerformJump(InputAction.CallbackContext obj)
    {
        if (!_isGrounded || _isMoving || !doChargeJump)
            return;

        Debug.Log($"Charged for: {chargeJumpForce} - jumping now!");
        
        Vector2 chargedJump = new Vector2(chargedJumpWidth * (float) _moveDirection.GetDir(), chargedJumpHeight) * chargeJumpForce;
        _rb.AddForce(chargedJump);

        chargeJumpForce = 0f;
        doChargeJump = false;
    }
    
    private void CheckIsGrounded()
    {
        float minX = _coll.bounds.min.x;
        float maxX = _coll.bounds.max.x;
        float dist = Mathf.Abs(maxX - minX);
        float cones = 4f;
        float distIteration = dist / cones;

        bool grounded = false;
        
        for (int i = 0; i <= cones; i++)
        {
            Vector2 rayPosition = new Vector2(_coll.bounds.min.x + (i * distIteration), _coll.bounds.min.y);

            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.down, 0.1f, ~(1 << 6));
            Debug.DrawRay( rayPosition, Vector2.down * 0.1f, Color.yellow);

            if (hit.collider != null)
            {
                grounded = true;
                break;
            }
        }

        _isGrounded = grounded;
        
        if (_isGrounded)
        {
            _isJumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!_isGrounded)
        {
            _rb.AddForce(new Vector2(_preCollisionVelocity.x * wallBounceCoefficient * -1f, 0));
            _preCollisionVelocity = Vector2.zero;
        }
    }
}
