using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerRunningAttackState : PlayerBaseState
    {
        private readonly int heavyAttack = Animator.StringToHash("Running Attack");
        private const float crossFadeDuration = .25f;
        private AttackSequence attackSequence = AttackSequence.Running_Attack;
        private float easingCurve = 5f;
        private float recommendSpeed = 1.35f;
        private float animLength;
        private AnimationCurve curve;
        private float elapsed = 0f;
        public PlayerRunningAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.animator.CrossFadeInFixedTime(heavyAttack,crossFadeDuration);
            playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
            animLength = playerStateMachine.animationAnimationClips[(int)attackSequence].anim.length / recommendSpeed;
            curve = playerStateMachine.animationAnimationClips[(int)attackSequence].curve;
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;

            // push player forward on last string of attack
            CalculateMoveDirection(elapsed, curve, easing: easingCurve);
            FaceMoveDirection();
            Move();

            if (elapsed > animLength){
                playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
            }
        }

        public override void Exit()
        {
            playerStateMachine.animator.SetFloat(animMultiplier,1f);
        }
    }

}
