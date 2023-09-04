using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class IdleAction : Action
    {
        private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        private readonly int isNeutralHash = Animator.StringToHash("IsNeutral");
        private const float animationDampTime = 0.05f;
        private Animator animator;

        public override void Awake(){
            animator = gameObject.GetComponent<Animator>();
        }

        protected override Status OnUpdate()
        {
            animator.SetBool(isNeutralHash,true);
            animator.SetFloat(moveSpeedHash,0f,animationDampTime,Time.deltaTime);
            return Status.Success;
        }
    }
}
