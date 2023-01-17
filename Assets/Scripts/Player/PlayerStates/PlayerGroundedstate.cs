using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedstate : PlayerState
{
    protected int xInput;
    private bool jumpInput;
    private bool isGrounded;

    public PlayerGroundedstate(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.xInput;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
