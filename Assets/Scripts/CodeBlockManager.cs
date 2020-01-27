using System.Collections.Generic;

namespace MoveToCode {
    public class CodeBlockManager : Singleton<CodeBlockManager> {
        HashSet<CodeBlock> codeBlocks;
        HashSet<SnapCollider> snapColliders;

        private void Awake() {
            codeBlocks = new HashSet<CodeBlock>();
            snapColliders = new HashSet<SnapCollider>();
        }

        public HashSet<CodeBlock> GetAllCodeBlocks() {
            return codeBlocks;
        }
        public HashSet<SnapCollider> GetAllSnapColliders() {
            return snapColliders;
        }

        public void RegisterCodeBlock(CodeBlock cIn) {
            codeBlocks.Add(cIn);
        }
        public void RegisterSnapCollider(SnapCollider sIn) {
            snapColliders.Add(sIn);
        }

        public void DeregisterCodeBlock(CodeBlock cIn) {
            codeBlocks.Remove(cIn);
        }
        public void DeregisterSnapCollider(SnapCollider sIn) {
            snapColliders.Remove(sIn);
        }
    }
}