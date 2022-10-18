using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class InteractionManager : Singleton<InteractionManager> {
        public float fullInteractionTimeMinutes, warmUpTimeMinutes;
        LoggingManager lm;
        LoggingManager LoggingManagerInstance {
            get {
                if (lm == null) {
                    lm = LoggingManager.instance;
                }
                return lm;
            }
        }
        public static string conditionCol = "conditionCol";
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
            "6DD050A5-CA3D-4C22-8D95-87F742A56E85", // 9
            "AA98E34A-9168-4D39-BC77-8732DF9C7C34", // 7
            "9C404098-2EDD-4A6E-8704-F097B50C4950", // 5
            "225BBB05-9853-4538-A13D-425875117AFB", // 3
            "A80157B5-F303-4B31-8E2F-C704F4DBF681" // 1
        };

        private void Start() {
            LoggingManagerInstance.AddLogColumn(conditionCol, "");
            StartCoroutine(PolicySwapCoroutine());
        }

        public float MinToSeconds(float f) {
            return f * 60f;
        }

        IEnumerator SetConditionOnDeviceID(bool startUp) {
            bool kuriWasOn = TutorKuriManagerInstance.IsOn;
            bool kuriIsOn = !OddDevices.Contains(UserIDManager.DeviceId); // allows for starting with kuri when in editor etc
            if (!startUp) {
                kuriIsOn = !kuriIsOn; // flip halfway
            }

            if (kuriIsOn) {
                TutorKuriManagerInstance.SetKuriVisibility(kuriIsOn);
            }

            TutorKuriManagerInstance.SetKC(-0.5f); // does not flip, always whatever is set (-0.5 for experiment)
            // do the wave animation
            LoggingManagerInstance.UpdateLogColumn(conditionCol, "Wave");
            TutorKuriManagerInstance.Wave();
            if (kuriIsOn) {
                KuriTextManager.instance.Addline("Hello, I'm Kuri! I'm here to help.");
            }
            else if (kuriWasOn) {
                KuriTextManager.instance.Addline("I'm going to take a break now, goodbye!");
            }
            yield return new WaitForSecondsRealtime(4.01f); // length of wave anim + some wiggle

            TutorKuriManagerInstance.Wave(); // do a second wave because it is so short and I don't want to make another animation
            yield return new WaitForSecondsRealtime(4.01f); // length of wave anim + some wiggle

            LoggingManagerInstance.UpdateLogColumn(conditionCol, kuriIsOn.ToString());
            if (!kuriIsOn) { // let wave goodbye
                TutorKuriManagerInstance.SetKuriVisibility(kuriIsOn);
                TutorKuriManagerInstance.EndAllBehaviors();
            }
            KuriTextManager.instance.Clear();
        }

        IEnumerator PolicySwapCoroutine() {

            // warm up routine (let things level out)
            TutorKuriManagerInstance.SetKC(reallyHighKC);
            TutorKuriManagerInstance.SetKuriVisibility(false); // turn off during warm up time
            LoggingManagerInstance.UpdateLogColumn(conditionCol, "warmup");
            yield return new WaitForSecondsRealtime(MinToSeconds(warmUpTimeMinutes));


            float timeLeft = MinToSeconds(fullInteractionTimeMinutes) - MinToSeconds(warmUpTimeMinutes);
            float intervalTime = timeLeft / numIntervals;

            // first condition
            yield return SetConditionOnDeviceID(true);
            yield return new WaitForSecondsRealtime(intervalTime);


            // second condition
            yield return SetConditionOnDeviceID(false);
            yield return new WaitForSecondsRealtime(intervalTime);

            Debug.Log("Quiting at time: " + Time.time);
            Application.Quit();
        }
    }
}