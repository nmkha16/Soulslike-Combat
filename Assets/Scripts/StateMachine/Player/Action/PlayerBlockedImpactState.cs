using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerBlockedImpactState : PlayerBaseState
    {
        private readonly int blockIdleHash = Animator.StringToHash("Block Idle");
        protected readonly int blockedImpactHash = Animator.StringToHash("Blocked Impact");
        private const float crossFadeDuration = .1f;
        private const float waitTime = 1.1f;
        private float elapsed = 0f;
        public PlayerBlockedImpactState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.velocity.x = 0;
            playerStateMachine.velocity.z = 0;
            playerStateMachine.animator.CrossFadeInFixedTime(blockedImpactHash, crossFadeDuration);
            OnPlaySoundOnce?.Invoke(SoundId.sfx_shield_hit);
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;

            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }

            FaceTargetDirection();

            if (elapsed > waitTime){
                SwitchToBlockState();
            }
        }

        public override void Exit()
        {
            CleanPlaySoundEvent();
            playerStateMachine.animator.CrossFadeInFixedTime(blockIdleHash, crossFadeDuration);
        }

        protected override void PlaySound(SoundId id){
            SoundManager.instance.PlayAudioWithRandomPitch(id);
            CleanPlaySoundEvent();
        }
    }
}
