using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    protected readonly int fallHash = Animator.StringToHash("Fall");
    private const float crossFadeDuration = .1f;
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.velocity.y = 0f;
        playerStateMachine.animator.CrossFadeInFixedTime(fallHash, crossFadeDuration);
    }

    public override void Tick()
    {
        ApplyGravity();
        Move();

        if (playerStateMachine.characterController.isGrounded){
            playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
        }
    }

    public override void Exit()
    {
        
    }

}
