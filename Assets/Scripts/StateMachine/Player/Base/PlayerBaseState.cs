using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine playerStateMachine;

    protected PlayerBaseState(PlayerStateMachine playerStateMachine){
        this.playerStateMachine = playerStateMachine;
    }

    protected void CalculateMoveDirection(){
        Vector3 camForward = new Vector3(playerStateMachine.mainCamera.forward.x, 0, playerStateMachine.mainCamera.forward.z);
        Vector3 camRight = new Vector3(playerStateMachine.mainCamera.right.x, 0, playerStateMachine.mainCamera.right.z);

        Vector3 moveDirection = camForward.normalized * playerStateMachine.inputReader.moveComposite.y + camRight.normalized * playerStateMachine.inputReader.moveComposite.x;
        
        playerStateMachine.velocity.x = moveDirection.x * playerStateMachine.moveSpeed;
        playerStateMachine.velocity.z = moveDirection.z * playerStateMachine.moveSpeed;
    }

    protected void FaceMoveDirection(){
        Vector3 faceDirection = new Vector3(playerStateMachine.velocity.x,0,playerStateMachine.velocity.z);

        if (faceDirection == Vector3.zero){
            return;
        }

        playerStateMachine.transform.rotation = Quaternion.Slerp(playerStateMachine.transform.rotation,Quaternion.LookRotation(faceDirection),playerStateMachine.lookRotationDampFactor * Time.deltaTime);
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
