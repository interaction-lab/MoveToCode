using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {
        CodeBlock myCodeBlock;
        ManipulationHandler manipulationHandler;
        SnapColliders mySnapColliders;
        HashSet<SnapCollider> curSnapCollidersInCollision;

        private void Awake() {
            myCodeBlock = GetComponent<CodeBlock>();
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(OnManipulationStart);
            manipulationHandler.OnManipulationEnded.AddListener(OnManipulationEnd);
            mySnapColliders = GetComponentInChildren<SnapColliders>();
            curSnapCollidersInCollision = new HashSet<SnapCollider>();
        }

        void OnManipulationStart(ManipulationEventData call) {
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

        void OnManipulationEnd(ManipulationEventData call) {
            if (!curSnapCollidersInCollision.Empty()) {
                SnapCollider closestCollider = CalculateClosestSnapCollider();
                closestCollider.DoSnapAction(closestCollider?.GetMyCodeBlock(), GetMyCodeBlock());
                ClearAndResetAllColliders();
            }
            else {
                // TODO: check if still in collision with current arg and snap back to it?
                myCodeBlock.RemoveFromParentBlock();
            }
            mySnapColliders?.EnableAllCollidersAndChildrenColliders();
        }

        // Requires Hashset to not be empty
        SnapCollider CalculateClosestSnapCollider() {
            SnapCollider result = null;
            float closestDist = -1;
            foreach (SnapCollider sc in curSnapCollidersInCollision) {
                float tempDist = Vector3.Distance(sc.transform.position, transform.position);
                if (result == null || closestDist > tempDist) {
                    result = sc;
                    closestDist = tempDist;
                }
            }
            return result;
        }

        void ClearAndResetAllColliders() {
            foreach (SnapCollider sc in curSnapCollidersInCollision.ToList()) {
                sc.ExitCollisionRoutine();
            }
            curSnapCollidersInCollision.Clear();
        }

        public void AddCollisionSnapCollider(SnapCollider snapColIn) {
            curSnapCollidersInCollision.Add(snapColIn);
        }

        public void RemoveCollisionSnapCollider(SnapCollider snapColIn) {
            curSnapCollidersInCollision.Remove(snapColIn);
        }

        public CodeBlock GetMyCodeBlock() {
            return myCodeBlock;
        }

    }
}