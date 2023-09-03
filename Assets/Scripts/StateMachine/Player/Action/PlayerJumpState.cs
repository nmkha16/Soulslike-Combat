using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerJumpState : PlayerBaseState
    {
        protected readonly int jumpHash = Animator.StringToHash("Jump");
        private const float crossFadeDuration = .1f;

        public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void Enter()
        {
            playerStateMachine.velocity = new Vector3(playerStateMachine.velocity.x, playerStateMachine.jumpForce,playerStateMachine.velocity.z);
            playerStateMachine.animator.CrossFadeInFixedTime(jumpHash,crossFadeDuration);
        }

        public override void Tick()
        {
            ApplyGravity();

            if (playerStateMachine.velocity.y <= 0){
                playerStateMachine.SwitchState(new PlayerFallState(playerStateMachine));
            }
            
            FaceMoveDirection();
            Move();
        }

        public override void Exit()
        {
            
        }
    }
}
