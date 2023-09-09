using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace FSM.Action{
    public class PlayerLockOnState : PlayerBaseState
    {
        private readonly int moveXHash = Animator.StringToHash("MoveX");
        private readonly int moveYHash = Animator.StringToHash("MoveY");
        private readonly int lockOnMoveBlendTreeHash = Animator.StringToHash("LockOnMoveBlendTree");
        private const float crossFadeDuration = .25f;
        private const float animationDampTime = 0.1f;
        private const float deltaLockOnSpeedReductionMultiplier = 0.70f;
        public PlayerLockOnState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.animator.CrossFadeInFixedTime(lockOnMoveBlendTreeHash,crossFadeDuration);
            playerStateMachine.inputReader.OnAttackPerformed += SwitchToAttack1State;
            playerStateMachine.inputReader.OnHeavyAttackPerformed += SwitchToHeavyAttackState;
            playerStateMachine.inputReader.OnLockedOnPerformed += ValidateLockOnState;
            playerStateMachine.inputReader.OnRollPerformed += SwitchToRollState;
            playerStateMachine.inputReader.OnBlockPerformed += SwitchToBlockState;
            playerStateMachine.inputReader.OnParryPerformed += SwitchToParryState;
        }

        public override void Tick()
        {
            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }

            if (IsLockOnTargetOutOfRange()){
                playerStateMachine.CancelLockOnState();
                SwitchToMoveState();
            }
            
            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }

            ApplyGravity();
            CalculateMoveDirection(deltaLockOnSpeedReductionMultiplier);
            FaceTargetDirection();
            Move();

            playerStateMachine.animator.SetFloat(moveXHash, playerStateMachine.inputReader.moveComposite.x,animationDampTime,Time.deltaTime);
            playerStateMachine.animator.SetFloat(moveYHash, playerStateMachine.inputReader.moveComposite.y,animationDampTime,Time.deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.inputReader.OnAttackPerformed -= SwitchToAttack1State;
            playerStateMachine.inputReader.OnHeavyAttackPerformed -= SwitchToHeavyAttackState;
            playerStateMachine.inputReader.OnLockedOnPerformed -= ValidateLockOnState;
            playerStateMachine.inputReader.OnRollPerformed -= SwitchToRollState;
            playerStateMachine.inputReader.OnBlockPerformed -= SwitchToBlockState;
            playerStateMachine.inputReader.OnParryPerformed -= SwitchToParryState;
        }

        private void SwitchToAttack1State(){
            playerStateMachine.SwitchState(new PlayerAttack1State(playerStateMachine));
        }

        private void ValidateLockOnState(){
            if (playerStateMachine.inputReader.isLockedOnTarget) return;
            SwitchToMoveState();
        }
    }

}
