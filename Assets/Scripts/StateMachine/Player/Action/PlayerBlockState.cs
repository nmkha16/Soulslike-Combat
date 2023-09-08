using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerBlockState : PlayerBaseState
    {
        private readonly int blockIdleHash = Animator.StringToHash("Block Idle");
        private readonly int blockToIdleHash = Animator.StringToHash("Block To Idle");
        private const float crossFadeDuration = .2f;
        public PlayerBlockState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.isBlocking = true;
            playerStateMachine.animator.CrossFadeInFixedTime(blockIdleHash,crossFadeDuration);
            playerStateMachine.inputReader.OnBlockPerformed += ExitBlockState;
            playerStateMachine.OnBlockedHit += SwitchToBlockedImpactState;
        }

        public override void Tick()
        {
            if (!playerStateMachine.inputReader.isHoldingBlock){
                playerStateMachine.isBlocking = false;
                SwitchToLockOnState();
            }

            if (!playerStateMachine.characterController.isGrounded){
                playerStateMachine.isBlocking = false;
                SwitchToFallState();
            }

            if (IsLockOnTargetOutOfRange()){
                playerStateMachine.isBlocking = false;
                playerStateMachine.CancelLockOnState();
                SwitchToMoveState();
            }

            ApplyGravity();
            FaceTargetDirection(playerStateMachine.lockOnTarget);
            Move();
        }

        public override void Exit()
        {
            //playerStateMachine.isBlocking = false;
            playerStateMachine.animator.CrossFadeInFixedTime(blockToIdleHash,crossFadeDuration);
            playerStateMachine.OnBlockedHit -= SwitchToBlockedImpactState;
            playerStateMachine.inputReader.OnBlockPerformed -= ExitBlockState;
        }

        private void ExitBlockState(){
            playerStateMachine.isBlocking = false;
            if (playerStateMachine.inputReader.isLockedOnTarget){
                SwitchToLockOnState();
                return;
            }
            SwitchToMoveState();
        }

        private void SwitchToBlockedImpactState(){
            playerStateMachine.SwitchState(new PlayerBlockedImpactState(playerStateMachine));
        }
    }
}
