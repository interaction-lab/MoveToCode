using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    //[RequireComponent(typeof(ExerciseInformationSeekingActions))]
    //[RequireComponent(typeof(ExerciseScaffolding))]
    public class ExerciseInternals : MonoBehaviour {
        public string consoleStringGoal { get; set; }
        public string kuriGoalString { get; set; }
        public string[] varNames { get; set; }
        public int[] initialVariableValues { get; set; }
        public int[] finalVariableGoalValues { get; set; }

        public CodeBlock[] exerciseCodeBlocks;

        //instantiate all of its parts (codeblocks needed for the exercise)
        public void InstantiateCodeBlocksAsExerciseChildren() {
            for (int i = 0; i < exerciseCodeBlocks.Length; i++) {
                GameObject codeBlockGameObject = Instantiate(ExerciseManager.codeBlockDictionary[exerciseCodeBlocks[i].GetType()]) as GameObject;

                if (exerciseCodeBlocks[i].GetType() == typeof(IDataType)) {
                    (codeBlockGameObject.GetComponent(exerciseCodeBlocks[i].GetType()) as CodeBlock).ChangeMyBlockInternalArg(
                        (exerciseCodeBlocks[i].GetMyInternalIArgument() as IDataType), exerciseCodeBlocks[i].output);
                }
                codeBlockGameObject.transform.SetParent(transform);
            }
        }

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

        private void UnsnapAllBlockFromBlockManager() {
            StartCodeBlock.instance.SetArgumentBlockAt(null, 0, false); // unsnap
            foreach (CodeBlock cb in CodeBlockManager.instance.GetAllCodeBlocks()) {
                if (cb != StartCodeBlock.instance) {
                    cb.transform.SnapToParent(transform);
                }
            }
        }

        protected virtual void OnEnable() {
            InstantiateCodeBlocksAsExerciseChildren();
            Assert.IsTrue(varNames.Length == initialVariableValues.Length && initialVariableValues.Length == finalVariableGoalValues.Length);
            SnapAllBlocksToBlockManager();
            AddAllVariables();
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
            KuriManager.instance.SayExerciseGoal();
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
