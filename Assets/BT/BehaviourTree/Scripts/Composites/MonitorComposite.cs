using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NRISVTE;

namespace TheKiwiCoder {
    public class MonitorComposite : CompositeNode {
        List<State> childrenLeftToExecute = new List<State>();
        List<int> childrenMonitorConditionsIndices = new List<int>();
        HashSet<int> childrenMonitorDecoratorsIndices = new HashSet<int>();

        protected override void OnStart() {
            childrenLeftToExecute.Clear();
            children.ForEach(a => {
                childrenLeftToExecute.Add(State.Running);
            });

            childrenMonitorConditionsIndices.Clear();
            childrenMonitorDecoratorsIndices.Clear();
            int i = 0;
            foreach(var child in children) {
                if(child is MonitorCondition || child is MonitorDecorator) {
                    childrenMonitorConditionsIndices.Add(i);
                }
                if(child is MonitorDecorator){
                    childrenMonitorDecoratorsIndices.Add(i);
                }
                ++i;
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Count(); ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    var status = children[i].Update();
                    if (status == State.Failure) {
                        AbortRunningChildren();
                        return State.Failure;
                    }
                    // skip over any monitor decorators (allows repeat checking of conditions)
                    if (status == State.Running && !childrenMonitorDecoratorsIndices.Contains(i)) {
                        stillRunning = true;
                    }

                    childrenLeftToExecute[i] = status;
                }
            }
            // reset all childrenMonitorConditions to running
            foreach(var index in childrenMonitorConditionsIndices) {
                childrenLeftToExecute[index] = State.Running;
            }
            return stillRunning ? State.Running : State.Success;
        }

        void AbortRunningChildren() {
            for (int i = 0; i < childrenLeftToExecute.Count(); ++i) {
                if (childrenLeftToExecute[i] == State.Running) {
                    children[i].Abort();
                }
            }
        }
    }
}