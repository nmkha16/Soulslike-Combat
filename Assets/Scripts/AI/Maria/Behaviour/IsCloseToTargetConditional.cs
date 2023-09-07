using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class IsCloseToTargetConditional : Conditional
    {
        [SerializeField] private float maxAcceptableDistance = 5f;
        private MariaBoss maria;

        protected override void OnAwake(){
            maria = gameObject.GetComponent<MariaBoss>();
        }

        protected override bool IsUpdatable()
        {
            if (maria.target == null) return false;
            return maria.IsArriveAtPosition(maria.target.position,maxAcceptableDistance);
        }
    }
}
