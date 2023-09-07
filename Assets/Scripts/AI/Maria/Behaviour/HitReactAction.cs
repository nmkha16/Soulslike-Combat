using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class HitReactAction : Action
    {
        private readonly int isOnHitHash = Animator.StringToHash("IsOnHit");
        [SerializeField] private float hitReactDuration = 1.2f;
        private MariaBoss maria;
        private Animator animator;
        private float elapsed = 0f;

        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;
            if (elapsed > hitReactDuration){
                elapsed = 0f;
                maria.isOnHit = false;
                maria.ExitIgnoreRaycastLayer();
                animator.SetBool(isOnHitHash, false);
                return Status.Success;
            }
            animator.SetBool(isOnHitHash,true);
            return Status.Running;
        }
    }
}
