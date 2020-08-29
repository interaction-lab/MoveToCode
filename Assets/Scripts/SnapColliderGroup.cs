using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderGroup : MonoBehaviour {
        Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider> snapColliders; // this is in mycodblock already
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
            // get all my colliders; have them enable all their codeblocks snap colliders
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

        private Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider> GetSnapColliders() {
            return myCodeBlock.GetMyIArgument().GetArgToSnapColliderDict();
        }
    }
}