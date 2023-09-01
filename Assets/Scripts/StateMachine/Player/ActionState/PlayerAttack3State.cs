using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack3State : PlayerBaseState
{
    private readonly int attack3Hash = Animator.StringToHash("Slash3");
    private const float crossFadeDuration = .25f;
    private AttackSequence attackSequence = AttackSequence.Attack3;
    private float recommendSpeed = 1.25f;
    private float animLength;
    private float elapsed = 0f;
    public PlayerAttack3State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.animator.CrossFadeInFixedTime(attack3Hash,crossFadeDuration);
        playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
        animLength = playerStateMachine.animationClips[(int)attackSequence].length / recommendSpeed;
    }

    public override void Tick()
    {
        elapsed += Time.deltaTime;

        if (elapsed > animLength){
            playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
        }

    }

    public override void Exit()
    {
        playerStateMachine.animator.SetFloat(animMultiplier,1f);
    }

}
