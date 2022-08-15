using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class RequestNextSamplePoint : ActionNode {
        InteractionManager interactionManager;
        InteractionManager InteractionManager_ {
            get {
                if (interactionManager == null) {
                    interactionManager = InteractionManager.instance;
                }
                return interactionManager;
            }
        }
        ServerJSONManager serverJSONManager;
        ServerJSONManager ServerJSONManage_ {
            get {
                if (serverJSONManager == null) {
                    serverJSONManager = ServerJSONManager.instance;
                }
                return serverJSONManager;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            ServerJSONManage_.RequestNextSamplePoint();
            return State.Success;
        }
    }
}
