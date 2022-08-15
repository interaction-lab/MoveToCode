using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRISVTE;

namespace TheKiwiCoder {
    public class MonitorSequence : CompositeNode {
        protected int currentNonMonitorRunning;
        List<int> monitorChildren = new List<int>();

        protected override void OnStart() {
            monitorChildren.Clear();
            int i = 0;
            foreach(var child in children){
                if(child is MonitorCondition || child is MonitorDecorator){
                    monitorChildren.Add(i);
                }
                ++i;
            }
            currentNonMonitorRunning = 0;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            bool monitorsPassed = CheckAllMonitorChildren();
            
            if(!monitorsPassed){
                return State.Failure;
            }

            for (int i = currentNonMonitorRunning; i < children.Count; ++i) {
                currentNonMonitorRunning = i;
                var child = children[currentNonMonitorRunning];
                if(child is MonitorCondition || child is MonitorDecorator){
                    continue;
                }

                switch (child.Update()) {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        continue;
                }
            }

            return State.Success;
        }

        private bool CheckAllMonitorChildren(){
            foreach(var index in monitorChildren){
                if(children[index].Update() == State.Failure){
                    return false;
                }
            }
            return true;
        }
    }
}