using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerImpactState : PlayerBaseState
    {
        protected readonly int impactHash = Animator.StringToHash("Unblocked Impact");
        private const float crossFadeDuration = .1f;
        private const float waitTime = 1.1f;
        private float elapsed = 0f;
        public PlayerImpactState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.animator.CrossFadeInFixedTime(impactHash, crossFadeDuration);
            playerStateMachine.ToggleInvincibility(true);
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;


            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }

            ApplyGravity();
            Move();

            if (elapsed > waitTime){
                if (playerStateMachine.inputReader.isLockedOnTarget){
                    playerStateMachine.SwitchState(new PlayerLockOnState(playerStateMachine));
                    return;
                }
                playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
            }
        }

        public override void Exit()
        {
            playerStateMachine.isTakenDamge = false;
            playerStateMachine.ToggleInvincibility(false);
        }

    }
}
