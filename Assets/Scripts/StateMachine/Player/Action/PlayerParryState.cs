using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    // uncomment below if player can move while on parry state
    public class PlayerParryState : PlayerBaseState
    {
        // private readonly int moveXHash = Animator.StringToHash("MoveX");
        // private readonly int moveYHash = Animator.StringToHash("MoveY");
        private readonly int lockOnMoveBlendTreeHash = Animator.StringToHash("LockOnMoveBlendTree");
        private readonly int idleToBlockHash = Animator.StringToHash("Idle To Block");
        private const float crossFadeDuration = .25f;
        // private const float animationDampTime = 0.2f;
        // private const float deltaLockOnSpeedReductionMultiplier = 0.50f;
        private DefenseSequence defenseSequence = DefenseSequence.Parry;
        private float animLength;
        private float elapsed = 0f;
        public PlayerParryState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.isParrying = true;
            playerStateMachine.animator.CrossFadeInFixedTime(lockOnMoveBlendTreeHash,crossFadeDuration);
            playerStateMachine.animator.CrossFadeInFixedTime(idleToBlockHash,crossFadeDuration,1);
            animLength = playerStateMachine.defenseAnimationClips[(int)defenseSequence].anim.length;
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;

            if (!playerStateMachine.characterController.isGrounded){
                playerStateMachine.SwitchState(new PlayerFallState(playerStateMachine));
            }

            if (elapsed > animLength){
                // exit parry state, enter block state
                SwitchToBlockState();
            }

            if (IsLockOnTargetOutOfRange()){
                playerStateMachine.CancelLockOnState();
                SwitchToMoveState();
            }

            ApplyGravity();
            // CalculateMoveDirection(deltaLockOnSpeedReductionMultiplier);
            FaceTargetDirection(playerStateMachine.lockOnTarget);
            Move();

            // playerStateMachine.animator.SetFloat(moveXHash, playerStateMachine.inputReader.moveComposite.x,animationDampTime,Time.deltaTime);
            // playerStateMachine.animator.SetFloat(moveYHash, playerStateMachine.inputReader.moveComposite.y,animationDampTime,Time.deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.isParrying = false;
        }
    }
}
