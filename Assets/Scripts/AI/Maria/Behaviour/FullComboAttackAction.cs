using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
using FSM;

namespace AI.Maria.Behaviour{
    public class FullComboAttackAction : Action
    {
        private readonly int isFullComboHash = Animator.StringToHash("IsFullCombo");
        [SerializeField] private AnimationClip anim;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float easing = 3f;
        private Animator animator;
        private MariaBoss maria;
        private float animLength;
        private float elapsed = 0f;

        public override void Awake() {
            animator = gameObject.GetComponent<Animator>();
            maria = gameObject.GetComponent<MariaBoss>();
        }

        public override void Start(){
            animLength = anim.length;
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= animLength){
                elapsed = 0f;
                animator.SetBool(isFullComboHash, false);
                return Status.Success;
            }
            maria.ApplyGravity();
            maria.CalculateMoveDirection(elapsed,curve,easing);
            maria.FaceMoveDirection();
            maria.Move();

            animator.SetBool(isFullComboHash,true);
            return Status.Running;
        }
    }
}
