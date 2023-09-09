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
            curve = playerStateMachine.attackAnimationClips[(int)attackSequence].curve;

            animLength = playerStateMachine.attackAnimationClips[(int)attackSequence].anim.length / recommendSpeed;

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
            }
            else{
                playerStateMachine.ToggleWeaponHitbox(false);
            }

            // note: might need to change to a better way, this is a kind of lazy fix
            // was trying to fix attack1 transits to riposte without activating the sound effect of attack1
            if (elapsed >= endHitbox-0.1f){
                OnPlaySoundOnce?.Invoke(SoundId.sfx_sword_whoosh);
            }
            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }


            CalculateMoveDirection(elapsed, curve, easing: easingCurve);
            FaceTargetDirectionImmediately();
            Move();

            if (shouldEnterNextAttack && elapsed > animLength * 0.75f){
                playerStateMachine.SwitchState(new PlayerAttack2State(playerStateMachine));
                return;
            }

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
            playerStateMachine.ToggleSwordSfx(false);
            playerStateMachine.ToggleWeaponHitbox(false);
            playerStateMachine.animator.SetFloat(animMultiplier,1f);
            playerStateMachine.ToggleWeaponHitbox(false);
            playerStateMachine.inputReader.OnAttackPerformed -= EnterNextAttackSequence;
            CleanPlaySoundEvent();
        }

        /// <summary>
        /// if user send attack input within 1/2 length of the attack animation, enter next attack sequence on end animation
        /// </summary>
        private void EnterNextAttackSequence(){
            if (elapsed > animLength/2){
                shouldEnterNextAttack = true;
            }
        }

        protected override void PlaySound(SoundId id){
            SoundManager.instance.PlayAudioWithRandomPitch(id);
            CleanPlaySoundEvent();
        }
    }

}
