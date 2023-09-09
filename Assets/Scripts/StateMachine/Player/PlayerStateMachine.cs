using System;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using FSM.Action;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : StateMachine, IDamagable, ICanParryStab
{
    public Action OnBlockedHit;
    public static Action<PlayerStateMachine> OnPlayerInitialized;
    public Action<Transform,bool> OnLockOnTargetActionPerformed;
    public Action OnParryStabPerformed;
    public Action OnParryExactStab; // invoke at frame where the user is about to stab target
    public Action OnParryStabEnded;
    public int health = 200;
    public Vector3 velocity;
    public float moveSpeed {get; private set;} = 5f;
    public float jumpForce {get; private set;} = 5f;
    public float lookRotationDampFactor {get; private set;} = 10f;
    public Camera mainCamera {get; private set;}
    public InputReader inputReader {get; private set;}
    public Animator animator {get; private set;}
    public CharacterController characterController {get; private set;}

    [Header("Attack Animation Clips")]
    public List<AttackAnimation> attackAnimationClips;
    [Header("Movement Animation Clips")]
    public List<AttackAnimation> movementAnimationClips;
    [Header("Defense Animation Clip")]
    public List<DefenseAnimation> defenseAnimationClips;
    [Header("Camera")]
    private GameObject cinemachineVirtualCamera;
    private float yaw, pitch;
    private const float cameraThreshold = 0.01f;
    public bool isLockedOnTarget => inputReader.isLockedOnTarget;
    public bool isRunning => inputReader.isRunning;

    public int Health { 
        get => health;
        set {
            this.health = value;
        }
    }
    [Header("This field has nothing to do with Input Reader")]
    public bool isTakenDamge;
    public bool isParrying;
    public bool isBlocking;

    [Header("Target Layer")]
    public LayerMask targetLayerMask;
    public float targetLockOnRadius = 8f; // use for check overlapsphere
    public float maxLockOnRangeBeforeCancel = 140f; // distance via sqrmagnitude
    private Collider[] hitColliders = new Collider[3];
    public Transform lockOnTarget;
    private LayerMask defaultLayerMask;
    private LayerMask ignoreRaycastLayerMask; 

    [Header("Hit Detection")]
    [SerializeField] private HitDetection hitDetection;

    [Header("Sword sfx")]
    [SerializeField] private GameObject swordSfx;
    
    public void AssignCamera(GameObject followPlayerCamera){
        this.cinemachineVirtualCamera = followPlayerCamera;
    }

    private void Awake(){
        if (hitDetection == null){
            hitDetection = GetComponentInChildren<HitDetection>();
        }

        defaultLayerMask = gameObject.layer;   
        ignoreRaycastLayerMask = LayerMask.NameToLayer("Ignore Raycast"); 
    }

    private void Start(){
        yaw = cinemachineVirtualCamera.transform.rotation.eulerAngles.y;

        mainCamera = Camera.main;

        inputReader = GetComponent<InputReader>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        SwitchState(new PlayerMoveState(this));
        OnPlayerInitialized?.Invoke(this);

        inputReader.OnLockedOnPerformed += LockOnTarget;
    }

    private void LateUpdate() {
        RotateCamera();
    }

    private void OnDestroy() {
        inputReader.OnLockedOnPerformed -= LockOnTarget;
    }

    protected void RotateCamera(){
        if (isLockedOnTarget){
            return;
        }

        if (inputReader.mouseDelta.sqrMagnitude > cameraThreshold && !isLockedOnTarget){
            yaw += inputReader.mouseDelta.x * Time.deltaTime * 3f;
            pitch += inputReader.mouseDelta.y * Time.deltaTime  * 3f;
        }

        // clamp value on 360 degree
        yaw = ClampAngle(yaw, float.MinValue, float.MaxValue);
        pitch = ClampAngle(pitch, -30f, 70f);

        cinemachineVirtualCamera.transform.rotation = Quaternion.Euler(pitch , yaw, 0f);
    }

    public float ClampAngle(float angle, float minAngle, float maxAngle){
        if (angle > 360f) angle -= 360f;
        else if (angle < -360f) angle +=360f;
        return Mathf.Clamp(angle,minAngle,maxAngle);
    }

    
    protected GameObject GetNearestTarget(Vector3 center, float radius){
        int numColliders = Physics.OverlapSphereNonAlloc(center,radius, hitColliders,this.targetLayerMask);

        if (numColliders == 0) return null;
        
        float currentNearestDistance = Vector3.SqrMagnitude(hitColliders[0].transform.position - this.transform.position);
        var nearestTarget = hitColliders[0].gameObject;
        for(int i = 1; i < numColliders; ++i){
            var newDistance = Vector3.SqrMagnitude(hitColliders[i].transform.position - this.transform.position);
            if (newDistance < currentNearestDistance){
                nearestTarget = hitColliders[i].gameObject;
            }
        }
        return nearestTarget;
    }

    protected void LockOnTarget(){
        // if user is currently locking the target, cancel locking action
        if (isLockedOnTarget){
            CancelLockOnState();
            return;
        }
        
        var target = GetNearestTarget(this.transform.position,this.targetLockOnRadius);
        // if we invoke the event with boolean param to let the receiver know if we get the target to lock on
        bool isFoundTarget = target != null;
        this.inputReader.isLockedOnTarget = isFoundTarget;
        if (isFoundTarget){
            this.OnLockOnTargetActionPerformed?.Invoke(target.transform,isFoundTarget);
            lockOnTarget = target.transform;
            SwitchToLockOnState();
            return;
        }
        lockOnTarget = null;
        this.OnLockOnTargetActionPerformed?.Invoke(null,isFoundTarget);
    }

    public void CancelLockOnState(){
        inputReader.isLockedOnTarget = false;
        lockOnTarget = null;
        OnLockOnTargetActionPerformed?.Invoke(null,false);
    }

    protected void SwitchToLockOnState(){
        this.SwitchState(new PlayerLockOnState(this));
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,targetLockOnRadius);
    }

    public void ToggleWeaponHitbox(bool toggle){
        hitDetection.enabled = toggle;
    }

    public void ToggleInvincibility(bool toggle){
        gameObject.layer = toggle ? ignoreRaycastLayerMask : defaultLayerMask;
    }

    public void TakeDamage(Transform from,HitWeapon hitWeapon, int amount)
    {
        if (isTakenDamge) return;

        if (isBlocking){
            OnBlockedHit?.Invoke();
        }
        else if (isParrying){
            if (from.TryGetComponent<IParriable>(out var parriable)){
                SoundManager.instance.PlayAudio(SoundId.sfx_parry);
                parriable.GetParried();
            }
        }
        else{
            HitEffectHelper.PlayHitEffect(hitWeapon);
            isTakenDamge = true;
            SwitchToImpactState();
            Health-= amount;
        }
    }

    protected void SwitchToImpactState(){
        SwitchState(new PlayerImpactState(this));
    }

    private void SwitchToParryStabState(){
        SwitchState(new PlayerParryStabState(this));
    }

    public void ParryStab()
    {
        OnParryStabPerformed?.Invoke();
        SwitchToParryStabState();
    }

    public void ToggleSwordSfx(bool toggle){
        swordSfx.SetActive(toggle);
    }
}
