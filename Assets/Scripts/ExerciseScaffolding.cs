using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class ExerciseScaffolding : MonoBehaviour {
        public string[] scafoldDialogue;
        public Transform[] objsOfInterest;
        int curScaffold = 0;

        public Pair<bool, Transform> SayNextScaffold() {
            Assert.IsTrue(scafoldDialogue.Length == objsOfInterest.Length, transform.name + ": Scaffold dialogue and objs of interest are not the same length");
            // check if the maze is correct, if so, do a high priority clear on kuri text
            if (MazeManager.instance.IsSameAsSolutionMaze()) {
                KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
            }
            if (curScaffold >= scafoldDialogue.Length) {
                return new Pair<bool, Transform>(false, null);
            }
            KuriTextManager.instance.Addline(scafoldDialogue[curScaffold++], KuriTextManager.PRIORITY.high); // high so that it does disappear
            return new Pair<bool, Transform>(true, objsOfInterest[curScaffold - 1]);
        }

        public void SetScaffoldDialogue(string[] dialogue) {
            scafoldDialogue = dialogue;
        }
    }
}
