using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class IsCloseToTarget : Conditional
    {
        [SerializeField] private float maxAcceptableDistanceFromTarget = 0.02f;
        private MariaBoss maria;
        private Transform target;

        protected override void OnAwake(){
            maria = gameObject.GetComponent<MariaBoss>();
        }

        protected override void OnStart(){
            target = maria.target;
        }

        protected override bool IsUpdatable()
        {
            return maria.IsArriveAtPosition(target.position, maxAcceptableDistanceFromTarget);
        }

    }
}
