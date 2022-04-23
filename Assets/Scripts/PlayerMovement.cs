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
    private InputAction _movement;
    private Vector2 _moveValue;

    private Rigidbody2D _rb;
    private Collider2D _coll;

    private bool _isGrounded = true;
    private bool _isJumping = false;
    public float speed;
    public float jumpHeight;
    public float jumpWidth;
    private float jumpForce = 100f;

    private MoveDirection _moveDirection;

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
        CheckIsGrounded();
        
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
            _rb.velocity = (_moveValue * speed * Time.fixedDeltaTime);
            _moveDirection.lastMove = _moveValue;
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (!_isGrounded)
            return;

        _isJumping = true;

        Vector2 quickJump = new Vector2(jumpWidth * (int) _moveDirection.GetDir(), jumpHeight) * jumpForce;
        _rb.AddForce(quickJump);
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
}
