using UnityEngine;

namespace MoveToCode {
    public class FreePlayExercise : Exercise {
        protected override void OnEnable() {
            GetExerciseInternals().SetKuriGoalString("This is freePlay, feel free to explore");
            GetExerciseInternals().SetConsoleStringGoal("IMPOSSIBLE");
            Resources.FindObjectsOfTypeAll<FreePlayMenuManager>()[0].gameObject.SetActive(true);
            base.OnEnable();
        }
    }
}
