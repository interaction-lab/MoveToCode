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
        }

        public HashSet<SnapCollider> GetCurSnapCollidersInContact() {
            if (curSnapCollidersInContact == null) {
                curSnapCollidersInContact = new HashSet<SnapCollider>();
            }
            return curSnapCollidersInContact;
        }

        void OnManipulationStart(ManipulationEventData call) {
            mySnapColliders?.EnableAllCompatibleColliders();
            mySnapColliders?.DisableAllCollidersAndChildrenColliders();
            currentlyDraggingCBS = this;
        }

        IEnumerator DisableMyColliderForOneFrame() {
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
                GetCurSnapCollidersInContact().Add(bestCandidateSnapCollider);
            }
            else if (!GetCurSnapCollidersInContact().Empty()) {
                bestCandidateSnapCollider = curSnapCollidersInContact.ElementAt(0);
                bestCandidateSnapCollider.GetMeshOutline().enabled = true;
            }
        }

        public void RemoveAsCurSnapColliderInContact(SnapCollider sc) {
            GetCurSnapCollidersInContact().Remove(sc);
            if (sc == bestCandidateSnapCollider) {
                AddSnapColliderInContact(null);
            }
        }

        void OnManipulationEnd(ManipulationEventData call) { //let go of the block
            currentlyDraggingCBS = null;
            lastDraggedCBS = this;
            if (bestCandidateSnapCollider != null) { // within grey zone; SNAP ON
                bestCandidateSnapCollider.DoSnapAction(bestCandidateSnapCollider.GetMyCodeBlock(), GetMyCodeBlock());
                Block2TextConsoleManager.instance.UpdateConsoleOnSnap(); //refresh the Block2Text console when you ADD a block 
            }
            else {// outside of grey zone; SNAP OFF 
                // Remove when dragged away
                myCodeBlock.RemoveFromParentSnapCollider(true);
                Block2TextConsoleManager.instance.UpdateConsoleOnSnap(); //refresh the Block2Text console when you REMOVE a block
            }
            mySnapColliders?.DisableAllCompatibleColliders();
            GetCurSnapCollidersInContact().Clear();
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