using System.Collections.Generic;

namespace BehaviourTree{
    public enum NodeState{
        Running,
        Success,
        Failure
    }
    public abstract class Node{
        protected NodeState state;
        public Node parent;
        protected List<Node> children = new List<Node>();

        private static Dictionary<string,object> blackboard;
        public Node(){
            parent = null;
        }

        public Node(List<Node> children){
            foreach(var child in children){
                Attach(child);
            }
        }

        private void Attach(Node node){
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.Failure;

        public void SetData(string key, object value){
            blackboard[key] = value;
        }

        public object GetData(string key){
            blackboard.TryGetValue(key,out object value);
            return value;
        }


        public bool ClearData(string key){
            if (blackboard.ContainsKey(key)){
                blackboard.Remove(key);
                return true;
            }
            return false;
        }
    }
}