using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class SprintTowardTargetAction : Action
    {
        private readonly int moveXHash = Animator.StringToHash("MoveX");
        private readonly int moveYHash = Animator.StringToHash("MoveY");
        private const float animationDampTime = 0.2f;
        private MariaBoss maria;
        private Animator animator;
        [SerializeField] private float maxAcceptableDistanceFromTargetToStopSprint = 5f;
        [SerializeField] private float moveSpeed = 3.5f;

        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
        }

        public override void Start(){
        }

        protected override Status OnUpdate()
        {
            if (maria.IsArriveAtPosition(maria.target.position,maxAcceptableDistanceFromTargetToStopSprint)){
                return Status.Success;
            }


            maria.ApplyGravity();
            maria.CalculateMoveDirection(maria.target.position,moveSpeed);
            maria.FaceMoveDirection();
            maria.Move();

            animator.SetFloat(moveXHash,0f,animationDampTime,Time.deltaTime);
            animator.SetFloat(moveYHash,1f,animationDampTime,Time.deltaTime);

            return Status.Running;
        }
    }
}
