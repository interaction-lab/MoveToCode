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

        public void DisableAllCompatibleColliders() {
            CodeBlockManager.instance.DisableCollidersCompatibleCodeBlock(GetMyCodeBlock());
        }

        // Private methods
        private void SetCollidersAndChildrenState(bool desiredActiveState) {
            foreach (SnapCollider sc in GetSnapColliders()) {
                sc.gameObject.SetActive(desiredActiveState);
            }
            foreach (KeyValuePair<IARG, CodeBlock> kvp in GetMyCodeBlock().GetArgDictAsCodeBlocks()) {
                if (desiredActiveState) {
                    kvp.Value.GetSnapColliders()?.EnableAllCollidersAndChildrenColliders();
                }
                else {
                    kvp.Value?.GetSnapColliders()?.DisableAllCollidersAndChildrenColliders();
                }
            }
        }

        private CodeBlock GetMyCodeBlock() {
            if (myCodeBlock == null) {
                myCodeBlock = transform.parent.GetComponent<CodeBlock>();
            }
            return myCodeBlock;
        }

        public SnapCollider GetSnapColliderAtPos(int pos)
        {
            return GetSnapColliders()[pos];
        }

        // TODO: this should be register/deregister
        private List<SnapCollider> GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = new List<SnapCollider>();
                snapColliders.Resize(GetMyCodeBlock().GetMyIArgument().GetNumArguments());
                foreach (Transform go in transform) {
                    SnapCollider sc = go.GetComponentInChildren<SnapCollider>(true);
                    if (sc != null)
                    {
                        snapColliders[sc.myArgumentPosition] = sc;
                    }
                }
            }
            return snapColliders;
        }
    }
}