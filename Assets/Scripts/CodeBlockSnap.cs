using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// only keep track of last collided instead, highlight it
namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {
        public static CodeBlockSnap currentlyDraggingCBS, lastDraggedCBS;
        /// <summary>
        /// Pointer to my internal `CodeBlock`
        /// </summary>
        CodeBlock myCodeBlock;
        /// <summary>
        /// Manipulation Handler for events, look at `void OnManipulationStart(ManipulationEventData call)`
        /// </summary>
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
            currentlyDraggingCBS = this;
            CodeBlockManager.instance.EnableCollidersCompatibleCodeBlock(GetMyCodeBlock());
            mySnapColliders?.DisableAllCollidersAndChildrenColliders();
        }

        IEnumerator DisableMyColliderForOneFrame() {
            GetMyCodeBlock().ToggleColliders(false);
            yield return new WaitForFixedUpdate();
            GetMyCodeBlock().ToggleColliders(true);
        }

        public void AddSnapColliderInContact(SnapCollider sc) {
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.MyMeshOutline.enabled = false;
            }
            bestCandidateSnapCollider = sc;
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.MyMeshOutline.enabled = true;
                GetCurSnapCollidersInContact().Add(bestCandidateSnapCollider);
            }
            else if (!GetCurSnapCollidersInContact().Empty()) {
                bestCandidateSnapCollider = curSnapCollidersInContact.ElementAt(0);
                bestCandidateSnapCollider.MyMeshOutline.enabled = true;
            }
        }

        public void RemoveAsCurSnapColliderInContact(SnapCollider sc) {
            GetCurSnapCollidersInContact().Remove(sc);
            if (sc == bestCandidateSnapCollider) {
                AddSnapColliderInContact(null);
            }
        }

        void OnManipulationEnd(ManipulationEventData call) {
            currentlyDraggingCBS = null;
            lastDraggedCBS = this;
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.DoSnapAction(bestCandidateSnapCollider.MyCodeBlock, GetMyCodeBlock());
            }
            else {
                myCodeBlock.RemoveFromParentSnapCollider(true);
            }
            //Block2TextConsoleManager.instance.UpdateConsoleOnSnap(); // INF LOOP ISSUE
            CodeBlockManager.instance.DisableCollidersCompatibleCodeBlock(GetMyCodeBlock());
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