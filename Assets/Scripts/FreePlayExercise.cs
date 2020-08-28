using UnityEngine;

namespace MoveToCode {
    public class FreePlayExercise : Exercise {
        protected override void OnEnable() {
            Resources.FindObjectsOfTypeAll<FreePlayMenuManager>()[0].gameObject.SetActive(true);
            base.OnEnable();
            GetExerciseInternals().SetKuriGoalString("This is freePlay, feel free to explore");
            GetExerciseInternals().SetConsoleStringGoal("IMPOSSIBLE");
        }
    }
}
