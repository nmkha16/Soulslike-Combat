using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class ReturnToSpawnAction : Action
    {
        private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        private readonly int isNeutral = Animator.StringToHash("IsNeutral");
        private const float animationDampTime = 0.1f;

        [SerializeField] private float maxAcceptableDistanceFromSpawn = 0.02f;
        [SerializeField] private float moveSpeed = 4f;

        private MariaBoss maria;
        private Animator animator;
        private Vector3 spawnPosition;

        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
        }

        public override void Start() {
            spawnPosition = maria.spawnPosition;
        }

        protected override Status OnUpdate()
        {
            if (maria.IsArriveAtPosition(spawnPosition,maxAcceptableDistanceFromSpawn)){
                return Status.Success;
            }
            
            maria.ApplyGravity();
            maria.CalculateMoveDirection(spawnPosition,moveSpeed);
            maria.FaceMoveDirection();
            maria.Move();

            animator.SetBool(isNeutral,true);
            animator.SetFloat(moveSpeedHash,1f,animationDampTime,Time.deltaTime);
            return Status.Running;
        }

    }
}