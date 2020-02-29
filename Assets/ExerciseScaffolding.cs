using UnityEngine;

namespace MoveToCode {
    public class ExerciseScaffolding : MonoBehaviour {
        public string[] scafoldDialogue;
        int curScaffold = 0;

        public void SayNextScaffold() {
            if (curScaffold >= scafoldDialogue.Length) {
                KuriManager.instance.SayAndDoPositiveAffect();
                return;
            }
            KuriTextManager.instance.Addline(scafoldDialogue[curScaffold++]);
        }

    }
}
