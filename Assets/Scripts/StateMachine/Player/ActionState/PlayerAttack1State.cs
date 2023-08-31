using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1State : PlayerBaseState
{
    protected readonly int attack1Hash = Animator.StringToHash("Slash1");
    private const float crossFadeDuration = .1f;
    private AttackSequence attackSequence = AttackSequence.Attack1;
    private float recommendSpeed = 1.25f;
    private float animLength;
    private float elapsed = 0f;
    public PlayerAttack1State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.animator.CrossFadeInFixedTime(attack1Hash,crossFadeDuration);
        playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
        animLength = playerStateMachine.animationClips[(int)attackSequence].length / recommendSpeed;
    }

    public override void Tick()
    {
        elapsed += Time.deltaTime;
        //ApplyGravity();

        if (elapsed > animLength){
            playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
        }

        //FaceMoveDirection();
        //Move();
    }

    public override void Exit()
    {
        playerStateMachine.animator.SetFloat(animMultiplier,1f);
    }

}
