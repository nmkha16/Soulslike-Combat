using System.Collections.Generic;

namespace BehaviourTree.Composite{
    public class Selector : Node
    {
        public Selector() : base (){}
        public Selector(List<Node> children) : base(children) {}

        public override NodeState Evaluate(){
            foreach(var child in children){
                switch(child.Evaluate()){
                    case NodeState.Failure:
                        continue;
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Success:
                        return NodeState.Success;
                    default:
                        continue;
                }
            }
            return NodeState.Failure;
        }
    }
}
