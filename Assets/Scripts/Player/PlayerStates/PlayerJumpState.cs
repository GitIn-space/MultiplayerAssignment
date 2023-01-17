using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private int amountOfJumpsLeft;

    private bool isGrounded;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        amountOfJumpsLeft = player.amountOfJumps;
        Debug.Log(amountOfJumpsLeft);
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ENTER JUMP");

        player.SetVelocityY(player.jumpVelocity);

        player.isJumping = true;

        amountOfJumpsLeft--;
    }

    public override void Exit()
    {
        base.Exit();

        player.isJumping = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isGrounded && player.currentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            player.CheckIfShouldFlip(player.xInput);
            player.SetVelocityX(5.0f * player.xInput);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        isGrounded = player.CheckIfGrounded();

    }

    public bool CanJump() => amountOfJumpsLeft > 0;
    public void ResetJumps() => amountOfJumpsLeft = player.amountOfJumps;
}
