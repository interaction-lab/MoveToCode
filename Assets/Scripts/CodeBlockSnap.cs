using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// only keep track of last collided instead, highlight it
namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {
        CodeBlock myCodeBlock;
        ManipulationHandler manipulationHandler;
        SnapColliderGroup mySnapColliders;
        SnapCollider curSnapColliderInContact;

        private void Awake() {
            myCodeBlock = GetComponent<CodeBlock>();
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(OnManipulationStart);
            manipulationHandler.OnManipulationEnded.AddListener(OnManipulationEnd);
            mySnapColliders = GetComponentInChildren<SnapColliderGroup>();
        }

        void OnManipulationStart(ManipulationEventData call) {
            // enable all of the snap colliders that are ok
            mySnapColliders?.EnableAllCompatibleColliders();
            mySnapColliders?.DisableAllCollidersAndChildrenColliders();
            // Disable my collider, wait one frame, reenable
            // This allows for a "resnap" to same spot
            StartCoroutine(DisableMyColliderForOneFrame());
        }

        IEnumerator DisableMyColliderForOneFrame() {
            Collider myCollider = GetComponent<Collider>();
            myCollider.enabled = false;
            yield return new WaitForFixedUpdate();
            myCollider.enabled = true;
        }

        public void SetCurSnapColliderInContact(SnapCollider sc) {
            if (curSnapColliderInContact != null) {
                curSnapColliderInContact.GetMeshOutline().enabled = false;
            }
            curSnapColliderInContact = sc;
            if (curSnapColliderInContact != null) {
                curSnapColliderInContact.GetMeshOutline().enabled = true;
            }
        }

        public void RemoveAsCurSnapColliderInContact(SnapCollider sc) {
            if (sc == curSnapColliderInContact) {
                SetCurSnapColliderInContact(null);
            }
        }

        void OnManipulationEnd(ManipulationEventData call) {
            if (curSnapColliderInContact != null) {
                curSnapColliderInContact.DoSnapAction(curSnapColliderInContact.GetMyCodeBlock(), GetMyCodeBlock());
            }
            else {
                // Remove when dragged away
                myCodeBlock.RemoveFromParentBlock();
            }
            mySnapColliders?.DisableAllCompatibleColliders();
        }

        public CodeBlock GetMyCodeBlock() {
            return myCodeBlock;
        }

    }
}