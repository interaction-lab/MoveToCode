using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveToCode;
using System.Linq;
using UnityEditor.Experimental.GraphView;

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

        [HideInInspector] public static string actionSeparator = ","; // this is ridiculous that this is here but going to leave it because it shouldn't* be an issue


        public State Update() {

            if (!started) {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state != State.Running) {
                OnStop();
                started = false;
            }

            return state;
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

        public void AbortOnlyRunningNodes() {
            BehaviourTree.Traverse(this, (node) => {
                if (node.started) {
                    node.started = false;
                    node.state = State.Running;
                    node.OnStop();
                }
            });
        }
        public virtual void OnDrawGizmos() { }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}