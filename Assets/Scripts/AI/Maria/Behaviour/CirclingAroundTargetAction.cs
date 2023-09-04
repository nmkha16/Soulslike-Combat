using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;
using Unity.VisualScripting;

namespace AI.Maria.Behaviour{
    enum StrafeDirection{
        Left = 0,
        Right = 1,
    }
    public class CirclingAroundTargetAction : Action
    {
        private readonly int isNeutralHash = Animator.StringToHash("IsNeutral");
        private readonly int moveXHash = Animator.StringToHash("MoveX");
        private readonly int moveYHash = Animator.StringToHash("MoveY");
        private const float animationDampTime = 0.2f;
        [SerializeField] private float maxAcceptableDistance = 1f;
        [SerializeField] private float strafeAngle = 65f;
        [SerializeField] private float strafeSpeed = 2.75f;
        private MariaBoss maria;
        private Animator animator;
        private StrafeDirection strafeDir = StrafeDirection.Left;

        private float elapsed = 0f;
        public override void Awake(){
            maria = gameObject.GetComponent<MariaBoss>();
            animator = gameObject.GetComponent<Animator>();
        }

        protected override Status OnUpdate()
        {
            elapsed += Time.deltaTime;
            if (maria.IsArriveAtPosition(maria.target.position,maxAcceptableDistance)){
                DecideStrafeOption();
                return Status.Success;
            }

            if (elapsed > 2f){
                DecideStrafeOption();
            }
            
            maria.ApplyGravity();
            StrafeTowardTarget();
            maria.FaceTarget();
            maria.Move();

            animator.SetBool(isNeutralHash,false);
            animator.SetFloat(moveXHash, strafeDir == StrafeDirection.Left ? -1f : 1f,animationDampTime,Time.deltaTime);
            animator.SetFloat(moveYHash,0.3f,animationDampTime,Time.deltaTime);

            return Status.Running;
        }

        private void StrafeTowardTarget(){
            switch (strafeDir){
                case StrafeDirection.Left:
                    CalculateStrafeDirection(-strafeAngle);
                    break;
                case StrafeDirection.Right:
                    CalculateStrafeDirection(strafeAngle);
                    break;
            }
        }

        private void DecideStrafeOption(){
            elapsed = 0;
            int option = UnityEngine.Random.Range(0,2);
            strafeDir = (StrafeDirection)option;
        }

        private void CalculateStrafeDirection(float angle){
            Vector3 dir = maria.target.position - maria.transform.position;

            var strafeDir = (Quaternion.AngleAxis(angle,Vector3.up) * dir).normalized;

            maria.velocity.x = strafeDir.x * strafeSpeed;
            maria.velocity.z = strafeDir.z * strafeSpeed;
        }
    }
}
