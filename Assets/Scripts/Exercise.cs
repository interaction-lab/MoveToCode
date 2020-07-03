using UnityEngine;
using UnityEngine.Assertions;
using Newtonsoft.Json;

namespace MoveToCode {
    //[RequireComponent(typeof(ExerciseInformationSeekingActions))]
    //[RequireComponent(typeof(ExerciseScaffolding))]
    public class Exercise : MonoBehaviour {

        public ExerciseInternals myExerciseInternals;

        public Exercise() { }

        public void SetupExercise(string address) {
            //read in json
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            myExerciseInternals = JsonConvert.DeserializeObject<ExerciseInternals>(address, settings);

            InstantiateCodeBlocksAsExerciseChildren();
            Assert.IsTrue(myExerciseInternals.varNames.Length == myExerciseInternals.GetInitialVariableValues().Length && myExerciseInternals.GetInitialVariableValues().Length == myExerciseInternals.GetFinalVariableGoalValues().Length);
            SnapAllBlocksToBlockManager();
            AddAllVariables();
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
            KuriManager.instance.SayExerciseGoal();
        }

        public void SetExerciseInternals(object exIn) {
            myExerciseInternals = exIn as ExerciseInternals;


        }

        public ExerciseInternals GetExerciseInternals() {
            return myExerciseInternals;
        }

        public void InstantiateCodeBlocksAsExerciseChildren() {
            for (int i = 0; i < myExerciseInternals.exerciseBlocks.Length; i++) {
                string id = myExerciseInternals.GetExerciseBlocks()[i].codeBlockID;
                object value = myExerciseInternals.GetExerciseBlocks()[i].value;
                Debug.Log(ExerciseManager.codeBlockDictionary[id]);
                GameObject codeBlockGameObject = Instantiate(
                    ExerciseManager.codeBlockDictionary[id], transform) as GameObject;
                (codeBlockGameObject.GetComponent<CodeBlock>().GetMyInternalIArgument() as IDataType)?.SetValue(value);
                codeBlockGameObject.transform.localPosition = new Vector3(-1.0f, -0.6048422f, 0.5010774f); //TODO: actually place these blocks somewhere decent
            }
        }

        public bool IsExerciseCorrect() {
            bool result = true;
            for (int i = 0; i < myExerciseInternals.varNames.Length; ++i) {
                result &= ((int)MemoryManager.instance.GetVariableValue(myExerciseInternals.varNames[i]).GetValue()) == myExerciseInternals.finalVariableGoalValues[i];
            }
            result &= ConsoleManager.instance.GetCleanedMainText() == myExerciseInternals.consoleStringGoal;
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

        protected virtual void OnEnable() { }

        private void AddAllVariables() {
            for (int i = 0; i < myExerciseInternals.varNames.Length; ++i) {
                MemoryManager.instance.AddNewVariableCodeBlock(myExerciseInternals.varNames[i],
                    new IntDataType(null, myExerciseInternals.initialVariableValues[i]));
            }
        }

        internal void CleanUp() {
            UnsnapAllBlockFromBlockManager();
            MemoryManager.instance.RemoveAllVariables();
        }

        public string GetGoalString() {
            Debug.Log(myExerciseInternals.GetKuriGoalString());
            return myExerciseInternals.GetKuriGoalString();
        }

    }
}
