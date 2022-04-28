using UnityEngine;

namespace MoveToCode {
    public class ExerciseScaffolding : MonoBehaviour {
        public string[] scafoldDialogue;
        int curScaffold = 0;

        public void SayNextScaffold() {
            if (curScaffold >= scafoldDialogue.Length) {
                TutorKuriManager.instance.kuriController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Encouragement);
                return;
            }
            KuriTextManager.instance.Addline(scafoldDialogue[curScaffold++], KuriTextManager.PRIORITY.high); // high so that it does disappear
        }

        public void SetScaffoldDialogue(string[] dialogue) {
            scafoldDialogue = dialogue;
        }
    }
}
