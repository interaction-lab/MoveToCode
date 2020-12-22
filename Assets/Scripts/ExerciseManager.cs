using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {
        public static string curExcersieCol = "CurExercise", exerciseSubmissionResultCol = "ExerciseSubmissionResult";
        string targetDirectory = @"Assets/Resources/ExerciseJsons";
        string[] fileEntries;

        Exercise curExercise;
        Exercise FreePlay;
        public int curExercisePos = 0;
        bool lastExerciseCompleted = false;

        private void Awake() {
#if WINDOWS_UWP
            curExercisePos = 0;
#endif
            SetUpFreePlayExercise();
            var tmp = Resources.LoadAll<TextAsset>(ResourcePathConstants.ExerciseJsonFolder);
            fileEntries = new string[tmp.Length];
            int i = 0;
            foreach (var ta in tmp) {
                fileEntries[i] = ta.ToString();
                ++i;
            }
            //fileEntries = Directory.GetFiles(targetDirectory).Where(s => s.EndsWith(".json")).ToArray();
            if (curExercisePos < fileEntries.Length) {
                SetUpCurExercise(curExercisePos);
            }
            else {
                InitiateFreePlay();
            }
            ToggleCurrentExercise(true);
            LoggingManager.instance.AddLogColumn(curExcersieCol, curExercisePos.ToString());
            LoggingManager.instance.AddLogColumn(exerciseSubmissionResultCol, "");
        }

        public int GetCurExcercisePos() {
            return curExercisePos;
        }

        private void SetUpCurExercise(int exerciseNum) {
            string json = fileEntries[exerciseNum];
            //string json = File.ReadAllText(fileEntries[exerciseNum]);
            GameObject exercise = InstantiateExercise(json);
            //Add scaffoldDialogue
            exercise.AddComponent<ExerciseScaffolding>().SetScaffoldDialogue(
                curExercise.GetExerciseInternals().GetscaffoldDialogue());
            //TODO: add ExerciseInformationSeekingActions
        }

        public Exercise GetCurExercise() {
            return curExercise;
        }

        public GameObject InstantiateExercise(string jsonString) {
            GameObject exGO = Instantiate(
                Resources.Load<GameObject>(ResourcePathConstants.ExercisePrefab), transform.parent) as GameObject;
            exGO.transform.SnapToParent(transform);
            exGO.GetComponent<Exercise>().SetupExercise(jsonString);
            curExercise = exGO.GetComponent<Exercise>();
            exGO.GetComponent<Exercise>().SetUpKuriInExercise();
            return exGO;
        }

        public void AlertCodeFinished() {
            if (curExercise != null && curExercise != FreePlay) { // This if is to guard against initializing interpreter
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
                SetUpCurExercise(curExercisePos);
                ToggleCurrentExercise(true);
            }
        }

        private void InitiateFreePlay() {
            Debug.Log("Free play would be initiated");
            curExercise = FreePlay;
            CodeBlockMenuManager.instance.TurnMenuOn();
            ToggleCurrentExercise(true);
        }

        private void ToggleCurrentExercise(bool desiredActiveState) {
            curExercise.gameObject.SetActive(desiredActiveState);
        }

        private void SetUpFreePlayExercise() {
            FreePlay = GetComponentsInChildren<Exercise>(true)[0];
            FreePlay.gameObject.SetActive(false);
        }
    }
}