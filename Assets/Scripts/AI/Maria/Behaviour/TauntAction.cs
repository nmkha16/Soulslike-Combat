using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class TauntAction : Action
    {
        private readonly int isTauntHash = Animator.StringToHash("IsTaunt");
        private readonly int isNeutralHash = Animator.StringToHash("IsNeutral");

        [SerializeField] private AnimationClip anim;
        private Animator animator;
        private MariaBoss maria;
        private float animLength;
        private float elapsed = 0f;
        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
        }

        public override void Start(){
            animLength = anim.length * 0.9f;
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;
            if (elapsed > animLength){
                elapsed = 0f;
                animator.SetBool(isTauntHash, false);
                maria.shouldTaunt = false;
                return Status.Success;
            }

            animator.SetBool(isTauntHash,true);
            animator.SetBool(isNeutralHash, false);

            return Status.Running;
        }

        public override void Abort(){
            elapsed = 0f;
            animator.SetBool(isTauntHash,false);
        }
    }
}
