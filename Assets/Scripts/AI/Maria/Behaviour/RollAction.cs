using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
using UnityEditor.Experimental.GraphView;

namespace AI.Maria.Behaviour{
    public class RollAction : Action
    {
        private readonly int isRollHash = Animator.StringToHash("IsRoll");
        [SerializeField] private AnimationClip anim;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float easing = 3f;
        private Animator animator;
        private MariaBoss maria;
        private Transform transform;
        private float animLength;
        private float elapsed = 0f;
        private Vector3 rollDirection;
        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
            transform = gameObject.transform;
        }

        public override void Start(){
            animLength = anim.length;
            rollDirection = transform.forward;
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= animLength){
                elapsed = 0f;
                animator.SetBool(isRollHash, false);
                DecideRollDirection();
                return Status.Success;
            }
            maria.ApplyGravity();
            maria.CalculateRollDirection(rollDirection,elapsed,curve,easing);
            maria.FaceMoveDirection();
            maria.Move();

            animator.SetBool(isRollHash,true);
            return Status.Running;
        }

        private void DecideRollDirection(){
            var dir = (RollDirection)UnityEngine.Random.Range(0,4);
            switch (dir)
            {
                case RollDirection.Left:
                    rollDirection = -transform.right;
                    break;
                case RollDirection.Right:
                    rollDirection = transform.right;
                    break;
                case RollDirection.Forward:
                    rollDirection = transform.forward;
                    break;
                case RollDirection.Backward:
                    rollDirection = -transform.forward;
                    break;
                default:
                    rollDirection = -transform.forward;
                    break;
            }
        }
    }
}
