using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class CheckTargetInRangeAction : Action
    {
        private readonly int isNeutralHash = Animator.StringToHash("IsNeutral");
        private MariaBoss maria;
        private Animator animator;
        private Transform transform;
        private LayerMask targetLayerMask;
        [SerializeField] private float radius = 8f;
        [SerializeField] private float maxAcceptableDistance = 140f;
        private Collider[] hitColliders = new Collider[1];

        public override void Awake() {
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
            transform = gameObject.transform;
        }

        public override void Start(){
            targetLayerMask = maria.targetLayerMask;
        }

        protected override Status OnUpdate()
        {
            // should perform speherecheck once then stop after the AI got the target
            // we should depend on distance check which is better than using overlapsphere every frame
            if (maria.target != null){
                var distance = (maria.target.transform.position - transform.position).sqrMagnitude;
                if (distance <= maxAcceptableDistance){
                    return Status.Success;
                }
                
                maria.target = null;
                maria.isInCombat = false;
                animator.SetBool(isNeutralHash,false);
                return Status.Failure;
            }

            // detect target for the first time
            var target = GetTargetInRange(transform.position,radius);

            if (target == null) {
                maria.isInCombat = false;
                maria.target = null;
                return Status.Failure;
            }
            else{
                maria.isInCombat = true;
                maria.target = target.transform;
                maria.shouldTaunt = true;
                animator.SetBool(isNeutralHash,false);
                return Status.Success;
            }
        }

        /// <summary>
        /// look for a player in range
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private GameObject GetTargetInRange(Vector3 center, float radius){
            int numColliders = Physics.OverlapSphereNonAlloc(center,radius, hitColliders,this.targetLayerMask);

            if (numColliders == 0) return null;

            return hitColliders[0].gameObject;
        }
    }
}
