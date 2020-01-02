using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {
        CodeBlock myCodeBlock;
        ManipulationHandler manipulationHandler;
        SnapColliders snapColliders;
        SnapCollider curSnapColliderInCollision;

        // TODO: On grabbing me, disable all hitboxes of mine as well as children


        private void Awake() {
            myCodeBlock = GetComponent<CodeBlock>();
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(OnManipulationStart);
            manipulationHandler.OnManipulationEnded.AddListener(OnManipulationEnd);
            snapColliders = GetComponentInChildren<SnapColliders>();
        }

        void OnManipulationStart(ManipulationEventData call) {
            Debug.Log(transform.name + " Selected");

            // deactivate outsideColliders
            snapColliders.DisableAllCollidersAndChildrenColliders();
        }
        void OnManipulationEnd(ManipulationEventData call) {
            if (curSnapColliderInCollision != null) {
                curSnapColliderInCollision.DoSnapAction(curSnapColliderInCollision?.GetMyCodeBlock(), GetMyCodeBlock());
                curSnapColliderInCollision.ExitCollisionRoutine();
            }

            snapColliders?.EnableAllCollidersAndChildrenColliders();
        }

        public void SetCollisionSnapCollider(SnapCollider snapColIn) {
            curSnapColliderInCollision = snapColIn;
        }

        public CodeBlock GetMyCodeBlock() {
            return myCodeBlock;
        }

    }
}