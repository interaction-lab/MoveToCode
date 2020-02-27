using System;
using UnityEngine;


namespace MoveToCode {
    public class Exercise : MonoBehaviour {
        public string consoleStringGoal;
        public string[] varNames;
        public int[] values;


        public bool IsExerciseCorrect() {
            return ConsoleManager.instance.GetCleanedMainText() == consoleStringGoal;
        }

        private void SnapAllBlocksToBlockManager() {
            foreach (CodeBlock cb in GetComponentsInChildren<CodeBlock>()) {
                cb.transform.SnapToCodeBlockManager();
            }
        }

        public void UnsnapAllBlockFromBlockManager() {
            StartCodeBlock.instance.SetArgumentBlockAt(null, 0); // unsnap
            foreach (CodeBlock cb in CodeBlockManager.instance.GetAllCodeBlocks()) {
                if (cb != StartCodeBlock.instance) {
                    cb.transform.SnapToParent(transform);
                }
            }
        }

        private void OnEnable() {
            SnapAllBlocksToBlockManager();
            AddAllVariables();
        }

        private void AddAllVariables() {
            for (int i = 0; i < varNames.Length; ++i) {
                MemoryManager.instance.AddNewVariableCodeBlock(varNames[i],
                    new IntDataType(null, values[i]));
            }
        }

        internal void CleanUp() {
            UnsnapAllBlockFromBlockManager();
            MemoryManager.instance.RemoveAllVariables();
        }
    }
}
