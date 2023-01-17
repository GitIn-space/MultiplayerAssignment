using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Alteruna.Trinity;
using TMPro;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }

    // Customization

    // Movement
    public int xInput;
    public int yInput;
    public bool jumpInput;
    public Vector2 inputVector;
    public Vector2 moveDirection;
    public float movementSpeed = 5;
    public bool playerInputDisabled = false;

    // Components
    [SerializeField] private Multiplayer mp;
    public Rigidbody2D rb;
    private Alteruna.Avatar avatar;
    private SpriteRenderer renderer;

    // Others
    [SerializeField] private Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    public Vector2 currentVelocity { get; private set; }
    public int facingDirection { get; private set; }
    public Vector2 workspace;
    public int amountOfJumps = 1;
    public float jumpVelocity = 5;
    public bool isJumping = false;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
    }

    private void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        mp = FindObjectOfType<Multiplayer>();

        facingDirection = 1;
        StateMachine.Init(IdleState);
    }

    private void Update()
    {
        if(!playerInputDisabled && avatar.IsMe)
        {
            GetPlayerInput();
            currentVelocity = rb.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void GetPlayerInput()
    {
        if(!playerInputDisabled)
        {
            xInput = (int)Input.GetAxisRaw("Horizontal");

            if (xInput != 0)
            {
                // movementSpeed = 5.0f; 
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                jumpInput = true;
            }
        }
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, currentVelocity.y);
        rb.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(currentVelocity.x, velocity);
        rb.velocity = workspace;
        currentVelocity = workspace;
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180f, 0.0f);
    }

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    public void SetJumpInputToFalse()
    {
        jumpInput = false;
    }
} 