using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class IsConnectedToServer : ActionNode {
        ConnectionManager _connectionManager;
        ConnectionManager connectionManager {
            get {
                if (_connectionManager == null) {
                    _connectionManager = ConnectionManager.instance;
                }
                return _connectionManager;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return connectionManager.IsConnected ? State.Success : State.Failure;
        }
    }
}
