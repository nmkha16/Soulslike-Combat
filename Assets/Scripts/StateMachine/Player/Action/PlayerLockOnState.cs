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
            playerStateMachine.velocity.y = Physics.gravity.y;
            playerStateMachine.animator.CrossFadeInFixedTime(lockOnMoveBlendTreeHash,crossFadeDuration);
            playerStateMachine.inputReader.OnAttackPerformed += SwitchToAttack1State;
            playerStateMachine.inputReader.OnHeavyAttackPerformed += SwitchToHeavyAttackState;
            playerStateMachine.inputReader.OnLockedOnPerformed += ValidateLockOnState;
        }

        public override void Tick()
        {
            if (!playerStateMachine.characterController.isGrounded){
                playerStateMachine.SwitchState(new PlayerFallState(playerStateMachine));
            }
            CalculateMoveDirection();
            FaceTargetDirection(playerStateMachine.lockOnTarget);
            Move();

            playerStateMachine.animator.SetFloat(moveXHash, playerStateMachine.inputReader.moveComposite.x,animationDampTime,Time.deltaTime);
            playerStateMachine.animator.SetFloat(moveYHash, playerStateMachine.inputReader.moveComposite.y,animationDampTime,Time.deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.inputReader.OnAttackPerformed -= SwitchToAttack1State;
            playerStateMachine.inputReader.OnHeavyAttackPerformed -= SwitchToHeavyAttackState;
            playerStateMachine.inputReader.OnLockedOnPerformed -= ValidateLockOnState;
        }

        protected override void CalculateMoveDirection(){
            Vector3 camForward = new Vector3(playerStateMachine.mainCamera.transform.forward.x, 0, playerStateMachine.mainCamera.transform.forward.z);
            Vector3 camRight = new Vector3(playerStateMachine.mainCamera.transform.right.x, 0, playerStateMachine.mainCamera.transform.right.z);

            Vector3 moveDirection = camForward.normalized * playerStateMachine.inputReader.moveComposite.y + camRight.normalized * playerStateMachine.inputReader.moveComposite.x;
            
            playerStateMachine.velocity.x = moveDirection.x * playerStateMachine.moveSpeed * deltaLockOnSpeedReductionMultiplier;
            playerStateMachine.velocity.z = moveDirection.z * playerStateMachine.moveSpeed * deltaLockOnSpeedReductionMultiplier;

        }

        private void SwitchToAttack1State(){
            playerStateMachine.SwitchState(new PlayerAttack1State(playerStateMachine));
        }

        private void SwitchToHeavyAttackState(){
            playerStateMachine.SwitchState(new PlayerHeavyAttackState(playerStateMachine));
        }

        private void ValidateLockOnState(){
            if (playerStateMachine.inputReader.isLockedOnTarget) return;
            SwitchToMoveState();
        }

        private void SwitchToMoveState(){
            playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
        }
    }

}
