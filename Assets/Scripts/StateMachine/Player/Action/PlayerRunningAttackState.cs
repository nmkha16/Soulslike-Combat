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
        private float recommendSpeed = 1.2f;
        private float animLength;
        private AnimationCurve curve;
        private float elapsed = 0f;
        private float percentTimeOfStartHitbox, percentTimeOfEndHitbox;
        private float startHitbox,endHitbox;
        public PlayerRunningAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.animator.CrossFadeInFixedTime(heavyAttack,crossFadeDuration);
            playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
            animLength = playerStateMachine.attackAnimationClips[(int)attackSequence].anim.length / recommendSpeed;
            curve = playerStateMachine.attackAnimationClips[(int)attackSequence].curve;

            percentTimeOfStartHitbox = playerStateMachine.attackAnimationClips[(int)attackSequence].percentTimeOfStartHitbox;
            percentTimeOfEndHitbox = playerStateMachine.attackAnimationClips[(int)attackSequence].percentTimeOfEndHitbox;

            startHitbox = percentTimeOfStartHitbox / recommendSpeed;
            endHitbox = percentTimeOfEndHitbox / recommendSpeed;

            // enable sfx
            playerStateMachine.ToggleSwordSfx(true);
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;

            if (elapsed >= startHitbox && elapsed <= endHitbox){
                playerStateMachine.ToggleWeaponHitbox(true);
                OnPlaySoundOnce?.Invoke(SoundId.sfx_sword_fast_whoosh);
            }
            else {
                playerStateMachine.ToggleWeaponHitbox(false);
            }

            
            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }
            
            
            ApplyGravity();
            // push player forward on last string of attack
            CalculateMoveDirection(elapsed, curve, easing: easingCurve);
            FaceMoveDirection();
            Move();

            if (elapsed > animLength){
                SwitchToMoveState();
            }
        }

        public override void Exit()
        {
            // disable sword sfx
            CleanPlaySoundEvent();
            playerStateMachine.ToggleSwordSfx(false);

            playerStateMachine.ToggleWeaponHitbox(false);
            playerStateMachine.animator.SetFloat(animMultiplier,1f);
        }
    }

}
