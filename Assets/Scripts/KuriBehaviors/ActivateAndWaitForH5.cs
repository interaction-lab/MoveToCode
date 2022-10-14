using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class ActivateAndWaitForH5 : ActionNode {
        #region members
        float maxTimeToWait = 10f, startTime;
        KuriArms kuriArms;
        bool handHit = false, initialized = false;
        PulseMeshRend RPulseMeshRend = null;
        VirtualKuriAudio audioManager;
        public VirtualKuriAudio AudioManager {
            get {
                if (audioManager == null) {
                    audioManager = TutorKuriManager.instance.KuriAudio;
                }
                return audioManager;
            }
        }
        MazeManager mm;
        MazeManager MazeManagerInstance {
            get {
                if (mm == null) {
                    mm = MazeManager.instance;
                }
                return mm;
            }
        }
        ScreenMenuManager smmm;
        ScreenMenuManager ScreenMenuManagerInstance {
            get {
                if (smmm == null) {
                    smmm = ScreenMenuManager.instance;
                }
                return smmm;
            }
        }
        TutorKuriManager tkm;
        TutorKuriManager TutorKuriManagerInstance {
            get {
                if (tkm == null) {
                    tkm = TutorKuriManager.instance;
                }
                return tkm;
            }
        }
        #endregion
        #region overrides
        // this is pathetically inefficient and poorly written, but it works
        protected override void OnStart() {
            handHit = false;
            startTime = Time.time;
            kuriArms = context.kuriArms;
            kuriArms.RightIKTarget.SetCollider(true);
            kuriArms.RightIKTarget.OnHitHand.AddListener(OnHitHand);
            RPulseMeshRend = context.kuriArms.RPulseMeshRend;
            RPulseMeshRend.StartPulse(Color.green);
            blackboard.ArmAnimatorSemaphoreCount -= 1;
            initialized = true;
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.low);
            KuriTextManager.instance.Addline("Give me a high five!", KuriTextManager.PRIORITY.low);
        }

        protected override void OnStop() {
            if (initialized) {
                kuriArms.RightIKTarget.OnHitHand.RemoveListener(OnHitHand);
                blackboard.emotion = KuriController.EMOTIONS.h5_end;
                blackboard.ArmAnimatorSemaphoreCount += 1;
                kuriArms.RightIKTarget.SetCollider(false);
                RPulseMeshRend.StopPulse();
                KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.low);
                if (MazeManagerInstance.ExerciseInFullyCompleteState) {
                    ScreenMenuManagerInstance.MyOnScreenPlayButton.SetToNextMazeWPulse();
                }
            }
        }

        protected override State OnUpdate() {
            if (handHit || !TutorKuriManagerInstance.IsOn) { // want this to end right away if this is not on
                return State.Success;
            }
            if (Time.time - startTime > maxTimeToWait) {
                return State.Failure;
            }
            return State.Running;
        }
        #endregion
        #region helpers
        void OnHitHand() {
            handHit = true;
            AudioManager.PlayHighFive();
        }
        #endregion
    }
}
