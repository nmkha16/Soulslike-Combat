using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class ParriedAction : Action
    {
        private readonly int isParriedHash = Animator.StringToHash("IsParried");
        [SerializeField] private AnimationClip anim;
        private MariaBoss maria;
        private Animator animator;
        private float animLength;
        private float elapsed = 0f;

        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
            animLength = anim.length;
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;

            if (elapsed > animLength){
                elapsed = 0f;
                animator.SetBool(isParriedHash,false);
                maria.isParried = false;
                return Status.Success;
            }

            maria.isParried = true;
            animator.SetBool(isParriedHash,true);
            return Status.Running;
        }

        public override void Abort(){
            elapsed = 0f;
            maria.isParried = false;
            animator.SetBool(isParriedHash,false);
        }
    }
}
