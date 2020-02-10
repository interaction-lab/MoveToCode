using Microsoft.MixedReality.Toolkit.Utilities;
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
            GetMyCodeBlockObjectMesh().AlertInstructionSizeChanged();
        }

        // protected overriden methods
        protected override void SetupMeshOutline() {
            // set up on object mesh instead
            if (outlineMaterial == null) {
                outlineMaterial = Resources.Load<Material>(ResourcePathConstants.OutlineCodeBlockMaterial) as Material;
            }
            GetMyCodeBlockObjectMesh().SetUpMeshOutline(outlineMaterial);
        }

        public override void ToggleOutline(bool on) {
            if (!GetMyCodeBlockObjectMesh().IsOutlineSetUp()) {
                SetupMeshOutline();
            }
            GetMyCodeBlockObjectMesh().ToggleOutline(on);
        }


        // private helpers
        private void AddNewExitCodeBlock(CodeBlock newCodeBlock, Vector3 newLocalPosition) {
            exitCodeBlock = newCodeBlock;
            if (newCodeBlock) {
                newCodeBlock.transform.SnapToParent(GetMyCodeBlockObjectMesh().GetExitInstructionParentTransform(), newLocalPosition); // this needs to be bottom transform 
            }
            SetExitInstruction(newCodeBlock?.GetMyInstruction());
        }

        private void RemoveCurrentExitCodeBlock() {
            SetExitInstruction(null);
            if (exitCodeBlock != null) {
                exitCodeBlock.transform.localPosition = exitCodeBlock.transform.localPosition + new Vector3(0.25f, 0.25f, 1.25f); // TODO: This Placement
                exitCodeBlock.transform.SetParent(CodeBlockManager.instance.transform);
                exitCodeBlock = null;
            }
        }
    }
}