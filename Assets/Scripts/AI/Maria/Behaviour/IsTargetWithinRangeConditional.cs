using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class IsTargetWithinRangeConditional : Conditional
    {
        [SerializeField] private float maxRange = 70f; // dont ask me how i get this number, i got it from debug the actual radius from CheckTargetInRangeAction
        private MariaBoss maria;

        protected override void OnAwake(){
            maria = gameObject.GetComponent<MariaBoss>();
        }

        protected override bool IsUpdatable()
        {
            if (maria.target == null) return false;
            return (maria.target.position - maria.transform.position).sqrMagnitude < maxRange;
        }

    }
}
