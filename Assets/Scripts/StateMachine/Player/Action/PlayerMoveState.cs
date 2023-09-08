using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerMoveState : PlayerBaseState
    {
        private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        private readonly int moveBlendTreeHash = Animator.StringToHash("MoveBlendTree");

        private const float animationDampTime = 0.1f;
        private const float crossFadeDuration = 0.2f;

        public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void Enter()
        {
            playerStateMachine.animator.CrossFadeInFixedTime(moveBlendTreeHash,crossFadeDuration);
            playerStateMachine.inputReader.OnJumpPerformed += SwitchToJumpState;
            playerStateMachine.inputReader.OnAttackPerformed += SwitchToAttack1State;
            playerStateMachine.inputReader.OnHeavyAttackPerformed += SwitchToHeavyAttackState;
            playerStateMachine.inputReader.OnRollPerformed += SwitchToRollState;
        }

        public override void Tick()
        {
            if (!playerStateMachine.characterController.isGrounded){
                SwitchToFallState();
            }
            
            ApplyGravity();
            CalculateMoveDirection();
            FaceMoveDirection();
            Move();

            playerStateMachine.animator.SetFloat(moveSpeedHash, 
                                                playerStateMachine.inputReader.moveComposite.sqrMagnitude > 0f ? 
                                                            (playerStateMachine.inputReader.isRunning ? 1f : 0.5f) : 0f, 
                                                animationDampTime,
                                                Time.deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.inputReader.OnJumpPerformed -= SwitchToJumpState;
            playerStateMachine.inputReader.OnAttackPerformed -= SwitchToAttack1State;
            playerStateMachine.inputReader.OnHeavyAttackPerformed -= SwitchToHeavyAttackState;
            playerStateMachine.inputReader.OnRollPerformed -= SwitchToRollState;
        }

        private void SwitchToAttack1State(){
            if (playerStateMachine.isRunning && playerStateMachine.inputReader.moveComposite.sqrMagnitude > 0){
                playerStateMachine.SwitchState(new PlayerRunningAttackState(playerStateMachine));
            }
            else playerStateMachine.SwitchState(new PlayerAttack1State(playerStateMachine));
        }

        protected override void SwitchToHeavyAttackState(){
            // player can perform heavy attack if only they are not running
            if (playerStateMachine.inputReader.moveComposite.sqrMagnitude == 0){
                playerStateMachine.SwitchState(new PlayerHeavyAttackState(playerStateMachine));
            }
        }
    }
}