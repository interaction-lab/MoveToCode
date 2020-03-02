using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    [RequireComponent(typeof(ExerciseInformationSeekingActions))]
    [RequireComponent(typeof(ExerciseScaffolding))]
    public class Exercise : MonoBehaviour {
        public string consoleStringGoal;
        public string kuriGoalString;
        public string[] varNames;
        public int[] initialVariableValues;
        public int[] finalVariableGoalValues;

        public bool IsExerciseCorrect() {
            bool result = true;
            for (int i = 0; i < varNames.Length; ++i) {
                result &= ((int)MemoryManager.instance.GetVariableValue(varNames[i]).GetValue()) == finalVariableGoalValues[i];
            }
            result &= ConsoleManager.instance.GetCleanedMainText() == consoleStringGoal;
            return result;
        }

        private void SnapAllBlocksToBlockManager() {
            foreach (CodeBlock cb in GetComponentsInChildren<CodeBlock>()) {
                Vector3 diff = (cb.transform.position - StartCodeBlock.instance.GetStartPos());
                cb.transform.SnapToCodeBlockManager();
                cb.transform.position = StartCodeBlock.instance.transform.position + diff;
            }
        }

        public void UnsnapAllBlockFromBlockManager() {
            StartCodeBlock.instance.SetArgumentBlockAt(null, 0, false); // unsnap
            foreach (CodeBlock cb in CodeBlockManager.instance.GetAllCodeBlocks()) {
                if (cb != StartCodeBlock.instance) {
                    cb.transform.SnapToParent(transform);
                }
            }
        }

        protected virtual void OnEnable() {
            Assert.IsTrue(varNames.Length == initialVariableValues.Length && initialVariableValues.Length == finalVariableGoalValues.Length);
            SnapAllBlocksToBlockManager();
            AddAllVariables();
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
            KuriManager.instance.SayExerciseGoal();
            Debug.Log("run2");
        }

        private void AddAllVariables() {
            for (int i = 0; i < varNames.Length; ++i) {
                MemoryManager.instance.AddNewVariableCodeBlock(varNames[i],
                    new IntDataType(null, initialVariableValues[i]));
            }
        }

        internal void CleanUp() {
            UnsnapAllBlockFromBlockManager();
            MemoryManager.instance.RemoveAllVariables();
        }

        public string GetGoalString() {
            return kuriGoalString;
        }
    }
}
