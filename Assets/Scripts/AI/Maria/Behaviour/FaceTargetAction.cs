using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class FaceTargetAction : Action
    {
        private MariaBoss maria;

        public override void Awake() {
            maria = gameObject.GetComponent<MariaBoss>();
        }

        protected override Status OnUpdate()
        {
            if (maria.target == false) return Status.Failure;

            maria.FaceTargetImmediately();
            return Status.Success;
        }
    }
}
