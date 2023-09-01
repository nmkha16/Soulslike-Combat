using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1State : PlayerBaseState
{
    private readonly int attack1Hash = Animator.StringToHash("Slash1");
    private const float crossFadeDuration = .25f;
    private AttackSequence attackSequence = AttackSequence.Attack1;
    private float recommendSpeed = 1.25f;
    private float animLength;
    private float elapsed = 0f;
    private bool shouldEnterNextAttack;
    public PlayerAttack1State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.inputReader.OnAttackPerformed += EnterNextAttackSequence;

        playerStateMachine.animator.CrossFadeInFixedTime(attack1Hash,crossFadeDuration);
        playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
        animLength = playerStateMachine.animationClips[(int)attackSequence].length / recommendSpeed;
    }

    public override void Tick()
    {
        elapsed += Time.deltaTime;

        if (shouldEnterNextAttack && elapsed > animLength * 0.75f){
            playerStateMachine.SwitchState(new PlayerAttack2State(playerStateMachine));
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
    /// if user send attack input within 1/2 length of the attack animation, enter next attack sequence on end animation
    /// </summary>
    private void EnterNextAttackSequence(){
        if (elapsed > animLength/2){
            shouldEnterNextAttack = true;
        }
    }
}
