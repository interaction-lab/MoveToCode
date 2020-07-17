using System.Collections.Generic;
using UnityEngine;
namespace MoveToCode {
    public class CodeBlockManager : Singleton<CodeBlockManager> {
        HashSet<CodeBlock> codeBlocks;
        HashSet<SnapCollider> snapColliders;

        public HashSet<CodeBlock> GetAllCodeBlocks() {
            if (codeBlocks == null) {
                codeBlocks = new HashSet<CodeBlock>();
            }
            return codeBlocks;
        }
        public HashSet<SnapCollider> GetAllSnapColliders() {
            if (snapColliders == null) {
                snapColliders = new HashSet<SnapCollider>();
            }
            return snapColliders;
        }

        public void RegisterCodeBlock(CodeBlock cIn) {
            GetAllCodeBlocks().Add(cIn);
        }
        public void RegisterSnapCollider(SnapCollider sIn) {
            GetAllSnapColliders().Add(sIn);
        }

        public void DeregisterCodeBlock(CodeBlock cIn) {
            GetAllCodeBlocks().Remove(cIn);
        }
        public void DeregisterSnapCollider(SnapCollider sIn) {
            GetAllSnapColliders().Remove(sIn);
        }

        public void EnableCollidersCompatibleCodeBlock(CodeBlock cIn) {
            SetCompatibleColliderState(cIn, true);
        }
        // Maybe make this "disable all active colliders by keeping track of them?
        public void DisableCollidersCompatibleCodeBlock(CodeBlock cIn) {
            SetCompatibleColliderState(cIn, false);
        }

        private void SetCompatibleColliderState(CodeBlock cIn, bool desiredActiveState) {
            IArgument internalArg = cIn.GetMyInternalIArgument();
            foreach (SnapCollider sc in GetAllSnapColliders()) {
                if (sc.HasCompatibleType(internalArg) && !sc.GetMyCodeBlock().GetIsMenuBlock()) {
                    sc.gameObject.SetActive(desiredActiveState);
                }
            }
        }

        public void ResetAllCodeBlockInternalState() {
            foreach (CodeBlock c in GetAllCodeBlocks()) {
                c.ResetInstructionInternalState();
            }
        }

    }
}