using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class FollowPlayerAction : Action
    {
        private MariaBoss maria;
        private Animator animator;
        private Transform transform;
        private Transform target;

        [SerializeField] private float moveSpeed = 3.5f;

        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
            transform = gameObject.transform;
        }

        public override void Start(){
            target = maria.target;
        }

        protected override Status OnUpdate()
        {
            maria.ApplyGravity();
            maria.CalculateMoveDirection(target.position,moveSpeed);
            maria.FaceMoveDirection();
            maria.Move();

            // animator set follow player in lock on manner

            return Status.Running;
        }
    }
}
