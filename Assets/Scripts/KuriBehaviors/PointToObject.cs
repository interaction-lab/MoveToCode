using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class PointToObject : ActionNode {
        KuriController KController;
        Transform objToPointTo, ikTransform;
        Vector3 startPos, endPos;
        private float speed;
        float timeToPoint = 3f, startTime = 0f;
        private float maxArmVectorLength = 1; // idk just guessing here lol
        ViewPortManager vpm;
        ViewPortManager ViewPortManagerInstance {
            get {
                if (vpm == null) {
                    vpm = ViewPortManager.instance;
                }
                return vpm;
            }
        }

        protected override void OnStart() {
            KController = context.KController;
            objToPointTo = blackboard.objToPointTo;
            ikTransform = KController.IkObjRight.transform;
            startPos = ikTransform.position;
            endPos = objToPointTo.position;
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
                ikTransform.position = objToPointTo.position;
            }
            startTime = Time.time;
        }

        protected override void OnStop() {
            ViewPortManagerInstance.TurnOffArrow(objToPointTo);
        }

        protected override State OnUpdate() {
            if (Time.time - startTime > timeToPoint) {
                return State.Success;
            }
            return State.Running;
        }
    }
}
