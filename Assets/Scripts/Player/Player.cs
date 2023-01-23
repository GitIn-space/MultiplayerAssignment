using Alteruna;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }

    // Customization

    // Movement
    [HideInInspector]
    public int xInput;
    public int yInput;
    public bool jumpInput;
    public Vector2 inputVector;
    public Vector2 moveDirection;
    public bool playerInputDisabled = false;
    [SerializeField] public float movementSpeed = 5;

    // Components
    [HideInInspector]
    public Multiplayer mp;
    public Rigidbody2DSynchronizable rb;
    public Alteruna.Avatar avatar;
    public SpriteRenderer renderer;

    // Others
    [HideInInspector]
    public bool isJumping = false;
    [SerializeField] private Transform childTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public int amountOfJumps = 1;
    [SerializeField] public float jumpVelocity = 5;

    [HideInInspector]
    public Vector2 currentVelocity { get; private set; }
    public int facingDirection { get; private set; }
    public Vector2 workspace;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
    }

    public void EnablePlayer(bool enable)
    {
        switch (enable)
        {
            case true:
                playerInputDisabled = false;
                break;
            case false:
                playerInputDisabled = true;
                renderer.color = Color.black;

                ProcedureParameters parms = new ProcedureParameters();
                parms.Set("team", (int)GetComponentInChildren<TeamSynch>().team);
                mp.InvokeRemoteProcedure("DeregisterPlayer", UserId.AllInclusive, parms);

                break;
        }
    }

    private void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        rb = GetComponent<Rigidbody2DSynchronizable>();
        mp = FindObjectOfType<Multiplayer>();

        facingDirection = 1;
        StateMachine.Init(IdleState);
    }

    private void Update()
    {
        if (!playerInputDisabled && avatar.IsMe)
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
        if (!playerInputDisabled)
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
        if (xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        facingDirection *= -1;
        Vector3 randomRotation = new Vector3(0, 180, 0);
        childTransform.Rotate(0.0f, 180f, 0.0f);
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