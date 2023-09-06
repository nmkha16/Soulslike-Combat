using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBT;

namespace AI.Maria.Behaviour{
    public class IsTargetRightSideConditional : Conditional
    {
        private MariaBoss maria;
        private Transform transform;
        protected override void OnAwake(){
            maria = gameObject.GetComponent<MariaBoss>();
            transform = gameObject.transform;
        }

        protected override bool IsUpdatable()
        {
            if (maria.target == null) return false; // return anything doesn't matter here, we can make AI turn left or right np when target out of range
            //return IsTargetRightSide(transform.forward,maria.target.position,Vector3.up);
            return isRightSide(transform.right,transform.position,maria.target.position);
        }

        // nmkha: this is old, the newer version is more efficient, less math computation
        // //https://forum.unity.com/threads/left-right-test-function.31420/
        // private bool IsTargetRightSide(Vector3 fwd, Vector3 targetPos, Vector3 up){
        //     Vector3 right = Vector3.Cross(up.normalized,fwd.normalized);
        //     float dir = Vector3.Dot(right,targetPos.normalized);
        //     return dir > 0f;
        // }

        // https://www.habrador.com/tutorials/math/3-turn-left-or-right/
        private bool isRightSide(Vector3 right, Vector3 selfPos, Vector3 targetPos){
            Vector3 targetDir = targetPos - selfPos;
            float dotProduct = Vector3.Dot(right,targetDir);
            return dotProduct > 0f;
        }
    }
}
