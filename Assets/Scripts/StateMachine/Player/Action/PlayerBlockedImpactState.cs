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
            playerStateMachine.velocity = Vector3.zero;
            playerStateMachine.animator.CrossFadeInFixedTime(blockedImpactHash, crossFadeDuration);
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
            playerStateMachine.animator.CrossFadeInFixedTime(blockIdleHash, crossFadeDuration);
        }
    }
}
