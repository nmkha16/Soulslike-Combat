using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    // remove comment below if you want player to be able to move while blocking
    public class PlayerBlockState : PlayerBaseState
    {
        // private readonly int moveXHash = Animator.StringToHash("MoveX");
        // private readonly int moveYHash = Animator.StringToHash("MoveY");
        // private readonly int lockOnMoveBlendTreeHash = Animator.StringToHash("LockOnMoveBlendTree");
        private readonly int blockIdleHash = Animator.StringToHash("Block Idle");
        private readonly int blockToIdleHash = Animator.StringToHash("Block To Idle");
        private const float crossFadeDuration = .25f;
        //private const float animationDampTime = 0.2f;
        private const float deltaLockOnSpeedReductionMultiplier = 0.50f;
        public PlayerBlockState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.isBlocking = true;
            // playerStateMachine.animator.CrossFadeInFixedTime(lockOnMoveBlendTreeHash,crossFadeDuration);
            playerStateMachine.animator.CrossFadeInFixedTime(blockIdleHash,crossFadeDuration,1);
            playerStateMachine.inputReader.OnBlockPerformed += ExitBlockState;
        }

        public override void Tick()
        {
            if (!playerStateMachine.inputReader.isHoldingBlock){
                SwitchToLockOnState();
            }

            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }

            if (IsLockOnTargetOutOfRange()){
                playerStateMachine.CancelLockOnState();
                SwitchToMoveState();
            }

            FaceTargetDirection(playerStateMachine.lockOnTarget);
            // ApplyGravity();
            // CalculateMoveDirection(deltaLockOnSpeedReductionMultiplier);
            // FaceTargetDirection(playerStateMachine.lockOnTarget);
            // Move();

            // playerStateMachine.animator.SetFloat(moveXHash, playerStateMachine.inputReader.moveComposite.x,animationDampTime,Time.deltaTime);
            // playerStateMachine.animator.SetFloat(moveYHash, playerStateMachine.inputReader.moveComposite.y,animationDampTime,Time.deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.isBlocking = false;
            playerStateMachine.animator.CrossFadeInFixedTime(blockToIdleHash,0.1f,1);
            playerStateMachine.inputReader.OnBlockPerformed -= ExitBlockState;
        }

        private void ExitBlockState(){
            if (playerStateMachine.inputReader.isLockedOnTarget){
                SwitchToLockOnState();
                return;
            }
            SwitchToMoveState();
        }
    }
}
