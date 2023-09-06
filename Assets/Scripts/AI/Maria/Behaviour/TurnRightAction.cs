using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class TurnRightAction : Action
    {
        private readonly int isRightTurnHash = Animator.StringToHash("IsRightTurn");
        [SerializeField] private float turnTime = 0.8f;
        private Animator animator;
        private float elapsed;

        public override void Awake() {
            animator = gameObject.GetComponent<Animator>();
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;

            if (elapsed > turnTime ){
                elapsed = 0f;
                animator.SetBool(isRightTurnHash,false);
                return Status.Success;
            }

            animator.SetBool(isRightTurnHash,true); 
            return Status.Running;
        }
    }
}
