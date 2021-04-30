using RosSharp.RosBridgeClient;
using System.Collections;
using UnityEngine;
using static MoveToCode.KuriController;

namespace MoveToCode {
    public class KuriManager : Singleton<KuriManager> {
        [Range(-3.0f, 3.0f)]
        public float robotKC;
        [HideInInspector]
        public bool usePhysicalKuri = true;
        static string rISACol = "robotISA",
            robotKCLevel = "robotKCLevel",
            kuriPhysicalEmoteActionCol = "kuriPhysicalAction",
            kuriMovementActionCol = "kuriMovementAction";


        PoseStampedPublisher poseStampPublisher;
        float timeSinceLastAction, timeWindow;

        public Transform KuriGoalPoseTransform;
        KuriController kuriController;

        bool inStartUp;

        private void Awake() {
            OptionSelectionManager.instance.Init();
            if (usePhysicalKuri) {
                kuriController = FindObjectOfType<PhysicalKuriController>().GetComponent<KuriController>();
            }
            else {
                kuriController = FindObjectOfType<VirtualKuriController>().GetComponent<KuriController>();
            }
            timeWindow = HumanStateManager.instance.timeWindow;
            timeSinceLastAction = 0;
            poseStampPublisher = FindObjectOfType<PoseStampedPublisher>();
            LoggingManager.instance.AddLogColumn(rISACol, "");
            LoggingManager.instance.AddLogColumn(robotKCLevel, "");
            LoggingManager.instance.AddLogColumn(kuriPhysicalEmoteActionCol, "");
            LoggingManager.instance.AddLogColumn(kuriMovementActionCol, "");
            KuriGoalPoseTransform = transform.GetChild(0); // TODO: this is awful
        }

        private void Start() {
            StartCoroutine(StartRoutine());
        }

        public void SetKC(float kcRIn) {
            robotKC = kcRIn;
            LoggingManager.instance.UpdateLogColumn(robotKCLevel, robotKC.ToString("F3"));
        }

        IEnumerator StartRoutine() {
            inStartUp = true;
            yield return null;
            yield return new WaitForSeconds(3);
            kuriController.DoAction(EMOTIONS.close_eyes);
            yield return new WaitForSeconds(InteractionManager.instance.MinToSeconds(InteractionManager.instance.warmUpTimeMinutes) - 3f);
            kuriController.DoAction(KuriEmoteStringPublisher.EMOTIONS.happy);
            inStartUp = false;
        }



        private void Update() {
            if (inStartUp) {
                return;
            }
            Tick();
            timeSinceLastAction += Time.deltaTime;
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
                //TakeMovementAction();
            }
            else {
                kuriController.DoRandomPositiveAction();
            }
            AlertActionMade();
        }

        private void TakeISAAction() {
            string actionMade = kuriController.TakeISAAction();
            LoggingManager.instance.UpdateLogColumn(rISACol, actionMade);
        }

        private void TakeMovementAction() {
            string actionMade = kuriController.TakeMovementAction();
            LoggingManager.instance.UpdateLogColumn(kuriMovementActionCol, actionMade);
            KuriGoalPoseTransform.position = ExerciseInformationSeekingActions.goOfFocus.transform.position;
            KuriGoalPoseTransform.rotation = Quaternion.LookRotation(ExerciseInformationSeekingActions.goOfFocus.transform.forward);
            poseStampPublisher?.PublishPosition(KuriGoalPoseTransform);
        }

        public void SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa) {
            //poseStampPublisher?.PubTurnTowardUser();
            //kuriEmoteStringPublisher?.PubRandomPositive();
            LoggingManager.instance.UpdateLogColumn(kuriPhysicalEmoteActionCol,
                kuriController.DoPositiveAffect(toa));

            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.low);
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
