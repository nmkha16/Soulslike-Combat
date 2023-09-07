using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
using UnityEditor.Experimental.GraphView;

namespace AI.Maria.Behaviour{
    public class RollAction : Action
    {
        protected readonly int animMultiplierHash = Animator.StringToHash("AnimMultiplier");
        private readonly int isRollHash = Animator.StringToHash("IsRoll");
        [SerializeField] private AnimationClip anim;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float easing = 3f;
        [SerializeField] private float recommendSpeed = 1.25f;
        [SerializeField] private int iframeNums = 10;
        private Animator animator;
        private MariaBoss maria;
        private Transform transform;
        private float animLength;
        private float elapsed = 0f;
        private int iframeCount = 0;
        private Vector3 rollDirection;
        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
            transform = gameObject.transform;
        }

        public override void Start(){
            animLength = anim.length / recommendSpeed;
            rollDirection = transform.forward;
        }

        protected override Status OnUpdate()
        {
            iframeCount++;
            if (iframeCount <= iframeNums){
                maria.ToggleInvincibility(true);
            }
            else{
                maria.ToggleInvincibility(false);
            }


            animator.SetFloat(animMultiplierHash,recommendSpeed);
            elapsed += Time.deltaTime;
            if (elapsed >= animLength){
                elapsed = 0f;
                iframeCount = 0;
                animator.SetBool(isRollHash, false);
                animator.SetFloat(animMultiplierHash,10f);
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

        public override void Abort(){
            iframeCount = 0;
            elapsed = 0f;
            animator.SetFloat(animMultiplierHash,1f);
            animator.SetBool(isRollHash,false);
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
