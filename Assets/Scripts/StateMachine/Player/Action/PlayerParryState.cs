using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerParryState : PlayerBaseState
    {
        private readonly int lockOnMoveBlendTreeHash = Animator.StringToHash("LockOnMoveBlendTree");
        private readonly int idleToBlockHash = Animator.StringToHash("Idle To Block");
        private const float crossFadeDuration = .25f;
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
            playerStateMachine.animator.CrossFadeInFixedTime(idleToBlockHash,crossFadeDuration);
            animLength = playerStateMachine.defenseAnimationClips[(int)defenseSequence].anim.length;
            playerStateMachine.velocity = Vector3.zero;

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
            FaceTargetDirection(playerStateMachine.lockOnTarget);
            Move();
        }

        public override void Exit()
        {
            playerStateMachine.isParrying = false;
        }
    }
}
