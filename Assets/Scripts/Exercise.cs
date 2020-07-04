using UnityEngine;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using System;

namespace MoveToCode {
    //[RequireComponent(typeof(ExerciseInformationSeekingActions))]
    //[RequireComponent(typeof(ExerciseScaffolding))]
    public class Exercise : MonoBehaviour {

        public ExerciseInternals myExerciseInternals;

        public Exercise() { }

        public void SetupExercise(string json) {
            //read in json
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            SetExerciseInternals(JsonConvert.DeserializeObject<ExerciseInternals>(json, settings));
            
            //create code blocks
            InstantiateCodeBlocks();
            Assert.IsTrue(myExerciseInternals.GetVarNames().Length == myExerciseInternals.GetInitialVariableValues().Length && myExerciseInternals.GetInitialVariableValues().Length == myExerciseInternals.GetFinalVariableGoalValues().Length);
            SnapAllBlocksToBlockManager();
            AddAllVariables();
        }

        public void SetUpKuriInExercise() {
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
            KuriManager.instance.SayExerciseGoal();
        }

        public void SetExerciseInternals(object exIn) {
            myExerciseInternals = new ExerciseInternals();
            myExerciseInternals = exIn as ExerciseInternals;
        }

        public ExerciseInternals GetExerciseInternals() {
            return myExerciseInternals;
        }

        public void InstantiateCodeBlocks() {
            for (int i = 0; i < myExerciseInternals.GetExerciseBlocks().Length; i++) {

                string id = myExerciseInternals.GetExerciseBlocks()[i].codeBlockID;
                object value = myExerciseInternals.GetExerciseBlocks()[i].value;

                //set math operation
                if (ExerciseManager.codeBlockDictionary[id] ==
                    Resources.Load<GameObject>(ResourcePathConstants.MathCodeBlockPrefab)) {
                    SetMathOp(value);
                //set conditional operation
                } else if (ExerciseManager.codeBlockDictionary[id] == 
                    Resources.Load<GameObject>(ResourcePathConstants.ConditionBlockPrefab)) {
                    SetConditionalOp(value);
                }

                //instantiate block
                GameObject codeBlockGameObject = Instantiate(
                    ExerciseManager.codeBlockDictionary[id], transform) as GameObject;

                //set data type value
                (codeBlockGameObject.GetComponent<CodeBlock>().GetMyInternalIArgument() as IDataType)?.SetValue(value);

                codeBlockGameObject.transform.localPosition = new Vector3(-1.0f, -0.6048422f, 0.2010774f); //TODO: actually place these blocks somewhere decent
            }
        }

        public bool IsExerciseCorrect() {
            bool result = true;
            return result;
        }

        private void SetMathOp(object valIn) {
            Resources.Load<GameObject>(ResourcePathConstants.MathCodeBlockPrefab).GetComponent<MathOperationCodeBlock>().
                        SetOperation((MathOperationCodeBlock.OPERATION)Enum.Parse(typeof(MathOperationCodeBlock.OPERATION), valIn as string));
        }

        private void SetConditionalOp(object valIn) {
            Resources.Load<GameObject>(ResourcePathConstants.ConditionBlockPrefab).GetComponent<ConditionalCodeBlock>().
                        SetOperation((ConditionalCodeBlock.OPERATION)Enum.Parse(typeof(ConditionalCodeBlock.OPERATION), valIn as string));
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
            return myExerciseInternals.GetKuriGoalString();
        }

    }
}
