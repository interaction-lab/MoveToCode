using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LoggablePointToObj : LoggableBehPrimitive {

        #region members
        Transform objToPointTo, handTransform, shoulderTransform;
        // maxArmLength calced from shoulder to end of hand, this is roughly it
        // speed is in m/s
        float timeToPoint = 3f, startTime = 0f, maxArmLength = 0.3f, speed = 0.5f;
        ViewPortManager vpm;
        ViewPortManager ViewPortManagerInstance {
            get {
                if (vpm == null) {
                    vpm = ViewPortManager.instance;
                }
                return vpm;
            }
        }
        Vector3 origEndNormalized, origPosObjToPointTo;
        KuriArms kArms;
        TargetIKObject ikObj;
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

        protected override State OnUpdate() {

            if (Time.time - startTime > timeToPoint) {
                return State.Success;
            }

            // 1. see if the objtopointto position changed
            if (objToPointTo.position != origPosObjToPointTo) {
                CalcTransformForIK();
            }

            // 2. move the ik to the target over time using speed
            ikObj.transform.position = Vector3.MoveTowards(ikObj.transform.position, origEndNormalized, speed * Time.deltaTime);

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
        }

        private void SetMembers() {
            objToPointTo = blackboard.objToPointTo;
            startTime = Time.time;
            kArms = context.kuriArms;
            CalcWhichArm();
            CalcTransformForIK();
        }

        void CalcWhichArm() {
            // calculate the distance between the shoulder and the obj
            float leftDist = Vector3.Distance(kArms.LShoulder.position, objToPointTo.position);
            float rightDist = Vector3.Distance(kArms.RShoulder.position, objToPointTo.position);
            ikObj = leftDist < rightDist ? kArms.LeftIKTarget : kArms.RightIKTarget;
            handTransform = leftDist < rightDist ? kArms.LHand : kArms.RHand;
            shoulderTransform = leftDist < rightDist ? kArms.LShoulder : kArms.RShoulder;
        }
        private void CalcTransformForIK() {
            // get vector from shoulder to obj
            Vector3 shoulderToObj = objToPointTo.position - shoulderTransform.position;
            // normalize this vector to a max magnitude of maxArmLength
            if (shoulderToObj.magnitude > maxArmLength) {
                shoulderToObj = shoulderToObj.normalized * maxArmLength;
            }
            origEndNormalized = shoulderTransform.position + shoulderToObj;

            origPosObjToPointTo = objToPointTo.position;
        }
        #endregion
    }
}
