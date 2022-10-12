using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LoggableTurnToObj : LoggableBehPrimitive {

        #region members
        Transform objTransform, origTransform, mazePaperT;
        TutorKuriTransformManager kuriTransformManager;
        KuriBTBodyController kuriBodyController;
        public static LoggableTurnToObj CurTurnToObj = null; // protect againist multiple running at once, default to whoever was running later

        float turnSpeed;
        #endregion
        #region overrides
        protected override void BehCleanUp() {
            if (CurTurnToObj == this) {
                CurTurnToObj = null;
            }
        }
        protected override void BehSetUp() {
            CurTurnToObj = this;
            mazePaperT = MazePaper.instance.transform;
            Init();
        }

        protected override State OnUpdate() {
            if (CurTurnToObj != this) {
                return State.Success; // quietly finish
            }
            if (objTransform == null || objTransform == mazePaperT) { // because mazepaper is within the kuri body, it will forever turn, I would make this search for anyting below or do a chekc earlier but that would take a lot of time
                return State.Failure;
            }

            if (objTransform != origTransform) {
                Init();
            }

            if (!LookingAtThisObj() &&
                !kuriTransformManager.IsWithinHeadPanConstraints()) {
                // tell kuri controller to turn at this object
                kuriBodyController.OnlyLookAtObj(objTransform);
            }

            Vector3 objPos = objTransform.position;
            Vector3 kuriPos = kuriTransformManager.Position;
            objPos.y = kuriPos.y;
            Vector3 targetDir = objPos - kuriPos;
            if (Vector3.Angle(kuriTransformManager.Forward, targetDir) < 1f) {
                return State.Success;
            }

            float step = turnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(kuriTransformManager.Forward, targetDir, step, 0.0f);
            kuriTransformManager.Rotation = Quaternion.LookRotation(newDir);


            return State.Running;
        }

        // Note this isn't perfect since the obj is never reset unless a new looking at is passed in
        private bool LookingAtThisObj() {
            return blackboard.objToLookAt == objTransform && LoggableLookAtObj.CurLoggableLookAtObj != null;
        }

        protected override void SetAnimatorSemaphoreCount() {
            AddToBodyAnimatorSemaphore = 1;
        }

        protected override void SetLogActionName() {
            actionName = string.Join(Separator,
                EventNames.OnTurnToObj,
                blackboard.objToTurnTo.name);
        }
        #endregion
        #region helpers
        void Init() {
            kuriTransformManager = context.kuriTransformManager;
            kuriBodyController = context.KController as KuriBTBodyController;
            turnSpeed = blackboard.bodySpeed;
            SetObjToTurnTo();
        }

        void SetObjToTurnTo() {
            objTransform = blackboard.objToTurnTo;
            origTransform = objTransform;
        }
        #endregion
    }
}
