using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LoggablePointToObj : LoggableBehPrimitive {

        #region members
        KuriController KController;
        Transform objToPointTo, ikTransform;
        float timeToPoint = 3f, startTime = 0f;
        ViewPortManager vpm;
        ViewPortManager ViewPortManagerInstance {
            get {
                if (vpm == null) {
                    vpm = ViewPortManager.instance;
                }
                return vpm;
            }
        }
        #endregion
        #region overrides
        protected override void BehCleanUp() {
            if (objToPointTo != Camera.main.transform) {
                ViewPortManagerInstance.TurnOffArrow(objToPointTo);
            }
        }

        protected override void BehSetUp() {
            SetMembers();
            CreateAndSetArrowPoint();
        }

        // TODO: make this move over time in arm range and then back to original position
        protected override State OnUpdate() {
            if (Time.time - startTime > timeToPoint) {
                return State.Success;
            }
            return State.Running;
        }

        protected override void SetAnimatorSemaphoreCount() {
            AddToArmAnimatorSemaphore = 1;
        }

        protected override void SetLogActionName() {
            actionName = string.Join(Separator,
                EventNames.OnPointToObj,
                blackboard.objToPointTo.name);
        }
        #endregion
        #region helpers
        private void CreateAndSetArrowPoint() {
            if (objToPointTo != Camera.main.transform) {
                if (ViewPortManagerInstance.GetArrowPoint(objToPointTo) == null) {
                    Color outter = Color.white;
                    Color inner = Color.black;
                    Material mat = objToPointTo.GetComponent<MeshRenderer>()?.material;
                    if (mat != null) {
                        inner = mat.color;
                    }
                    if (inner == Color.black) {
                        outter = Color.white;
                    }

                    string nameT = objToPointTo.name;
                    nameT = char.ToUpper(nameT[0]) + nameT.Substring(1);
                    ViewPortManagerInstance.SpawnNewArrowPoint(
                        objToPointTo,
                        Vector3.zero,
                        outter,
                        inner,
                        nameT + " Is Behind You");
                }
                ViewPortManagerInstance.TurnOnArrow(objToPointTo);
            }
            ikTransform.position = objToPointTo.position;
        }

        private void SetMembers() {
            KController = context.KController;
            objToPointTo = blackboard.objToPointTo;
            ikTransform = KController.IkObjRight.transform;
            startTime = Time.time;
        }
        #endregion
    }
}
