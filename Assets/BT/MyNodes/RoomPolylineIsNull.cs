using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class RoomPolylineIsNull : ActionNode {
        FakeWallRoomPolylineEstimator roomPolylineEstimator;
        FakeWallRoomPolylineEstimator RoomEstimator {
            get {
                if (roomPolylineEstimator == null) {
                    roomPolylineEstimator = FakeWallRoomPolylineEstimator.instance;
                }
                return roomPolylineEstimator;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return RoomEstimator.GetWallPolyLines().Count >= 4 ?
            State.Success :
            State.Failure;
        }
    }
}
