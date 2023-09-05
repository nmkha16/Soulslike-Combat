using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Maria{
    public class MariaBoss : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [HideInInspector] public Vector3 spawnPosition {get; private set;}
        [Header("Properties")]
        public Vector3 velocity;
        [SerializeField] private float lookRotationDampFactor = 10f;

        [Header("Target Player")]
        public LayerMask targetLayerMask;
        public bool isInCombat;
        [HideInInspector] public Transform target;

        private void Awake(){
            if (characterController == null){
                characterController = GetComponent<CharacterController>();
            }
            if (animator == null){
                animator = GetComponent<Animator>();
            }
            spawnPosition = transform.position;
        }

        public void FaceMoveDirection(){
            Vector3 dir = new Vector3(velocity.x,0,velocity.z);
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),lookRotationDampFactor * Time.deltaTime);
        }

        public void CalculateMoveDirection(Vector3 position, float moveSpeed){
            Vector3 dir = (position - transform.position).normalized;
            this.velocity.x = dir.x * moveSpeed;
            this.velocity.z = dir.z * moveSpeed;
        }

        public void CalculateMoveDirection(float elapsed, AnimationCurve curve, float easing){
            Vector3 moveDirection = transform.forward;

            velocity.x = moveDirection.x * curve.Evaluate(elapsed) * easing;
            velocity.z = moveDirection.z * curve.Evaluate(elapsed) * easing;
        }

        public void FaceTarget(){
            if (target == null) return;
            Vector3 dir = target.position - this.transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),lookRotationDampFactor* Time.deltaTime);
        }

        public void ApplyGravity(){
            if (this.velocity.y > Physics.gravity.y){
                this.velocity.y += Physics.gravity.y * Time.deltaTime;
            }
        }

        public void Move(){
            characterController.Move(this.velocity * Time.deltaTime);
        }

        public bool IsArriveAtPosition(Vector3 position, float maxAcceptableDistance){
            var distance = position - this.transform.position;
            return distance.sqrMagnitude <= maxAcceptableDistance;
        }

    }
}
