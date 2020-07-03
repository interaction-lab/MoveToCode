﻿using UnityEngine;
using UnityEngine.Assertions;
using Newtonsoft.Json;

namespace MoveToCode {
    //[RequireComponent(typeof(ExerciseInformationSeekingActions))]
    //[RequireComponent(typeof(ExerciseScaffolding))]
    public class Exercise : MonoBehaviour {

        public ExerciseInternals myExerciseInternals;

        public Exercise() { }

        public void SetExerciseInternals(string address) {
            //var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            //myExerciseInternals = JsonConvert.DeserializeObject<ExerciseInternals>(address, settings);
            //Debug.Log(myExerciseInternals.consoleStringGoal);

            /*
            myExerciseInternals = JsonUtility.FromJson<ExerciseInternals>(address);
            Debug.Log(myExerciseInternals.exerciseCodeBlocks[0].GetType().Name);
            Debug.Log(myExerciseInternals.kuriGoalString);
            */

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            myExerciseInternals = JsonConvert.DeserializeObject<ExerciseInternals>(address, settings);
            Debug.Log(myExerciseInternals.kuriGoalString);
            Debug.Log(myExerciseInternals.GetExerciseBlocks()[0].codeBlockID);

            InstantiateCodeBlocksAsExerciseChildren();
            Assert.IsTrue(myExerciseInternals.varNames.Length == myExerciseInternals.GetInitialVariableValues().Length && myExerciseInternals.GetInitialVariableValues().Length == myExerciseInternals.GetFinalVariableGoalValues().Length);
            SnapAllBlocksToBlockManager();
            AddAllVariables();
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
            KuriManager.instance.SayExerciseGoal();
        }

        public ExerciseInternals GetExerciseInternals() {
            return myExerciseInternals;
        }

        public void InstantiateCodeBlocksAsExerciseChildren() {
            for (int i = 0; i < myExerciseInternals.exerciseBlocks.Length; i++) {
                string id = myExerciseInternals.GetExerciseBlocks()[i].codeBlockID;
                Debug.Log(ExerciseManager.codeBlockDictionary[id]);
                GameObject codeBlockGameObject = Instantiate(
                    ExerciseManager.codeBlockDictionary[id], transform) as GameObject;

                /*if (myExerciseInternals.exerciseBlocks[i].GetType() == typeof(IDataType)) {
                    (codeBlockGameObject.GetComponent(myExerciseInternals.exerciseBlocks[i].GetType()) as CodeBlock).ChangeMyBlockInternalArg(
                        (myExerciseInternals.exerciseBlocks[i].GetMyInternalIArgument() as IDataType), myExerciseInternals.exerciseCodeBlocks[i].output);
                }*/
                //codeBlockGameObject.transform.SnapToParent(transform);
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
            return myExerciseInternals.kuriGoalString;
        }

    }
}
