using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class InteractionManager : Singleton<InteractionManager> {
        public float fullInteractionTimeMinutes, warmUpTimeMinutes;
        public float[] kcCond = { 0, 0 };
        bool kcState;

        private void Start() {
            StartCoroutine(WaitToSwap());
        }

        public float MinToSeconds(float f) {
            return f * 60f;
        }

        IEnumerator WaitToSwap() {
            KuriManager.instance.SetKC(ChooseKCR());
            yield return new WaitForSeconds((MinToSeconds(fullInteractionTimeMinutes) - MinToSeconds(warmUpTimeMinutes)) / 2.0f);
            KuriManager.instance.SetKC(ChooseKCR());
            yield return new WaitForSeconds((MinToSeconds(fullInteractionTimeMinutes) - MinToSeconds(warmUpTimeMinutes)) / 2.0f);
            Application.Quit();
        }

        // TODO: might make this into my choice before hand
        public float ChooseKCR() {
            int choice = Random.Range(0, kcCond.Length);
            float result = kcCond[choice];
            kcCond[choice] = kcCond[(choice + 1) % kcCond.Length];
            return result;
        }
    }
}