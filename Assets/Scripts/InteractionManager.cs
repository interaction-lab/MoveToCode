using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class InteractionManager : Singleton<InteractionManager> {
        public float fullInteractionTimeMinutes, warmUpTimeMinutes;
        float numIntervals = 2;
        float reallyHighKC = 700f;
        TutorKuriManager tkm;
        TutorKuriManager TutorKuriManagerInstance {
            get {
                if (tkm == null) {
                    tkm = TutorKuriManager.instance;
                }
                return tkm;
            }
        }

        HashSet<string> OddDevices = new HashSet<string>() {

        };

        private void Start() {
            StartCoroutine(PolicySwapCoroutine());
        }

        public float MinToSeconds(float f) {
            return f * 60f;
        }

        void SetConditionOnDeviceID(bool startUp) {
            bool cond = OddDevices.Contains(UserIDManager.DeviceId);
            if (!startUp) {
                cond = !cond; // flip halfway
            }
            TutorKuriManagerInstance.SetKuriVisibility(cond);
            TutorKuriManagerInstance.SetKC(TutorKuriManagerInstance.robotKC); // does not flip, always whatever is set (-0.5 for experiment)
        }

        IEnumerator PolicySwapCoroutine() {

            TutorKuriManagerInstance.SetKC(reallyHighKC);
            TutorKuriManagerInstance.SetKuriVisibility(false); // turn off during warm up time
            yield return new WaitForSeconds(MinToSeconds(warmUpTimeMinutes));


            SetConditionOnDeviceID(true);

            float timeLeft = MinToSeconds(fullInteractionTimeMinutes) - MinToSeconds(warmUpTimeMinutes);
            float intervalTime = timeLeft / numIntervals;
            yield return new WaitForSeconds(intervalTime);

            SetConditionOnDeviceID(false);
            Application.Quit();
        }
    }
}