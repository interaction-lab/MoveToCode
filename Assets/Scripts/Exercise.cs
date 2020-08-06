using UnityEngine;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class Exercise : MonoBehaviour {

        List<GameObject> codeBlockGameObjectList;
        int numBlocksInSpawnCol = 4; //number of blocks in a single column when blocks are instantiated 

        public ExerciseInternals myExerciseInternals;

        public Exercise() { }

        public void SetupExercise(string json) {
            //read in json
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            SetExerciseInternals(JsonConvert.DeserializeObject<ExerciseInternals>(json, settings));
            
            //create code blocks
            InstantiateCodeBlocks();
            RepositionCodeBlocks();
            Assert.IsTrue(myExerciseInternals.GetVarNames().Length == myExerciseInternals.GetInitialVariableValues().Length && myExerciseInternals.GetInitialVariableValues().Length == myExerciseInternals.GetFinalVariableGoalValues().Length);
            SnapAllBlocksToBlockManager();
            AddAllVariables();
        }

        public void SetUpKuriInExercise() {
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
            KuriManager.instance.SayExerciseGoal();
        }

        public void SetExerciseInternals(object exIn) {
            myExerciseInternals = exIn as ExerciseInternals;
        }

        public ExerciseInternals GetExerciseInternals() {
            return myExerciseInternals;
        }

        public void InstantiateCodeBlocks() {
            codeBlockGameObjectList = new List<GameObject>();
            for (int i = 0; i < myExerciseInternals.GetExerciseBlocks().Length; i++) {

                string id = myExerciseInternals.GetExerciseBlocks()[i].codeBlockID;
                object value = myExerciseInternals.GetExerciseBlocks()[i].value;

                //set math operation
                if (ResourcePathConstants.codeBlockDictionary[id] ==
                    Resources.Load<GameObject>(ResourcePathConstants.MathCodeBlockPrefab)) {
                    SetMathOp(ResourcePathConstants.codeBlockDictionary[id], value);
                //set conditional operation
                } else if (ResourcePathConstants.codeBlockDictionary[id] == 
                    Resources.Load<GameObject>(ResourcePathConstants.ConditionBlockPrefab)) {
                    SetConditionalOp(ResourcePathConstants.codeBlockDictionary[id], value);
                //set data value
                } else if (value != null) {
                    SetDataValue(ResourcePathConstants.codeBlockDictionary[id], value);
                }

                //instantiate block
                GameObject codeBlockGameObject = Instantiate(ResourcePathConstants.codeBlockDictionary[id], transform) as GameObject;
                codeBlockGameObjectList.Add(codeBlockGameObject);
            }
        }

        public bool IsExerciseCorrect() {
            bool result = true;
            for (int i = 0; i < myExerciseInternals.GetVarNames().Length; ++i) {
                result &= (MemoryManager.instance.GetVariableValue(myExerciseInternals.GetVarNames()[i]).GetValue() as int?) == (myExerciseInternals.GetFinalVariableGoalValues()[i] as int?) ||
                    (MemoryManager.instance.GetVariableValue(myExerciseInternals.GetVarNames()[i]).GetValue() as Array) == (myExerciseInternals.GetFinalVariableGoalValues()[i] as Array);
            }
            result &= ConsoleManager.instance.GetCleanedMainText() == myExerciseInternals.GetConsoleStringGoal();
            return result;
        }

        private void RepositionCodeBlocks() {
            //coordinates of top right block in codeblock grid spawn space
            float topRightX = ConsoleManager.instance.transform.position.x - 1.25f;
            float topRightY = ConsoleManager.instance.transform.position.y - 1;
            float topRightZ = ConsoleManager.instance.transform.position.z - 1;

            float prevX = topRightX, prevY = topRightY;
            for (int i = 0; i < codeBlockGameObjectList.Count; i++) {
                if (i == 0) {
                    codeBlockGameObjectList[i].transform.localPosition = new Vector3(prevX, prevY, topRightZ);
                } else if (i % numBlocksInSpawnCol == 0) {
                    prevY = topRightY;
                    codeBlockGameObjectList[i].transform.localPosition = new Vector3(prevX -= 0.5f, prevY, topRightZ);
                } else {
                    codeBlockGameObjectList[i].transform.localPosition = new Vector3(prevX, prevY -= 0.25f, topRightZ);
                }
            }
        }

        private void SetMathOp(GameObject prefab, object valIn) {
            prefab.GetComponent<MathOperationCodeBlock>().SetOperation(
                (MathOperationCodeBlock.OPERATION)Enum.Parse(typeof(MathOperationCodeBlock.OPERATION), valIn as string));
        }

        private void SetConditionalOp(GameObject prefab, object valIn) {
            prefab.GetComponent<ConditionalCodeBlock>().SetOperation(
                (ConditionalCodeBlock.OPERATION)Enum.Parse(typeof(ConditionalCodeBlock.OPERATION), valIn as string));
        }

        private void SetDataValue(GameObject prefab, object valIn) {
            prefab.GetComponent<DataCodeBlock>().SetOutput(valIn);
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
            myExerciseInternals = new ExerciseInternals();
        }

        private void AddAllVariables() {
            for (int i = 0; i < myExerciseInternals.GetVarNames().Length; ++i) {
                MemoryManager.instance.AddNewVariableCodeBlock(myExerciseInternals.GetVarNames()[i],
                    new IntDataType(null, myExerciseInternals.GetInitialVariableValues()[i]));
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
