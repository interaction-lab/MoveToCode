using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliders : MonoBehaviour {
        List<SnapCollider> snapColliders;
        CodeBlock myCodeBlock;
        public void DisableAllCollidersAndChildrenColliders() {
            SetCollidersAndChildrenState(false);
        }

        public void EnableAllCollidersAndChildrenColliders() {
            SetCollidersAndChildrenState(true);
        }


        // Private methods
        private void SetCollidersAndChildrenState(bool desiredActiveState) {
            foreach (SnapCollider sc in GetSnapColliders()) {
                sc.gameObject.SetActive(desiredActiveState);
            }
            foreach (CodeBlock c in GetMyCodeBlock().GetAllAttachedCodeBlocks()) {
                if (desiredActiveState) {
                    c.GetSnapColliders()?.EnableAllCollidersAndChildrenColliders();
                }
                else {
                    c.GetSnapColliders()?.DisableAllCollidersAndChildrenColliders();
                }
            }
        }

        private CodeBlock GetMyCodeBlock() {
            if (myCodeBlock == null) {
                myCodeBlock = transform.parent.GetComponent<CodeBlock>();
            }
            return myCodeBlock;
        }

        private List<SnapCollider> GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = new List<SnapCollider>(transform.GetComponentsInChildren<SnapCollider>());
            }
            return snapColliders;
        }
    }
}