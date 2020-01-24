using UnityEngine;

namespace MoveToCode {
    public abstract class ControlFlowCodeBlock : InstructionCodeBlock {
        CodeBlock exitCodeBlock;
        CodeBlockObjectMesh myCodeBlockObjectMesh;

        // public methods
        public void SetExitInstruction(Instruction iIn) {
            (myBlockInternalArg as ControlFlowInstruction).SetExitInstruction(iIn);
        }

        public CodeBlockObjectMesh GetMyCodeBlockObjectMesh() {
            if (myCodeBlockObjectMesh == null) {
                myCodeBlockObjectMesh = GetComponentInChildren<CodeBlockObjectMesh>();
            }
            return myCodeBlockObjectMesh;
        }

        public void SetExitCodeBlock(CodeBlock newExitInstuctionCodeBlock, Vector3 newLocalPosition) {
            RemoveCurrentExitCodeBlock();
            newExitInstuctionCodeBlock?.RemoveFromParentBlock();
            AddNewExitCodeBlock(newExitInstuctionCodeBlock, newLocalPosition);
        }

        public bool IsMyExitInstruction(Instruction iIn) {
            return iIn == (myBlockInternalArg as ControlFlowInstruction).GetExitInstruction();
        }

        public void AlertInstructionChanged() {
            GetMyCodeBlockObjectMesh().AlertInstructionChanged();
        }


        // private helpers
        private void AddNewExitCodeBlock(CodeBlock newCodeBlock, Vector3 newLocalPosition) {
            exitCodeBlock = newCodeBlock;
            if (newCodeBlock) {
                newCodeBlock.transform.SetParent(GetMyCodeBlockObjectMesh().GetExitInstructionParentTransform()); // this needs to be bottom transform 
                newCodeBlock.transform.localPosition = newLocalPosition; // TODO: once arg placing is done, update this for better placement
            }
            SetExitInstruction(newCodeBlock?.GetMyInstruction());
        }

        private void RemoveCurrentExitCodeBlock() {
            SetExitInstruction(null);
            if (exitCodeBlock != null) {
                exitCodeBlock.transform.localPosition = new Vector3(1.05f, 1.05f, 0); // TODO: This Placement
                exitCodeBlock.transform.SetParent(CodeBlockManager.instance.transform);
                exitCodeBlock = null;
            }
        }
    }
}