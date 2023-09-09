using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerImpactState : PlayerBaseState
    {
        protected readonly int impactHash = Animator.StringToHash("Unblocked Impact");
        private const float crossFadeDuration = 0.02f;
        private const float waitTime = 1.2f;
        private float elapsed = 0f;
        public PlayerImpactState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.velocity.x = 0f;
            playerStateMachine.velocity.z = 0f;
            playerStateMachine.animator.CrossFadeInFixedTime(impactHash, crossFadeDuration);
            playerStateMachine.ToggleInvincibility(true);
            playerStateMachine.isTakenDamge = true;
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;


            ApplyGravity();
            Move();

            if (!playerStateMachine.characterController.isGrounded){
                Debug.Log("a");
                SwitchToFallState();
            }

            if (elapsed > waitTime){
                if (playerStateMachine.inputReader.isLockedOnTarget){
                    playerStateMachine.isTakenDamge = false;
                    playerStateMachine.ToggleInvincibility(false);
                    SwitchToLockOnState();
                    return;
                }
                playerStateMachine.isTakenDamge = false;
                playerStateMachine.ToggleInvincibility(false);
                SwitchToMoveState();
            }
        }

        public override void Exit()
        {
            playerStateMachine.isTakenDamge = false;
            playerStateMachine.ToggleInvincibility(false);
        }
    }
}
