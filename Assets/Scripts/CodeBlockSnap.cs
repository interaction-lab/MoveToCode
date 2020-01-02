using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {
        CodeBlock myCodeBlock;
        ManipulationHandler manipulationHandler;
        GameObject snapColliders;
        SnapCollider curSnapColliderInCollision;

        private void Awake() {
            myCodeBlock = GetComponent<CodeBlock>();
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(OnManipulationStart);
            manipulationHandler.OnManipulationEnded.AddListener(OnManipulationEnd);
            snapColliders = GetComponentInChildren<SnapColliders>()?.gameObject;
        }

        void OnManipulationStart(ManipulationEventData call) {
            Debug.Log(transform.name + " Selected");

            // deactivate outsideColliders
            snapColliders?.SetActive(false);
        }
        void OnManipulationEnd(ManipulationEventData call) {
            if (curSnapColliderInCollision != null) {
                curSnapColliderInCollision.DoSnapAction(curSnapColliderInCollision.GetMyCodeBlock(), GetMyCodeBlock());
                curSnapColliderInCollision.ExitCollisionRoutine();
            }

            snapColliders?.SetActive(true);
        }

        public bool IsSnappable() {
            return snapColliders != null && !snapColliders.activeSelf;
        }

        public void SetCollisionSnapCollider(SnapCollider snapColIn) {
            curSnapColliderInCollision = snapColIn;
        }

        public CodeBlock GetMyCodeBlock() {
            return myCodeBlock;
        }

    }
}