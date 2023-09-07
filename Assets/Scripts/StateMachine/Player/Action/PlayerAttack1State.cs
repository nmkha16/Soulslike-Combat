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
        private float startHitbox,endHitbox;

        public PlayerAttack1State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.inputReader.OnAttackPerformed += EnterNextAttackSequence;

            playerStateMachine.animator.CrossFadeInFixedTime(attack1Hash,crossFadeDuration);
            playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
            animLength = playerStateMachine.animationClips[(int)attackSequence].anim.length / recommendSpeed;
            curve = playerStateMachine.animationClips[(int)attackSequence].curve;
            startHitbox = animLength * 0.41f;
            endHitbox = animLength * 0.5f;
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

            // push player forward on last string of attack
            CalculateMoveDirection(elapsed, curve, easing: easingCurve);
            FaceMoveDirection();
            Move();

            if (shouldEnterNextAttack && elapsed > animLength * 0.75f){
                playerStateMachine.SwitchState(new PlayerAttack2State(playerStateMachine));
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
