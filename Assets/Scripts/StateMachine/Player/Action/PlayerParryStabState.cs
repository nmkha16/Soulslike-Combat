using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerParryStabState : PlayerBaseState
    {
        private readonly int parryStabHash = Animator.StringToHash("Parry Stab");
        private const float crossFadeDuration = 0f;
        private AttackSequence attackSequence = AttackSequence.Parry_Stab;
        private float animLength;
        private float elapsed = 0f;
        public PlayerParryStabState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.animator.CrossFadeInFixedTime(parryStabHash,crossFadeDuration);
            animLength = playerStateMachine.attackAnimationClips[(int)attackSequence].anim.length;
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;

            if (!playerStateMachine.characterController.isGrounded){
                playerStateMachine.SwitchState(new PlayerFallState(playerStateMachine));
            }
            
            if (elapsed > animLength){
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
            
        }
    }
}
