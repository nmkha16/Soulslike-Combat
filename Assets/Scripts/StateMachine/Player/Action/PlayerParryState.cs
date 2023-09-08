using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerParryState : PlayerBaseState
    {
        private readonly int parryHash = Animator.StringToHash("Parry");
        private const float crossFadeDuration = .1f;
        private DefenseSequence defenseSequence = DefenseSequence.Parry;
        private float animLength;
        private float elapsed = 0f;
        private int frameCount = 0;
        public PlayerParryState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.isParrying = true;
            playerStateMachine.animator.CrossFadeInFixedTime(parryHash,crossFadeDuration);
            animLength = playerStateMachine.defenseAnimationClips[(int)defenseSequence].anim.length;
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;
            frameCount++;
            
            if (frameCount > 40){
                playerStateMachine.isParrying = false;
            }


            if (!playerStateMachine.characterController.isGrounded){
                playerStateMachine.SwitchState(new PlayerFallState(playerStateMachine));
            }

            if (IsLockOnTargetOutOfRange()){
                playerStateMachine.CancelLockOnState();
                SwitchToMoveState();
            }

            
            if (elapsed > animLength * 0.9f){
                if (playerStateMachine.inputReader.isLockedOnTarget){
                    SwitchToLockOnState();
                    return;
                }
                SwitchToMoveState();
            }
            FaceTargetDirection(playerStateMachine.lockOnTarget);
        }

        public override void Exit()
        {
            playerStateMachine.isParrying = false;
        }
    }
}
