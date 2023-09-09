using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{

    public class PlayerAttack3State : PlayerBaseState
    {
        private readonly int attack3Hash = Animator.StringToHash("Slash3");
        private const float crossFadeDuration = .25f;
        private AttackSequence attackSequence = AttackSequence.Attack3;
        private float easingCurve = 3.75f;
        private float recommendSpeed = 1.35f;
        private float animLength;
        private AnimationCurve curve;
        private float elapsed = 0f;
        private float percentTimeOfStartHitbox, percentTimeOfEndHitbox;
        private float startHitbox,endHitbox;
        public PlayerAttack3State(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {            
            playerStateMachine.animator.CrossFadeInFixedTime(attack3Hash,crossFadeDuration);
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
            CalculateMoveDirection(elapsed,curve, easing: easingCurve);
            FaceTargetDirectionImmediately();
            Move();

            if (elapsed > animLength){
                if (playerStateMachine.isLockedOnTarget){
                    SwitchToLockOnState();
                    return;
                }
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
