using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2State : PlayerBaseState
{
    private readonly int attack2Hash = Animator.StringToHash("Slash2");
    private const float crossFadeDuration = .25f;
    private AttackSequence attackSequence = AttackSequence.Attack2;
    private float recommendSpeed = 1.25f;
    private float animLength;
    private float elapsed = 0f;
    private bool shouldEnterNextAttack;

    public PlayerAttack2State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.inputReader.OnAttackPerformed += EnterNextAttackSequence;

        playerStateMachine.animator.CrossFadeInFixedTime(attack2Hash,crossFadeDuration);
        playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
        animLength = playerStateMachine.animationClips[(int)attackSequence].length / recommendSpeed;
    }

    public override void Tick()
    {
        elapsed += Time.deltaTime;

        if (shouldEnterNextAttack && elapsed > animLength * 0.75f){
            playerStateMachine.SwitchState(new PlayerAttack3State(playerStateMachine));
            return;
        }

        if (elapsed > animLength){
            playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
        }

    }

    public override void Exit()
    {
        playerStateMachine.animator.SetFloat(animMultiplier,1f);
        playerStateMachine.inputReader.OnAttackPerformed -= EnterNextAttackSequence;
    }

    /// <summary>
    /// If user send attack input within 1/2 length of the attack animation, enter next attack sequence on end animation
    /// </summary>
    private void EnterNextAttackSequence(){
        if (elapsed > animLength/2){
            shouldEnterNextAttack = true;
        }
    }

}
