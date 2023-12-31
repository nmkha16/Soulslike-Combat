using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class StrafeLeftAroundTargetAction : Action
{
        private readonly int moveXHash = Animator.StringToHash("MoveX");
        private readonly int moveYHash = Animator.StringToHash("MoveY");
        private const float animationDampTime = 0.25f;
        [SerializeField] private float strafeAngle = -65f;
        [SerializeField] private float strafeSpeed = 2.75f;
        private MariaBoss maria;
        private Animator animator;
        private float elapsed = 0f;
        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
        }
        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;

            if (elapsed > .75f){
                elapsed = 0f;
                return Status.Success;
            }
            
            maria.ApplyGravity();
            maria.CalculateStrafeDirection(strafeAngle,strafeSpeed);
            maria.FaceTarget();
            maria.Move();

            animator.SetFloat(moveXHash,-1f,animationDampTime,Time.deltaTime);
            animator.SetFloat(moveYHash,0f,animationDampTime,Time.deltaTime);

            return Status.Running;
        }

        public override void Abort(){
            elapsed = 0f;
        }
    }
}
