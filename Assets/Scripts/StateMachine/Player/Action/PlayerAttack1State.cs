using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Action{
    public class PlayerAttack1State : PlayerBaseState
    {
        private readonly int attack1Hash = Animator.StringToHash("Slash1");
        private const float crossFadeDuration = .25f;
        private AttackSequence attackSequence = AttackSequence.Attack1;
        private float easingCurve = 5f;
        private float recommendSpeed = 1.40f;
        private float animLength;
        private AnimationCurve curve;
        private float elapsed = 0f;
        private bool shouldEnterNextAttack;

        private float percentTimeOfStartHitbox, percentTimeOfEndHitbox;
        private float startHitbox,endHitbox;

        public PlayerAttack1State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.inputReader.OnAttackPerformed += EnterNextAttackSequence;
            playerStateMachine.animator.CrossFadeInFixedTime(attack1Hash,crossFadeDuration);
            playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
            animLength = playerStateMachine.attackAnimationClips[(int)attackSequence].anim.length / recommendSpeed;
            curve = playerStateMachine.attackAnimationClips[(int)attackSequence].curve;

            percentTimeOfStartHitbox = playerStateMachine.attackAnimationClips[(int)attackSequence].percentTimeOfStartHitbox;
            percentTimeOfEndHitbox = playerStateMachine.attackAnimationClips[(int)attackSequence].percentTimeOfEndHitbox;

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

            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }


            CalculateMoveDirection(elapsed, curve, easing: easingCurve);
            FaceMoveDirection();
            Move();

            if (shouldEnterNextAttack && elapsed > animLength * 0.75f){
                playerStateMachine.SwitchState(new PlayerAttack2State(playerStateMachine));
                return;
            }

            if (elapsed > animLength * 0.9f){
                if (playerStateMachine.inputReader.isLockedOnTarget){
                    SwitchToLockOnState();
                    return;
                }
                SwitchToMoveState();
            }

        }

        public override void Exit()
        {
            playerStateMachine.animator.SetFloat(animMultiplier,1f);
            playerStateMachine.ToggleWeaponHitbox(false);
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

}
