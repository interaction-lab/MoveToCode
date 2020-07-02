using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {
        public static string curExcersieCol = "CurExercise", exerciseSubmissionResultCol = "ExerciseSubmissionResult";
        //public static Dictionary<string, GameObject> codeBlockDictionary;
        public static Dictionary<Type, GameObject> codeBlockDictionary;

        Exercise curExercise;
        List<Exercise> exerciseList;
        public int curExercisePos = 0;
        bool lastExerciseCompleted = false;

        private void Awake() {
#if WINDOWS_UWP
            curExercisePos = 0;
#endif
            SetupCodeBlockDictionary();
            //Instantiate elements

            SetUpExerciseList();
            curExercise = exerciseList[curExercisePos];
            ToggleCurrentExercise(true);
            LoggingManager.instance.AddLogColumn(curExcersieCol, curExercisePos.ToString());
            LoggingManager.instance.AddLogColumn(exerciseSubmissionResultCol, "");
        }

        private void SetUpExerciseList() {
            exerciseList = new List<Exercise>();
            foreach (Exercise ex in GetComponentsInChildren<Exercise>(true)) {
                exerciseList.Add(ex);
                ex.gameObject.SetActive(false);
                string json = JsonUtility.ToJson(ex);
                Debug.Log(json);
            }
        }

        public Exercise GetCurExercise() {
            return curExercise;
        }

        public void AlertCodeFinished() {
            if (curExercise != null) { // This if is to guard against initializing interpreter
                if (curExercise.IsExerciseCorrect()) {
                    KuriManager.instance.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Congratulation);
                    LoggingManager.instance.UpdateLogColumn(exerciseSubmissionResultCol, "Correct");
                    lastExerciseCompleted = true;
                }
                else {
                    KuriManager.instance.DoScaffoldingDialogue();
                    LoggingManager.instance.UpdateLogColumn(exerciseSubmissionResultCol, "InCorrect");
                }
            }
        }

        public void AlertCodeReset() {
            if (lastExerciseCompleted) {
                CycleNewExercise();
            }
        }

        private void CycleNewExercise() {
            lastExerciseCompleted = false;
            curExercise.CleanUp();
            ToggleCurrentExercise(false);
            curExercisePos += 1;
            if (curExercisePos == exerciseList.Count) {
                InitiateFreePlay();
            }
            else {
                LoggingManager.instance.UpdateLogColumn(curExcersieCol, curExercisePos.ToString());
                curExercise = exerciseList[curExercisePos];
                ToggleCurrentExercise(true);
            }
        }

        private void InitiateFreePlay() {
            Debug.Log("Free play woould be initiated");
        }

        private void ToggleCurrentExercise(bool desiredActiveState) {
            curExercise.gameObject.SetActive(desiredActiveState);
        }

        private void SetupCodeBlockDictionary() {
            /*
            codeBlockDictionary.Add("printBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("conditionBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("ifBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("intBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("mathBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("setVarBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("stringBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("whileBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("charBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("arrayBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("arrayIndexBlock", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            */
            codeBlockDictionary.Add(typeof(PrintCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(ConditionalCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.ConditionBlockPrefab));
            codeBlockDictionary.Add(typeof(IfCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.IfCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(IntCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.IntCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(MathOperationCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.MathCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(SetVariableCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.SetVariableCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(StringCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.StringCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(WhileCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.WhileCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(CharCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.CharCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(ArrayCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.ArrayCodeBlockPrefab));
            codeBlockDictionary.Add(typeof(ArrayIndexCodeBlock), Resources.Load<GameObject>(ResourcePathConstants.ArrayIndexCodeBlockPrefab));
        }
    }
}
