using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera followPlayerCamera;
    [SerializeField] private CinemachineVirtualCamera lockOnCamera;
    [SerializeField] private CinemachineVirtualCamera riposteCamera;
    [SerializeField] private CinemachineShake riposteCameraShake;
    [SerializeField] private float riposteCameraShakeAmplitudeGain = 2f;
    [SerializeField] private float riposteCameraShakeTime = 0.3f;
    [Header("UI")]
    [SerializeField] private PlayerLockOnUI lockingUI;
    [SerializeField] private Animator cinemachineAnimator;
    protected readonly int followPlayerAnimHash = Animator.StringToHash("FollowPlayerCamera");
    protected readonly int lockOnTargetAnimHash = Animator.StringToHash("LockOnTargetCamera");
    protected readonly int riposteCameraAnimHash = Animator.StringToHash("RiposteCamera");
    private PlayerStateMachine playerStateMachine;

    private Coroutine lockOnRoutine;

    private void Awake(){
        PlayerStateMachine.OnPlayerInitialized += LoadPlayerStateMachine;
        riposteCameraShake = riposteCamera.GetComponent<CinemachineShake>();
    }

    private void Start(){
        SpawnPlayer();
        // Locks the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Confines the cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false; 
    }
    
    private void OnDestroy() {
        PlayerStateMachine.OnPlayerInitialized -= LoadPlayerStateMachine;
        this.playerStateMachine.OnLockOnTargetActionPerformed -= LockOnTargetUI;
        this.playerStateMachine.OnParryStabPerformed -= StartRiposteCamera;
        this.playerStateMachine.OnParryStabEnded -= ReturnToLockOnCamera;
        this.playerStateMachine.OnParryExactStab -= ShakeCamera;
        Cursor.visible = false; 
        Cursor.lockState= CursorLockMode.None;
    }

    private void LoadPlayerStateMachine(PlayerStateMachine playerStateMachine){
        this.playerStateMachine = playerStateMachine;
        this.playerStateMachine.OnLockOnTargetActionPerformed += LockOnTargetUI;
        this.playerStateMachine.OnParryStabPerformed += StartRiposteCamera;
        this.playerStateMachine.OnParryStabEnded += ReturnToLockOnCamera;
        this.playerStateMachine.OnParryExactStab += ShakeCamera;
    }

    private void LockOnTargetUI(Transform target, bool isFound){
        if (isFound){
            cinemachineAnimator.Play(lockOnTargetAnimHash);
            lockOnCamera.m_LookAt = target;
            var targetCharacterController = target.GetComponent<CharacterController>();

            StopLockOnRoutine();

            lockOnRoutine = StartCoroutine(UpdateLockOnIndicatorUIPosition(targetCharacterController));
        }
        else {
            lockingUI.ToggleLockOnIndicator(false);

            StopLockOnRoutine();

            cinemachineAnimator.Play(followPlayerAnimHash);
        }
    }

    private void StopLockOnRoutine(){
        if (lockOnRoutine != null){
            StopCoroutine(lockOnRoutine);
            lockOnRoutine = null;
        }
    }

    private IEnumerator UpdateLockOnIndicatorUIPosition(CharacterController target){
        lockingUI.ToggleLockOnIndicator(true);
        while (true){
            var newPosition = CalculateHeightIndicator(target);
            lockingUI.UpdateLockOnIndicator(newPosition);
            yield return null;
        }
    }

    /// <summary>
    /// Use capsule collider high to calculate target's chest area
    /// Multiply Capsule Height with Y scale = Height
    /// To get the target's chest location, we take the Height - (Height/2/2)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private Vector3 CalculateHeightIndicator(CharacterController target){
        float h1 = target.height;
        float h2 = target.transform.localScale.y;

        float h = h1*h2;
        float half_h = (h/2)/2;
        float chestYoffset = h - half_h;

        var result = target.transform.position;
        result.y = chestYoffset;
        return result;
    }

    private void SpawnPlayer(){
        var player = Instantiate(playerPrefab);
        player.GetComponent<PlayerStateMachine>().AssignCamera(followPlayerCamera.gameObject);

        // for 3rd person camera
        followPlayerCamera.m_Follow = player.transform;
        followPlayerCamera.m_LookAt = player.transform;

        // for lock on camera
        lockOnCamera.m_Follow = player.transform;

        // for riposte camera
        riposteCamera.m_Follow = player.transform;
    }

    private void StartRiposteCamera(){
        cinemachineAnimator.Play(riposteCameraAnimHash);
        riposteCamera.m_LookAt = playerStateMachine.lockOnTarget;
    }

    private void ReturnToLockOnCamera(){
        cinemachineAnimator.Play(lockOnTargetAnimHash);
    }

    private void ShakeCamera(){
        riposteCameraShake.ShakeCamera(riposteCameraShakeAmplitudeGain,riposteCameraShakeTime);
    }
}
