using RosSharp.RosBridgeClient;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriManager : Singleton<KuriManager> {
        [Range(0.0f, 1.0f)]
        public float robotKC;

        static string rISACol = "robotISA";

        KuriEmoteStringPublisher kuriEmoteStringPublisher;
        PoseStampedPublisher poseStampPublisher;
        float timeSinceLastAction, timeWindow;

        Transform kuriGoalPoseTransform, kuriCurPoseTransform;

        private void Awake() {
            timeWindow = HumanStateManager.instance.timeWindow;
            timeSinceLastAction = 0;
            kuriEmoteStringPublisher = FindObjectOfType<KuriEmoteStringPublisher>();
            poseStampPublisher = FindObjectOfType<PoseStampedPublisher>();
            kuriGoalPoseTransform = transform.GetChild(0);
            kuriCurPoseTransform = transform.GetChild(1);
            LoggingManager.instance.AddLogColumn(rISACol, "");
        }

        private void Update() {
            Tick();
            timeSinceLastAction += Time.deltaTime;
        }

        public Transform GetKuriGoalPoseTransform() {
            if (kuriGoalPoseTransform == null) {
                kuriGoalPoseTransform = transform.GetChild(0);
            }
            return kuriGoalPoseTransform;
        }

        public Transform GetKuriCurPoseTransform() {
            if (kuriCurPoseTransform == null) {
                kuriCurPoseTransform = transform.GetChild(1);
            }
            return kuriCurPoseTransform;
        }

        public void AlertActionMade() {
            timeSinceLastAction = 0;
        }

        private void Tick() {
            if (timeSinceLastAction < timeWindow) {
                return;
            }
            float kctS = HumanStateManager.instance.GetKCt();
            if (kctS < robotKC) {
                TakeISAAction();
                TakeMovementAction();
            }
            else {
                kuriEmoteStringPublisher?.PubRandomPositive();
            }
            AlertActionMade();
        }

        private void TakeISAAction() {
            string actionString = ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseInformationSeekingActions>().DoISAAction();
            LoggingManager.instance.UpdateLogColumn(rISACol, actionString);
        }

        private void TakeMovementAction() {
            Debug.Log("Movement action taken");
            kuriGoalPoseTransform.position = ExerciseInformationSeekingActions.goOfFocus.transform.position;
            kuriGoalPoseTransform.rotation = Quaternion.LookRotation(ExerciseInformationSeekingActions.goOfFocus.transform.forward);
            poseStampPublisher?.PublishPosition(kuriGoalPoseTransform);
        }

        public void SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa) {
            poseStampPublisher?.PubTurnTowardUser();
            kuriEmoteStringPublisher?.PubRandomPositive();
            KuriTextManager.instance.SayRandomPositiveAffect(toa);
            AlertActionMade();
        }

        public void SayExerciseGoal() {
            poseStampPublisher?.PubTurnTowardUser();
            KuriTextManager.instance.Addline(string.Join("",
                "Goal: ",
                ExerciseManager.instance.GetCurExercise().GetGoalString()),
                KuriTextManager.PRIORITY.high);
            AlertActionMade();
        }

        public void DoScaffoldingDialogue() {
            poseStampPublisher?.PubTurnTowardUser();
            ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseScaffolding>().SayNextScaffold();
            AlertActionMade();
        }
    }
}
