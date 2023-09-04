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
            var target = GetTarget(transform.position,radius);

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
        private GameObject GetTarget(Vector3 center, float radius){
            int numColliders = Physics.OverlapSphereNonAlloc(center,radius, hitColliders,this.targetLayerMask);

            if (numColliders == 0) return null;

            return hitColliders[0].gameObject;
        }
    }
}
