using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderGroup : MonoBehaviour {
        List<SnapCollider> snapColliders;
        CodeBlock myCodeBlock;

        // public methods
        public void DisableAllCollidersAndChildrenColliders() {
            SetCollidersAndChildrenState(false);
        }

        public void EnableAllCollidersAndChildrenColliders() {
            SetCollidersAndChildrenState(true);
        }

        public void EnableAllCompatibleColliders() {
            CodeBlockManager.instance.EnableCollidersCompatibleCodeBlock(GetMyCodeBlock());
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
                if (GetComponent<CodeBlockObjectMesh>()) {
                    snapColliders = new List<SnapCollider>();
                    foreach (Transform go in transform) {
                        snapColliders.AddRange(go.GetComponentsInChildren<SnapCollider>());
                    }
                }
                else {
                    snapColliders = new List<SnapCollider>(transform.GetComponentsInChildren<SnapCollider>());
                }
            }
            return snapColliders;
        }
    }
}