using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    //private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int moveXHash = Animator.StringToHash("MoveX");
    private readonly int moveYHash = Animator.StringToHash("MoveY");
    private readonly int moveBlendTreeHash = Animator.StringToHash("MoveBlendTree");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.1f;

    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        playerStateMachine.velocity.y = Physics.gravity.y;
        playerStateMachine.animator.CrossFadeInFixedTime(moveBlendTreeHash,crossFadeDuration);
        playerStateMachine.inputReader.OnJumpPerformed += SwitchToJumpState;
        playerStateMachine.inputReader.OnAttackPerformed += SwitchToAttack1State;
    }

    public override void Tick()
    {
        if (!playerStateMachine.characterController.isGrounded){
            playerStateMachine.SwitchState(new PlayerFallState(playerStateMachine));
        }
        CalculateMoveDirection();
        //FaceMoveDirection();
        Move();
        //playerStateMachine.animator.SetFloat(moveSpeedHash, playerStateMachine.inputReader.moveComposite.sqrMagnitude > 0f ? 1f : 0f, animationDampTime,Time.deltaTime);
        
        playerStateMachine.animator.SetFloat(moveXHash,playerStateMachine.inputReader.moveComposite.x, animationDampTime, Time.deltaTime);
        playerStateMachine.animator.SetFloat(moveYHash,playerStateMachine.inputReader.moveComposite.y, animationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
        playerStateMachine.inputReader.OnJumpPerformed -= SwitchToJumpState;
        playerStateMachine.inputReader.OnAttackPerformed -= SwitchToAttack1State;
    }

    private void SwitchToJumpState(){
        playerStateMachine.SwitchState(new PlayerJumpState(playerStateMachine));
    }

    private void SwitchToAttack1State(){
        playerStateMachine.SwitchState(new PlayerAttack1State(playerStateMachine));
    }
}
