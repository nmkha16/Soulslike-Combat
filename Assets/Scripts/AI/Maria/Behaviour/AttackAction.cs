using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
using FSM;

namespace AI.Maria.Behaviour{
    public class AttackAction : Action
    {
        [SerializeField] private AnimationClip anim; // reference to the anim clip so can we can get exact anim length
        [SerializeField] private AnimationCurve curve; // the animation curve to apply forward movement
        [SerializeField] private string animationHashParams;
        [SerializeField] private float easing = 3f;
        private int isAttackHash;
        private Animator animator;
        private MariaBoss maria;
        private float animLength;
        private float elapsed = 0f;

        public override void Awake() {
            animator = gameObject.GetComponent<Animator>();
            maria = gameObject.GetComponent<MariaBoss>();
            isAttackHash = Animator.StringToHash(animationHashParams);
        }

        public override void Start(){
            animLength = anim.length;
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= animLength){
                elapsed = 0f;
                animator.SetBool(isAttackHash, false);
                return Status.Success;
            }
            maria.ApplyGravity();
            maria.CalculateMoveDirection(elapsed,curve,easing);
            maria.FaceMoveDirection();
            maria.Move();

            animator.SetBool(isAttackHash,true);
            return Status.Running;
        }
    }
}
