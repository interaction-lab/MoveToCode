using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public abstract class LoggableBehPrimitive : ActionNode {
        #region members
        Animator bAnim, aAnim;
        protected Animator BodyAnimator {
            get {
                if (bAnim == null) {
                    bAnim = context.mainAnimator;
                }
                return bAnim;
            }
        }
        protected Animator ArmAnimator {
            get {
                if (aAnim == null) {
                    aAnim = context.armAnimator;
                }
                return aAnim;
            }
        }
        protected int AddToArmAnimatorSemaphore = 0, AddToBodyAnimatorSemaphore = 0;

        public string actionName;
        public static string ActionStartedCol = "KTActionStarted";
        public static string ActionEndedCol = "KTActionEnded";
        LoggingManager lm;
        LoggingManager LoggingManagerInstance {
            get {
                if (lm == null) {
                    lm = LoggingManager.instance;
                }
                return lm;
            }
        }
        TutorKuriManager tkm;
        TutorKuriManager TutorKuriManagerInstance {
            get {
                if (tkm == null) {
                    tkm = TutorKuriManager.instance;
                }
                return tkm;
            }
        }
        bool initialized = false;
        #endregion
        #region abstract
        protected abstract void SetLogActionName();
        protected abstract void SetAnimatorSemaphoreCount();
        protected abstract void BehSetUp();
        protected abstract void BehCleanUp();
        #endregion
        #region behaviortree
        protected override void OnStart() {
            SetLogActionName();
            SetUpLogCols();

            LogActionStart();
            SetAnimatorSemaphoreCount();
            UpdateAnimators(1); // 1 is add to semaphore, used for things turning off the animators
            BehSetUp();

            initialized = true;
            TutorKuriManagerInstance.TimeLastActionStarted = Time.time;
        }
        protected override void OnStop() {
            if (initialized) { // guards against the original OnStart() not being called
                BehCleanUp();
                UpdateAnimators(-1); // -1 is remove from semaphore, used for things turning on the animators
                LogActionEnd();
                TutorKuriManagerInstance.TimeLastActionEnded = Time.time;
            }
        }
        #endregion
        #region helpers
        private void SetUpLogCols() {
            if (actionName == "") {
                Debug.LogWarning("No action name given to LogActionStarted");
            }
            if (!LoggingManagerInstance.GetColumnLookUp().ContainsKey(ActionStartedCol)) {
                LoggingManagerInstance.AddLogColumn(ActionStartedCol, "");
                LoggingManagerInstance.AddLogColumn(ActionEndedCol, "");
            }
        }
        protected void LogActionStart() {
            string logString = LoggingManagerInstance.GetValueInRowAt(ActionStartedCol);
            if (logString == "") {
                LoggingManagerInstance.UpdateLogColumn(ActionStartedCol, actionName);
            }
            else {
                LoggingManagerInstance.UpdateLogColumn(ActionStartedCol, string.Join(actionSeparator, logString, actionName));
            }

        }

        // Animator behaviors take zero to negative side of the semaphores
        protected void UpdateAnimators(int flipper) {
            blackboard.ArmAnimatorSemaphoreCount += AddToArmAnimatorSemaphore * flipper;
            blackboard.BodyAnimatorSemaphoreCount += AddToBodyAnimatorSemaphore * flipper;
            ArmAnimator.enabled = blackboard.ArmAnimatorSemaphoreCount <= 0;
            BodyAnimator.enabled = blackboard.BodyAnimatorSemaphoreCount <= 0;
        }

        protected void LogActionEnd() {
            string logString = LoggingManagerInstance.GetValueInRowAt(ActionEndedCol);
            if (logString == "") {
                LoggingManagerInstance.UpdateLogColumn(ActionEndedCol, actionName);
            }
            else {
                LoggingManagerInstance.UpdateLogColumn(ActionEndedCol, string.Join(actionSeparator, logString, actionName));
            }
        }
        #endregion
    }
}
