using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

// only keep track of last collided instead, highlight it
namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {

        /// <summary>
        /// CodeBlockSnap that is currently being grabbed by user
        /// </summary>
        public static CodeBlockSnap CurrentlyDraggingCodeBlockSnap;

        CodeBlock myCodeBlock;
        /// <summary>
        /// Pointer to my internal `CodeBlock`
        /// </summary>
        public CodeBlock MyCodeBlock {
            get {
                if (myCodeBlock == null) {
                    myCodeBlock = GetComponent<CodeBlock>();
                }
                return myCodeBlock;
            }
        }
        /// <summary>
        /// Manipulation Handler for events, look at `void OnManipulationStart(ManipulationEventData call)`
        /// </summary>
        ManipulationHandler manipulationHandler;

        SnapColliderGroup snapColliderGroup;

        /// <summary>
        /// Current set of `SnapCollider`s in contact with the dragging block
        /// </summary>
        HashSet<SnapCollider> curSnapCollidersInContact { get; set; } = new HashSet<SnapCollider>();

        /// <summary>
        /// Snapcollider that is decided on from the set of all collided with SnapColliders currently
        /// </summary>
        SnapCollider bestCandidateSnapCollider;

        private void Awake() {
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(OnManipulationStart);
            manipulationHandler.OnManipulationEnded.AddListener(OnManipulationEnd);
            snapColliderGroup = gameObject.GetComponentInChildrenOnlyDepthOne<SnapColliderGroup>();
            ResetCBS();
        }

        /// <summary>
        /// Set up for finding canidate SnapColliders when user grabs CodeBlock 
        /// </summary>
        /// <param name="call">Manipulation Event when user grabs block</param>
        void OnManipulationStart(ManipulationEventData call) {
            CurrentlyDraggingCodeBlockSnap = this;
            CodeBlockManager.instance.EnableCollidersCompatibleCodeBlock(MyCodeBlock);
            snapColliderGroup?.DisableAllCollidersAndChildrenColliders();
            // run raycast
            StartCoroutine(ShootRayFromHandThroughSnapColliders(call.Pointer));
        }

        IEnumerator ShootRayFromHandThroughSnapColliders(IMixedRealityPointer pointer)
        {
            RaycastHit rayHitData;
            while(CurrentlyDraggingCodeBlockSnap == this && pointer != null){
                Vector3 rayOrigin = pointer.Position;
                Vector3 direction = transform.position - rayOrigin;
                if(Physics.Raycast(rayOrigin,direction, out rayHitData)){
                    SnapCollider sc = rayHitData.collider.transform.GetComponent<SnapCollider>();
                    if(sc != null){
                        if(!curSnapCollidersInContact.Contains(sc)){
                            AddSnapColliderInContact(sc);
                        }
                    }
                }
                yield return null;
            }
        }

        /// <summary>
        /// Evaulates what snap action to take and then resets CBS
        /// </summary>
        /// <param name="call">End manipuation event</param>
        void OnManipulationEnd(ManipulationEventData call) {
            EvaluateBestCandidateCollider();
            CodeBlockManager.instance.DisableCollidersCompatibleCodeBlock(MyCodeBlock);
            ResetCBS();
            CurrentlyDraggingCodeBlockSnap = null;
        }

        /// <summary>
        /// Adds SnapCollder in contact to set and updates best snap collider based upon argument.
        /// See `UpdateBestSnapCollider(SnapCollider sc)` for more details.
        /// </summary>
        /// <param name="sc">Snap collider in contact. Can be null.</param>
        public void AddSnapColliderInContact(SnapCollider sc) {
            DisableLastBestCandidatesOutline();
            UpdateBestSnapCollider(sc);
        }

        /// <summary>
        /// If `sc` is not null, will update best candidate to `sc`
        /// If `sc` is null, will attempt to add any snapcolliders still in contact
        /// Due to the set up, best candidate will be null if both `sc` is null and
        /// the set of colliders in contact is empty.
        /// </summary>
        /// <param name="sc">Snap collider in contact. Can be null.</param>
        private void UpdateBestSnapCollider(SnapCollider sc) {
            bestCandidateSnapCollider = sc;
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.MyMeshOutline.enabled = true;
                curSnapCollidersInContact.Add(bestCandidateSnapCollider);
            }
            else if (!curSnapCollidersInContact.Empty()) {
                bestCandidateSnapCollider = curSnapCollidersInContact.ElementAt(0);
                bestCandidateSnapCollider.MyMeshOutline.enabled = true;
            }
        }

        /// <summary>
        /// Turns off outline of the last best snap collider
        /// </summary>
        private void DisableLastBestCandidatesOutline() {
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.MyMeshOutline.enabled = false;
            }
        }

        /// <summary>
        /// Removes SnapCollider from curSnapCollidersInContact set
        /// </summary>
        /// <param name="sc">SnapCollider to be removed. Cannot be null.</param>
        public void RemoveAsCurSnapColliderInContact(SnapCollider sc) {
            Assert.IsTrue(sc != null);
            curSnapCollidersInContact.Remove(sc);
            if (sc == bestCandidateSnapCollider) {
                AddSnapColliderInContact(null);
            }
        }

        /// <summary>
        /// Snaps to best non-null candidate.
        /// If candidate is null, removes from parent
        /// CurSnapCollidersInContact cannot be empty in this case 
        /// or else bestCandidate would just be whatever is in that set
        /// </summary>
        private void EvaluateBestCandidateCollider() {
            if (bestCandidateSnapCollider != null) {
                bestCandidateSnapCollider.DoSnapAction(MyCodeBlock);
            }
            else {
                Assert.IsTrue(curSnapCollidersInContact.Empty());
                MyCodeBlock.RemoveFromParentSnapCollider(true);
            }
        }

        /// <summary>
        /// Resets static state and removes snap collider candidates from set
        /// </summary>
        private void ResetCBS() {
            curSnapCollidersInContact.Clear();
            AddSnapColliderInContact(null);
        }

        private void OnEnable() {
            AddSnapColliderInContact(null);
        }

        private void OnDisable() {
            AddSnapColliderInContact(null);
        }
    }
}