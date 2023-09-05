using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class CheckTargetInRangeAction : Action
    {
        private MariaBoss maria;
        private Transform transform;
        [Header("Target Player")]
        private LayerMask targetLayerMask;
        [SerializeField] private float radius = 8f;
        [SerializeField] private float maxAcceptableDistance = 140f;
        private Collider[] hitColliders = new Collider[1];

        public override void Awake() {
            transform = gameObject.transform;
            maria = gameObject.GetComponent<MariaBoss>();
        }

        public override void Start(){
            targetLayerMask = maria.targetLayerMask;
        }

        protected override Status OnUpdate()
        {
            // should perform speherecheck once and stop after the AI got the target
            // then we should depend on distance check which is better than using overlapsphere every frame
            if (maria.target != null){
                var distance = (maria.target.transform.position - transform.position).sqrMagnitude;
                if (distance <= maxAcceptableDistance){
                    return Status.Success;
                }
                
                maria.target = null;
                maria.isInCombat = false;
                return Status.Failure;
            }

            var target = GetTargetInRange(transform.position,radius);

            if (target == null) {
                maria.isInCombat = false;
                maria.target = null;
                return Status.Failure;
            }
            else{
                maria.isInCombat = true;
                maria.target = target.transform;
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
