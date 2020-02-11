using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// only keep track of last collided instead, highlight it
namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {
        public static CodeBlockSnap currentlyDraggingCBS, lastDraggedCBS;
        CodeBlock myCodeBlock;
        ManipulationHandler manipulationHandler;
        SnapColliderGroup mySnapColliders;
        HashSet<SnapCollider> curSnapCollidersInContact;
        SnapCollider bestCandidateSnapCollider;

        private void Awake() {
            myCodeBlock = GetComponent<CodeBlock>();
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(OnManipulationStart);
            manipulationHandler.OnManipulationEnded.AddListener(OnManipulationEnd);
            mySnapColliders = GetComponentInChildren<SnapColliderGroup>();
            curSnapCollidersInContact = new HashSet<SnapCollider>();
        }

        void OnManipulationStart(ManipulationEventData call) {
            // enable all of the snap colliders that are ok
            mySnapColliders?.EnableAllCompatibleColliders();
            mySnapColliders?.DisableAllCollidersAndChildrenColliders();
            currentlyDraggingCBS = this;
            // Disable my collider, wait one frame, reenable
            // This allows for a "resnap" to same spot
            //StartCoroutine(DisableMyColliderForOneFrame());
        }

        IEnumerator DisableMyColliderForOneFrame() {
            // get meshoutline, disable colliders
            GetMyCodeBlock().ToggleColliders(false);
            yield return new WaitForFixedUpdate();
            GetMyCodeBlock().ToggleColliders(true);
        }

        public void AddSnapColliderInContact(SnapCollider sc) {
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.GetMeshOutline().enabled = false;
            }
            bestCandidateSnapCollider = sc;
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.GetMeshOutline().enabled = true;
                curSnapCollidersInContact.Add(bestCandidateSnapCollider);
            }
            else if (!curSnapCollidersInContact.Empty()) {
                bestCandidateSnapCollider = curSnapCollidersInContact.ElementAt(0);
                bestCandidateSnapCollider.GetMeshOutline().enabled = true;
            }
        }

        public void RemoveAsCurSnapColliderInContact(SnapCollider sc) {
            curSnapCollidersInContact.Remove(sc);
            if (sc == bestCandidateSnapCollider) {
                AddSnapColliderInContact(null);
            }
        }

        void OnManipulationEnd(ManipulationEventData call) {
            currentlyDraggingCBS = null;
            lastDraggedCBS = this;
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.DoSnapAction(bestCandidateSnapCollider.GetMyCodeBlock(), GetMyCodeBlock());
            }
            else {
                // Remove when dragged away
                myCodeBlock.RemoveFromParentBlock();
            }
            mySnapColliders?.DisableAllCompatibleColliders();
            curSnapCollidersInContact.Clear();
            AddSnapColliderInContact(null);
        }

        public CodeBlock GetMyCodeBlock() {
            return myCodeBlock;
        }

        private void OnEnable() {
            AddSnapColliderInContact(null);
        }

        private void OnDisable() {
            AddSnapColliderInContact(null);
        }

    }
}