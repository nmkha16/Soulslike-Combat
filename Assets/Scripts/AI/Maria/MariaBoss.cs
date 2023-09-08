using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Maria{
    public class MariaBoss : MonoBehaviour, IDamagable, IParriable
    {   
        public Action<Transform> OnActivateHitbox; // params are Transform of hit component
        public Action OnEndHitbox;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [HideInInspector] public Vector3 spawnPosition {get; private set;}
        [SerializeField] public GameObject bloodSplatter;
        private int health;
        public int Health { 
            get {
                return health;
            }
            set{
                health = value;
            }
        }
        public bool isOnHit;
        public bool isParried;
        public bool isParryStabbed;

        [Header("Properties")]
        public Vector3 velocity;
        [SerializeField] private float lookRotationDampFactor = 10f;

        [Header("Target Player")]
        public LayerMask targetLayerMask;
        public bool isInCombat;
        public bool shouldTaunt;
        [HideInInspector] public Transform target;

        [Header("Ignore Raycast Layer")]
        private LayerMask defaultLayerMask;
        [SerializeField] private LayerMask ignoreRaycastLayer;

        [Header("Hit component")]
        public Transform leftLegTransform;
        public Transform rightLegTransform;
        public Transform rightHandTransform;

        private void Awake(){
            if (characterController == null){
                characterController = GetComponent<CharacterController>();
            }
            if (animator == null){
                animator = GetComponent<Animator>();
            }
            spawnPosition = transform.position;
            health = 100;
            defaultLayerMask = gameObject.layer;
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

        public void TakeDamage(Transform from, int amount)
        {
            gameObject.layer = ignoreRaycastLayer;
            isOnHit = true; // isOnHit true will trigger conditional to on hit
            health -= amount;
            // TODO: update health bar UI maybe

            // tell the attacker to enter parry stab animation
            if (isParried){
                if (from.TryGetComponent<ICanParryStab>(out var canParryStab)){
                    isParryStabbed = true; // victim will enter parry stabbed anim
                    canParryStab.ParryStab(); // attacker will enter parry stab anim
                }
            }
        }

        public void ExitIgnoreRaycastLayer(){
            gameObject.layer = defaultLayerMask;
        }

        public void ToggleInvincibility(bool toggle){
            gameObject.layer = toggle ? ignoreRaycastLayer : defaultLayerMask;
        }

        public void GetParried()
        {
            isParried = true;
        }
    }
}
