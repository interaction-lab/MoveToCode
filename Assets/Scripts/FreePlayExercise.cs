using UnityEngine;

namespace MoveToCode {
    public class FreePlayExercise : Exercise {
        protected override void OnEnable() {
            kuriGoalString = "This is freePlay, feel free to explore";
            consoleStringGoal = "IMPOSSIBLE";
            base.OnEnable();
        }
    }
}