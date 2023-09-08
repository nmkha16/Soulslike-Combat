using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class GetParryStabbedAction : Action
    {
        private readonly int isParryStabbedHash = Animator.StringToHash("IsParryStabbed");
        [SerializeField] private AnimationClip anim;
        private float aboutToStabTime = 0.8f;
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

            if (elapsed >= aboutToStabTime){
                maria.bloodSplatter.SetActive(true);
                aboutToStabTime = 99f;
            }

            if (elapsed > animLength){
                elapsed = 0f;
                aboutToStabTime = .8f;
                animator.SetBool(isParryStabbedHash,false);
                maria.isParried = false;
                maria.isParryStabbed = false;
                maria.isOnHit = false;
                return Status.Success;
            }

            animator.SetBool(isParryStabbedHash,true);
            maria.isParryStabbed = true;
            return Status.Running;
        }

        public override void Abort(){
            aboutToStabTime = .8f;
            elapsed = 0f;
            maria.isParried = false;
            maria.isParryStabbed = false;
            maria.isOnHit = false;
            animator.SetBool(isParryStabbedHash,false);
        }
    }
}
