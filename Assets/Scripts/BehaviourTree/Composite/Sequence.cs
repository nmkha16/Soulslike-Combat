using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree.Composite{
    public class Sequence : Node
    {
        public Sequence() : base() {}
        public Sequence(List<Node> children) : base (children){}
        
        public override NodeState Evaluate(){
            bool isAnyChildRunning = false;

            foreach(var child in children){
                switch(child.Evaluate()){
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        isAnyChildRunning = true;
                        continue;
                    default:
                        return NodeState.Success;
                }
            }

            state = isAnyChildRunning ? NodeState.Running : NodeState.Failure;
            return state;
        }
    }
}
