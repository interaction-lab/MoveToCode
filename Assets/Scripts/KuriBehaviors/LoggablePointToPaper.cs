using UnityEngine;

namespace MoveToCode {
    public class LoggablePointToPaper : LoggableBehPrimitive {
        MazePaper mpp;
        MazePaper MazePaperInstance {
            get {
                if (mpp == null) {
                    mpp = MazePaper.instance;
                }
                return mpp;
            }
        }
        float timeToPoint, startTime;
        protected override void BehCleanUp() {
            MazePaperInstance.TurnOff();
        }

        protected override void BehSetUp() {
            MazePaperInstance.TurnOn(blackboard.pointToPaperName);
            timeToPoint = blackboard.timeToPoint;
            startTime = Time.time;
        }
        #region members
        #endregion
        #region overrides
        protected override State OnUpdate() {
            if (Time.time - startTime > timeToPoint) {
                return State.Success;
            }
            return State.Running;
        }

        protected override void SetAnimatorSemaphoreCount() {
            // no need for any of the animators
        }

        protected override void SetLogActionName() {
            actionName = string.Join(Separator, EventNames.OnPointToPaper, blackboard.pointToPaperName);
        }
        #endregion
        #region helpers
        #endregion
    }
}
