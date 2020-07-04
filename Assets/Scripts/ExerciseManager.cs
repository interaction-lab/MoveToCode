using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {
        public static string curExcersieCol = "CurExercise", exerciseSubmissionResultCol = "ExerciseSubmissionResult";
        public static Dictionary<string, GameObject> codeBlockDictionary;
        string targetDirectory = @"Assets/Resources/ExerciseJsons";
        string[] fileEntries;

        Exercise curExercise;
        List<Exercise> exerciseList;
        public int curExercisePos = 0;
        bool lastExerciseCompleted = false;

        private void Awake() {
#if WINDOWS_UWP
            curExercisePos = 0;
#endif
            SetupCodeBlockDictionary();
            SetUpExerciseList();
            fileEntries = Directory.GetFiles(targetDirectory).Where(s => s.EndsWith(".json")).ToArray();
            SetUpCurExercise(curExercisePos);
            ToggleCurrentExercise(true);
            LoggingManager.instance.AddLogColumn(curExcersieCol, curExercisePos.ToString());
            LoggingManager.instance.AddLogColumn(exerciseSubmissionResultCol, "");
        }

        private void SetUpCurExercise(int exerciseNum) {
            string json = File.ReadAllText(fileEntries[exerciseNum]);

            //Instantiate exercise
            GameObject exGO = Instantiate(
                    Resources.Load<GameObject>(ResourcePathConstants.ExercisePrefab), transform.parent) as GameObject;
            exGO.transform.SnapToParent(transform);
            exGO.GetComponent<Exercise>().SetupExercise(json);
            curExercise = exGO.GetComponent<Exercise>();
            exGO.GetComponent<Exercise>().SetUpKuriInExercise();
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
            if (curExercisePos == fileEntries.Length) {
                InitiateFreePlay();
            }
            else {
                LoggingManager.instance.UpdateLogColumn(curExcersieCol, curExercisePos.ToString());
                Debug.Log(curExercisePos);
                SetUpCurExercise(curExercisePos);
                ToggleCurrentExercise(true);
            }
        }

        private void InitiateFreePlay() {
            Debug.Log("Free play woould be initiated");
            curExercise = exerciseList[0];
            ToggleCurrentExercise(true);
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
            codeBlockDictionary.Add("Variable", Resources.Load<GameObject>(ResourcePathConstants.VariableCodeBlockPrefab));
        }

        private void SetUpExerciseList() {
            exerciseList = new List<Exercise>();
            foreach (Exercise ex in GetComponentsInChildren<Exercise>(true)) {
                exerciseList.Add(ex);
                ex.gameObject.SetActive(false);
            }
        }
    }
}
