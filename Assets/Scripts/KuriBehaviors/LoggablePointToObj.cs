using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoveToCode {
    public class LoggablePointToObj : LoggableBehPrimitive {

        #region members
        public static LoggablePointToObj CurPointToObj = null;
        Transform objToPointTo, handTransform, shoulderTransform;
        // maxArmLength calced from shoulder to end of hand, this is roughly it
        // speed is in m/s
        float timeToPoint, startTime = 0f, maxArmLength = 0.3f, speed = 0.5f;
        ViewPortManager vpm;
        ViewPortManager ViewPortManagerInstance {
            get {
                if (vpm == null) {
                    vpm = ViewPortManager.instance;
                }
                return vpm;
            }
        }
        TutorKuriTransformManager tktm;
        TutorKuriTransformManager TutorKuriTransformManagerInstance {
            get {
                if (tktm == null) {
                    tktm = TutorKuriTransformManager.instance;
                }
                return tktm;
            }
        }
        Vector3 origEndNormalized, origPosObjToPointTo, origStart, kuriOrigPos;
        KuriArms kArms;
        TargetIKObject ikObj;
        UnityEvent OnUntilInteract;
        bool UserInteracted = false, movingBackToOrigStart = false;

        #endregion
        #region overrides
        protected override void BehCleanUp() {
            if (CurPointToObj == this) {
                CurPointToObj = null;
            }
            if (objToPointTo != Camera.main.transform) {
                ViewPortManagerInstance.TurnOffArrow(objToPointTo);
            }
            if (OnUntilInteract != null) {
                OnUntilInteract.RemoveListener(UntilInteractListener);
            }
        }

        protected override void BehSetUp() {
            CurPointToObj = this;
            SetMembers();
            CreateAndSetArrowPoint();
        }

        protected override State OnUpdate() {
            if (CurPointToObj != this) {
                return State.Success; // quietly finish
            }
            if (movingBackToOrigStart &&
                Vector3.Distance(ikObj.transform.position, origStart) < 0.01f) { // phase 2 aka move back to orig start
                return State.Success;
            }
            else { // phase 1, aka point to obj
                if (Time.time - startTime > timeToPoint || UserInteracted || objToPointTo.gameObject.activeSelf == false) {
                    movingBackToOrigStart = true;
                    origEndNormalized = origStart; // hacky but should work
                }
            }

            // 1. see if the objtopointto position changed or if kuri has moved
            if (!movingBackToOrigStart &&
                    (objToPointTo.position != origPosObjToPointTo ||
                     TutorKuriTransformManagerInstance.Position != kuriOrigPos)) {
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
                    // check if image
                    if (objToPointTo.GetComponent<Image>() != null) {
                        inner = objToPointTo.GetComponent<Image>().color;
                    }
                    else if (objToPointTo.GetComponent<MeshRenderer>() != null) {
                        inner = objToPointTo.GetComponent<MeshRenderer>().material.color;
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
            timeToPoint = blackboard.timeToPoint;
            objToPointTo = blackboard.objToPointTo;
            startTime = Time.time;
            kArms = context.kuriArms;
            UserInteracted = false;
            movingBackToOrigStart = false;
            OnUntilInteract = context.eventRouter.GetEvent(EventNames.OnInteractWith);
            if (OnUntilInteract != null) {
                OnUntilInteract.AddListener(UntilInteractListener);
            }
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
            kuriOrigPos = TutorKuriTransformManagerInstance.Position;
            origStart = handTransform.position; // not perfect as it will depend where the hand has moved to but not going to add the body transform each time because fdlkj;djkslf;
        }

        void UntilInteractListener() {
            UserInteracted = true;
        }
        #endregion
    }
}
