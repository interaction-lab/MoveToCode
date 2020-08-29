using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderGroup : MonoBehaviour {
        Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider> snapColliders;
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
            foreach (KeyValuePair<SNAPCOLTYPEDESCRIPTION, SnapCollider> scKV in GetSnapColliders()) {
                scKV.Value.gameObject.SetActive(desiredActiveState);
            }
            foreach (KeyValuePair<SNAPCOLTYPEDESCRIPTION, CodeBlock> kvp in GetMyCodeBlock().GetArgDictAsCodeBlocks()) {
                (desiredActiveState ?
                    new Action(kvp.Value.GetSnapColliders().EnableAllCollidersAndChildrenColliders) :
                               kvp.Value.GetSnapColliders().DisableAllCollidersAndChildrenColliders)();

            }
        }

        private CodeBlock GetMyCodeBlock() {
            if (myCodeBlock == null) {
                myCodeBlock = transform.parent.GetComponent<CodeBlock>();
            }
            return myCodeBlock;
        }


        // TODO: this should be register/deregister
        private Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider> GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = new Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider>();
                foreach (Transform go in transform) {
                    SnapCollider sc = go.GetComponentInChildren<SnapCollider>(true);
                    if (sc != null) {
                        snapColliders[sc.GetIArgType()] = sc;
                    }
                }
            }
            return snapColliders;
        }
    }
}