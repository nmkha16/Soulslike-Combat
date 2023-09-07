using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerAttack2State : PlayerBaseState
    {
        private readonly int attack2Hash = Animator.StringToHash("Slash2");
        private const float crossFadeDuration = .25f;
        private AttackSequence attackSequence = AttackSequence.Attack2;
        private float easingCurve = 5f;
        private float recommendSpeed = 1.45f;
        private float animLength;
        private AnimationCurve curve;
        private float elapsed = 0f;
        private bool shouldEnterNextAttack;    
        private float percentTimeOfStartHitbox, percentTimeOfEndHitbox;
        private float startHitbox,endHitbox;
        public PlayerAttack2State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.inputReader.OnAttackPerformed += EnterNextAttackSequence;

            playerStateMachine.animator.CrossFadeInFixedTime(attack2Hash,crossFadeDuration);
            playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
            animLength = playerStateMachine.animationAnimationClips[(int)attackSequence].anim.length / recommendSpeed;
            curve = playerStateMachine.animationAnimationClips[(int)attackSequence].curve;

            percentTimeOfStartHitbox = playerStateMachine.animationAnimationClips[(int)attackSequence].percentTimeOfStartHitbox;
            percentTimeOfEndHitbox = playerStateMachine.animationAnimationClips[(int)attackSequence].percentTimeOfEndHitbox;

            startHitbox = animLength * percentTimeOfStartHitbox;
            endHitbox = animLength * percentTimeOfEndHitbox;
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;

            if (elapsed >= startHitbox && elapsed <= endHitbox){
                playerStateMachine.ToggleWeaponHitbox(true);
            }
            else {
                playerStateMachine.ToggleWeaponHitbox(false);
            }

            CalculateMoveDirection(elapsed, curve, easing: easingCurve);
            FaceMoveDirection();
            Move();

            if (shouldEnterNextAttack && elapsed > animLength * 0.75f){
                playerStateMachine.SwitchState(new PlayerAttack3State(playerStateMachine));
                return;
            }

            if (elapsed > animLength * 0.9f){
                if (playerStateMachine.inputReader.isLockedOnTarget){
                    playerStateMachine.SwitchState(new PlayerLockOnState(playerStateMachine));
                    return;
                }
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
}