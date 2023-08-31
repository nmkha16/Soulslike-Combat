using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 velocity;
    public float moveSpeed {get; private set;} = 5f;
    public float jumpForce {get; private set;} = 5f;
    public float lookRotationDampFactor {get; private set;} = 10f;
    public Transform mainCamera {get; private set;}
    public InputReader inputReader {get; private set;}
    public Animator animator {get; private set;}
    public CharacterController characterController {get; private set;}

    [Header("Attack Animation Clips")]
    public List<AnimationClip> animationClips;

    // TODO: Allow 3rd camera rotation
    // // cinemachine
    // private float cinemachineTargetYaw;
    // private float cinemachineTargetPitch;

    private void Start(){
        mainCamera = Camera.main.transform;

        inputReader = GetComponent<InputReader>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        SwitchState(new PlayerMoveState(this));
    }
}
