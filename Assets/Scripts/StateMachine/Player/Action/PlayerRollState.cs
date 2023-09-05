using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Action{
    public class PlayerRollState : PlayerBaseState
    {
        private readonly int rollHash = Animator.StringToHash("Roll");
        private const float crossFadeDuration = 0.2f;
        private AttackSequence attackSequence = AttackSequence.Roll; // bear with me the name i'm too lazy to fix it, inspector will reset all my animation curve
        private float easingCurve = 5f;
        private float recommendSpeed = 1.5f;
        private float animLength;
        private AnimationCurve curve;
        private float elapsed = 0f;

        private Vector2 moveCompositeSnapshot;
        public PlayerRollState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.animator.CrossFadeInFixedTime(rollHash, crossFadeDuration);
            playerStateMachine.animator.SetFloat(animMultiplier,recommendSpeed);
            animLength = playerStateMachine.animationClips[(int)attackSequence].anim.length / recommendSpeed;
            curve = playerStateMachine.animationClips[(int)attackSequence].curve;
            moveCompositeSnapshot = playerStateMachine.inputReader.moveComposite;
        }

        public override void Tick()
        {
            elapsed += Time.deltaTime;

            ApplyGravity();
            CalculateMoveDirection(elapsed, curve, easing: easingCurve);
            FaceMoveDirection();
            Move();

            if (elapsed > animLength * 0.9f){
                if (playerStateMachine.inputReader.isLockedOnTarget){
                    playerStateMachine.SwitchState(new PlayerLockOnState(playerStateMachine));
                    return;
                }
                playerStateMachine.SwitchState(new PlayerMoveState(playerStateMachine));
            }
        }

        public override void Exit()
        {
            
        }

        protected override void CalculateMoveDirection(float elapsed, AnimationCurve curve, float easing){
            Vector3 camForward = new Vector3(playerStateMachine.mainCamera.transform.forward.x, 0, playerStateMachine.mainCamera.transform.forward.z);
            Vector3 camRight = new Vector3(playerStateMachine.mainCamera.transform.right.x, 0, playerStateMachine.mainCamera.transform.right.z);
            Vector3 moveDirection = camForward.normalized * moveCompositeSnapshot.y + camRight.normalized * moveCompositeSnapshot.x;

            playerStateMachine.velocity.x = moveDirection.x * curve.Evaluate(elapsed) * easing;
            playerStateMachine.velocity.z = moveDirection.z * curve.Evaluate(elapsed) * easing;
        }
    }

}
