using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderGroup : MonoBehaviour {

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
            foreach (KeyValuePair<string, SnapCollider> scKV in GetSnapColliders()) {
                scKV.Value.gameObject.SetActive(desiredActiveState);
                if (scKV.Value.HasCodeBlockArgAttached()) {
                    (desiredActiveState ?
                    new Action(scKV.Value.GetMyCodeBlockArg().GetSnapColliderGroup().EnableAllCollidersAndChildrenColliders) :
                              scKV.Value.GetMyCodeBlockArg().GetSnapColliderGroup().DisableAllCollidersAndChildrenColliders)();
                } // here
            }
        }

        private CodeBlock GetMyCodeBlock() {
            if (myCodeBlock == null) {
                myCodeBlock = transform.parent.GetComponent<CodeBlock>();
            }
            return myCodeBlock;
        }

        private Dictionary<string, SnapCollider> GetSnapColliders() {
            return myCodeBlock.GetMyIArgument().GetArgToSnapColliderDict();
        }
    }
}