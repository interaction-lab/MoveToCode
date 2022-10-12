using UnityEngine;

namespace MoveToCode {
    public class ExerciseScaffolding : MonoBehaviour {
        public string[] scafoldDialogue;
        int curScaffold = 0;

        public bool SayNextScaffold() {
            if (curScaffold >= scafoldDialogue.Length) {
                TutorKuriManager.instance.KController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Encouragement);
                return false;
            }
            KuriTextManager.instance.Addline(scafoldDialogue[curScaffold++], KuriTextManager.PRIORITY.high); // high so that it does disappear
            return true;
        }

        public void SetScaffoldDialogue(string[] dialogue) {
            scafoldDialogue = dialogue;
        }
    }
}
