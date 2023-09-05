using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using FSM;

public abstract class PlayerBaseState : State
{
    protected readonly int animMultiplier = Animator.StringToHash("AnimMultiplier");
    protected readonly PlayerStateMachine playerStateMachine;
    private const float deltaWalkSpeedReductionMultiplier = 0.25f;

    protected PlayerBaseState(PlayerStateMachine playerStateMachine){
        this.playerStateMachine = playerStateMachine;
    }

    protected virtual void CalculateMoveDirection(){
        Vector3 camForward = new Vector3(playerStateMachine.mainCamera.transform.forward.x, 0, playerStateMachine.mainCamera.transform.forward.z);
        Vector3 camRight = new Vector3(playerStateMachine.mainCamera.transform.right.x, 0, playerStateMachine.mainCamera.transform.right.z);

        Vector3 moveDirection = camForward.normalized * playerStateMachine.inputReader.moveComposite.y + camRight.normalized * playerStateMachine.inputReader.moveComposite.x;
        if (playerStateMachine.inputReader.isRunning){
            playerStateMachine.velocity.x = moveDirection.x * playerStateMachine.moveSpeed;
            playerStateMachine.velocity.z = moveDirection.z * playerStateMachine.moveSpeed;
        }
        else{
            playerStateMachine.velocity.x = moveDirection.x * playerStateMachine.moveSpeed * deltaWalkSpeedReductionMultiplier;
            playerStateMachine.velocity.z = moveDirection.z * playerStateMachine.moveSpeed * deltaWalkSpeedReductionMultiplier;
        }
    }

    protected virtual void CalculateMoveDirection(float elapsed, AnimationCurve curve, float easing){
        Vector3 moveDirection = playerStateMachine.transform.forward;

        playerStateMachine.velocity.x = moveDirection.x * curve.Evaluate(elapsed) * easing;
        playerStateMachine.velocity.z = moveDirection.z * curve.Evaluate(elapsed) * easing;
    }

    protected void FaceMoveDirection(){
        Vector3 faceDirection = new Vector3(playerStateMachine.velocity.x,0,playerStateMachine.velocity.z);

        if (faceDirection == Vector3.zero){
            return;
        }

        playerStateMachine.transform.rotation = Quaternion.Slerp(playerStateMachine.transform.rotation,Quaternion.LookRotation(faceDirection),playerStateMachine.lookRotationDampFactor * Time.deltaTime);
    }

    protected void FaceTargetDirection(Transform target){
        if (target == null) return;
        Vector3 dir = target.position - playerStateMachine.transform.position;
        playerStateMachine.transform.rotation = Quaternion.LookRotation(dir);
    }

    protected void ApplyGravity(){
        if (playerStateMachine.velocity.y > Physics.gravity.y){
            playerStateMachine.velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    protected void Move(){
        playerStateMachine.characterController.Move(playerStateMachine.velocity * Time.deltaTime);
    }

}
