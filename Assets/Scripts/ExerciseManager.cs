using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {
        public static string curExcersieCol = "CurExercise", exerciseSubmissionResultCol = "ExerciseSubmissionResult";
        public static Dictionary<string, GameObject> codeBlockDictionary;
        string[] fileEntries;
        string targetDirectory;

        Exercise curExercise;
        List<Exercise> exerciseList;
        public int curExercisePos = 0;
        bool lastExerciseCompleted = false;

        private void Awake() {
#if WINDOWS_UWP
            curExercisePos = 0;
#endif
            SetupCodeBlockDictionary();

            targetDirectory = @"Assets/Resources/ExerciseJsons";
            fileEntries = Directory.GetFiles(targetDirectory);
            
            string json = File.ReadAllText(fileEntries[0]);

            //Instantiate exercises
            GameObject currExercise = Instantiate(
                    Resources.Load<GameObject>(ResourcePathConstants.ExercisePrefab), transform.parent) as GameObject;
            currExercise.transform.SnapToParent(transform);
            currExercise.GetComponent<Exercise>().SetupExercise(json);
            
            //exerciseList = new List<Exercise>();
            //exerciseList.Add(currExercise.GetComponent<Exercise>());

            //SetUpExerciseList();
            curExercise = exerciseList[curExercisePos];
            ToggleCurrentExercise(true);
            LoggingManager.instance.AddLogColumn(curExcersieCol, curExercisePos.ToString());
            LoggingManager.instance.AddLogColumn(exerciseSubmissionResultCol, "");
        }

        private void SetUpExercise(int exerciseNum) {

        }

        private void SetUpExerciseList() {
            exerciseList = new List<Exercise>();
            foreach (Exercise ex in GetComponentsInChildren<Exercise>(true)) {
                exerciseList.Add(ex);
                ex.gameObject.SetActive(false);
                //string json = JsonUtility.ToJson(ex);
                //JsonUtility.FromJson<Exercise>(@"sdf");
                //Debug.Log(json);
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
                //curExercise = exerciseList[curExercisePos];
                SetUpExercise(curExercisePos);
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
            codeBlockDictionary = new Dictionary<string, GameObject>();
            codeBlockDictionary.Add("Print", Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab));
            codeBlockDictionary.Add("Conditional", Resources.Load<GameObject>(ResourcePathConstants.ConditionBlockPrefab));
            codeBlockDictionary.Add("If", Resources.Load<GameObject>(ResourcePathConstants.IfCodeBlockPrefab));
            codeBlockDictionary.Add("Int", Resources.Load<GameObject>(ResourcePathConstants.IntCodeBlockPrefab));
            codeBlockDictionary.Add("Math", Resources.Load<GameObject>(ResourcePathConstants.MathCodeBlockPrefab));
            codeBlockDictionary.Add("SetVar", Resources.Load<GameObject>(ResourcePathConstants.SetVariableCodeBlockPrefab));
            codeBlockDictionary.Add("String", Resources.Load<GameObject>(ResourcePathConstants.StringCodeBlockPrefab));
            codeBlockDictionary.Add("While", Resources.Load<GameObject>(ResourcePathConstants.WhileCodeBlockPrefab));
            codeBlockDictionary.Add("Char", Resources.Load<GameObject>(ResourcePathConstants.CharCodeBlockPrefab));
            codeBlockDictionary.Add("Array", Resources.Load<GameObject>(ResourcePathConstants.ArrayCodeBlockPrefab));
            codeBlockDictionary.Add("ArrayIndex", Resources.Load<GameObject>(ResourcePathConstants.ArrayIndexCodeBlockPrefab));
        }
    }
}
