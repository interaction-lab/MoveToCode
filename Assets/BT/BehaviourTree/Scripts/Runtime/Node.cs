using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveToCode;
using System.Linq;

namespace TheKiwiCoder {
    public abstract class Node : ScriptableObject {
        public enum State {
            Running,
            Failure,
            Success
        }

        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        [HideInInspector] public Context context;
        [HideInInspector] public Blackboard blackboard;
        [TextArea] public string description;
        public bool drawGizmos = false;

        LoggingManager lm;
        LoggingManager LoggingManagerInstance {
            get {
                if (lm == null) {
                    lm = LoggingManager.instance;
                }
                return lm;
            }
        }
        [HideInInspector] public string actionSeparator = ",";


        public State Update() {

            if (!started) {
                OnStart();
                started = true;
            }

            state = OnUpdate();
            LogNodeAndState();

            if (state != State.Running) {
                OnStop();
                started = false;
            }

            return state;
        }

        private void LogNodeAndState() {
            string nodeStr = LoggingManagerInstance.GetValueInRowAt(BehaviourTreeRunner.actionLogName);
            if (nodeStr != "") {
                nodeStr = string.Join(actionSeparator, nodeStr, this.name);
            }
            else {
                nodeStr = this.name;
            }
            LoggingManagerInstance.AddLogColumn(BehaviourTreeRunner.actionLogName, nodeStr);

            string stateStr = LoggingManagerInstance.GetValueInRowAt(BehaviourTreeRunner.actionLogState);
            if (stateStr != "") {
                stateStr = string.Join(actionSeparator, stateStr, state.ToString());
            }
            else {
                stateStr = state.ToString();
            }
            LoggingManagerInstance.AddLogColumn(BehaviourTreeRunner.actionLogState, stateStr);
        }

        public virtual Node Clone() {
            return Instantiate(this);
        }

        public void Abort() {
            BehaviourTree.Traverse(this, (node) => {
                node.started = false;
                node.state = State.Running;
                node.OnStop();
            });
        }

        public virtual void OnDrawGizmos() { }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}