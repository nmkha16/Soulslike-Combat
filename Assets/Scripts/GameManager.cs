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
    [Header("UI")]
    [SerializeField] private PlayerLockOnUI lockingUI;
    [SerializeField] private Animator cinemachineAnimator;
    protected readonly int followPlayerAnim = Animator.StringToHash("FollowPlayerCamera");
    protected readonly int lockOnTargetAnim = Animator.StringToHash("LockOnTargetCamera");
    private PlayerStateMachine playerStateMachine;

    private void Awake(){
        PlayerStateMachine.OnPlayerInitialized += LoadPlayerStateMachine;
    }

    private void Start(){
        SpawnPlayer();
    }
    
    private void OnDestroy() {
        PlayerStateMachine.OnPlayerInitialized -= LoadPlayerStateMachine;
        playerStateMachine.OnLockOnTargetActionPerformed -= LockOnTargetUI;

    }

    private void LoadPlayerStateMachine(PlayerStateMachine playerStateMachine){
        this.playerStateMachine = playerStateMachine;
        this.playerStateMachine.OnLockOnTargetActionPerformed += LockOnTargetUI;
    }

    private void LockOnTargetUI(Transform target, bool isFound){
        var targetPosition = isFound ? CalculateHeightIndicator(target) : Vector3.zero;
        
        if (isFound){
            cinemachineAnimator.Play(lockOnTargetAnim);
            lockOnCamera.m_LookAt = target;
        }
        else cinemachineAnimator.Play(followPlayerAnim);

        lockingUI.ToggleLockOnIndicator(targetPosition,isFound);
    }

    private Vector3 CalculateHeightIndicator(Transform target){
        float h1 = target.GetComponent<CapsuleCollider>().height;
        float h2 = target.localScale.y;

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

    }
}
