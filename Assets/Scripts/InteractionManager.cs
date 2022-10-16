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

        IEnumerator SetConditionOnDeviceID(bool startUp) {
            bool kuriIsOn = !OddDevices.Contains(UserIDManager.DeviceId); // allows for starting with kuri when in editor etc
            if (!startUp) {
                kuriIsOn = !kuriIsOn; // flip halfway
            }

            if(kuriIsOn){
                TutorKuriManagerInstance.SetKuriVisibility(kuriIsOn);
            }
           
            TutorKuriManagerInstance.SetKC(TutorKuriManagerInstance.robotKC); // does not flip, always whatever is set (-0.5 for experiment)
            // do the wave animation
            TutorKuriManagerInstance.Wave();
            if (kuriIsOn) {
                KuriTextManager.instance.Addline("Hello, I'm Kuri!");
            }
            else {
                KuriTextManager.instance.Addline("I'm going to take a break now, goodbye!");
            }
            yield return new WaitForSecondsRealtime(4.1f); // length of wave anim + some wiggle

            TutorKuriManagerInstance.Wave(); // do a second wave because it is so short and I don't want to make another animation
            yield return new WaitForSecondsRealtime(4.1f); // length of wave anim + some wiggle

            if(!kuriIsOn){ // let wave goodbye
                TutorKuriManagerInstance.SetKuriVisibility(kuriIsOn);
            }
            KuriTextManager.instance.Clear();
        }

        IEnumerator PolicySwapCoroutine() {

            // warm up routine (let things level out)
            TutorKuriManagerInstance.SetKC(reallyHighKC);
            TutorKuriManagerInstance.SetKuriVisibility(false); // turn off during warm up time
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