using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class SendModelData : ActionNode {
        int score = 100; /// always 100 for now
        ServerJSONManager _serverJSONManager;
        ServerJSONManager serverJSONManager {
            get {
                if (_serverJSONManager == null) {
                    _serverJSONManager = ServerJSONManager.instance.GetComponent<ServerJSONManager>();
                }
                return _serverJSONManager;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            serverJSONManager.SendLabeledPoint(score);
            return State.Success;
        }
    }
}
