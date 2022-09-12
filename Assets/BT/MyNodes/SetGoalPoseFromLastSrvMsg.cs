using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Newtonsoft.Json;

namespace MoveToCode {
    public class SetGoalPoseFromLastSrvMsg : ActionNode {
        ConnectionManager connectionManager;
        ConnectionManager ConnectionManager_ {
            get {
                if (connectionManager == null) {
                    connectionManager = ConnectionManager.instance;
                }
                return connectionManager;
            }
        }

        KuriTransformManager kuriT;
        KuriTransformManager KuriT_ {
            get {
                if (kuriT == null) {
                    kuriT = KuriManager.instance.GetComponent<KuriTransformManager>();
                }
                return kuriT;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Vector3 newGoal;
            List<float> kuriCordList = new List<float>();
            string lastMsg = ConnectionManager_.LatestMsg;
            if (lastMsg != null && !lastMsg.Contains("point_id")) {
                ServerPointResponseJSON serverPointResponseJSON = JsonConvert.DeserializeObject<ServerPointResponseJSON>(lastMsg);
                kuriCordList = serverPointResponseJSON.point;
                newGoal = new Vector3(kuriCordList[0], 0, kuriCordList[1]);
                // transform back into meters from cm
                newGoal *= 0.01f;
                // transform back to world cords by adding Kuri position
                newGoal += KuriT_.Position;
                // set on the ground
                newGoal.y = KuriT_.GroundYCord;
                blackboard.goalPosition = newGoal;
                DebugTextManager_.SetDebugText("Goal position: " + newGoal.ToString());
                return State.Success;
            }
            return State.Failure;
        }
    }
}
