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

    // Customization

    // Movement
    public int xInput;
    public int yInput;
    public Vector2 inputVector;
    public Vector2 moveDirection;
    public float movementSpeed;
    public bool playerInputDisabled = false;

    // Components
    [SerializeField] private Multiplayer mp;
    public Rigidbody2D rb;
    private Alteruna.Avatar avatar;
    private SpriteRenderer renderer;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
    }

    private void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        mp = FindObjectOfType<Multiplayer>();

        StateMachine.Init(IdleState);
    }

    private void Update()
    {
        if(!playerInputDisabled && avatar.IsMe)
        {
            GetPlayerInput();
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
                movementSpeed = 5.0f; 
            }
        }
    }
}