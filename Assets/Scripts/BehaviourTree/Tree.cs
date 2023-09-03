using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree{
    public abstract class Tree : MonoBehaviour
    {
        protected Node root = null;

        protected virtual void Start(){
            Setup();
        }

        protected void Update(){
            if (root != null){
                root.Evaluate();
            }
        }

        protected abstract void Setup();
    }
}
