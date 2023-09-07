using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Maria{
    public class MariaBoss : MonoBehaviour, IDamagable
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [HideInInspector] public Vector3 spawnPosition {get; private set;}

        private int health;
        public int Health { 
            get {
                return health;
            }
            set{
                health = value;
            }
        }

        [Header("Properties")]
        public Vector3 velocity;
        [SerializeField] private float lookRotationDampFactor = 10f;

        [Header("Target Player")]
        public LayerMask targetLayerMask;
        public bool isInCombat;
        public bool shouldTaunt;
        [HideInInspector] public Transform target;

        private void Awake(){
            if (characterController == null){
                characterController = GetComponent<CharacterController>();
            }
            if (animator == null){
                animator = GetComponent<Animator>();
            }
            spawnPosition = transform.position;
            health = 100;
        }

        public void FaceMoveDirection(){
            Vector3 dir = new Vector3(velocity.x,0,velocity.z);
            if (dir == Vector3.zero) return;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),lookRotationDampFactor * Time.deltaTime);
        }

        public void CalculateMoveDirection(Vector3 position, float moveSpeed){
            Vector3 dir = (position - transform.position).normalized;
            this.velocity.x = dir.x * moveSpeed;
            this.velocity.z = dir.z * moveSpeed;
        }

        /// <summary>
        /// calculate forward direction of the said object
        /// </summary>
        /// <param name="elapsed"></param>
        /// <param name="curve"></param>
        /// <param name="easing"></param>
        public void CalculateMoveDirection(float elapsed, AnimationCurve curve, float easing){
            Vector3 moveDirection = transform.forward;
            velocity.x = moveDirection.x * curve.Evaluate(elapsed) * easing;
            velocity.z = moveDirection.z * curve.Evaluate(elapsed) * easing;
        }

        public void CalculateRollDirection(Vector3 rollDirection,float elapsed, AnimationCurve curve, float easing){
            velocity.x = rollDirection.x * curve.Evaluate(elapsed) * easing;
            velocity.z = rollDirection.z * curve.Evaluate(elapsed) * easing;
        }

        public void FaceTarget(){
            if (target == null) return;
            Vector3 dir = target.position - this.transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),lookRotationDampFactor* Time.deltaTime);
        }

        public void FaceTargetImmediately(){
            if (target == null) return;
            Vector3 dir = target.position - this.transform.position;
            dir.y = 0;
            if (dir == Vector3.zero) return;
            transform.rotation = Quaternion.LookRotation(dir);
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

        public void CalculateStrafeDirection(float angle, float strafeSpeed){
            Vector3 dir = target.position - transform.position;

            var strafeDir = (Quaternion.AngleAxis(angle,Vector3.up) * dir).normalized;

            velocity.x = strafeDir.x * strafeSpeed;
            velocity.z = strafeDir.z * strafeSpeed;
        }

        public void TakeDamage(int amount)
        {
            this.Health -= amount;
            Debug.Log("ouch");
            // play hit animation
            
        }
    }
}
