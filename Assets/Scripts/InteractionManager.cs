using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class InteractionManager : Singleton<InteractionManager> {
        public float fullInteractionTimeMinutes, warmUpTimeMinutes;
        public float startKC = 1f;
        public float numIntervals = 10;
        float reallyHighKC = 700f;
        public bool startLow;
        bool kcState;

        private void Start() {
            StartCoroutine(PolicySwapCoroutine());
        }

        public float MinToSeconds(float f) {
            return f * 60f;
        }

        IEnumerator PolicySwapCoroutine() {
            KuriManager.instance.SetKC(reallyHighKC);
            yield return new WaitForSeconds(MinToSeconds(warmUpTimeMinutes));
            float timeLeft = MinToSeconds(fullInteractionTimeMinutes) - MinToSeconds(warmUpTimeMinutes);
            float intervalTime = timeLeft / numIntervals;
            for (int i = 0; i < numIntervals; ++i) {
                KuriManager.instance.SetKC(ChooseKCRAtInterval(i));
                yield return new WaitForSeconds(intervalTime);
            }
            Application.Quit();
        }

        public float ChooseKCRAtInterval(int atInterval) {
            float result = startKC;
            float addition = atInterval * (2.0f / (numIntervals - 1));
            if (startLow) {
                result = -startKC;
                result += addition;
            }
            else {
                result -= addition;
            }
            return result;
        }
    }
}