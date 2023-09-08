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
            playerStateMachine.velocity = Vector3.zero;
            playerStateMachine.animator.CrossFadeInFixedTime(impactHash, crossFadeDuration);
            playerStateMachine.ToggleInvincibility(true);
            playerStateMachine.isTakenDamge = true;
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
                Debug.Log("impact timeout done");
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
