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
        }

        private void Start(){
            spawnPosition = transform.position;
        }

        public void FaceMoveDirection(){
            Vector3 dir = new Vector3(velocity.x,0,velocity.z);
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),lookRotationDampFactor * Time.deltaTime);
        }

        public void CalculateMoveDirection(Vector3 position, float moveSpeed){
            Vector3 dir = position - transform.position;

            this.velocity.x = dir.x * moveSpeed;
            this.velocity.z = dir.z * moveSpeed;
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
