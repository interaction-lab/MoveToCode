using UnityEngine;

namespace MoveToCode {
    public class FreePlayExercise : Exercise {
        protected override void OnEnable() {
            //kuriGoalString = "This is freePlay, feel free to explore";
            //consoleStringGoal = "IMPOSSIBLE";
            Resources.FindObjectsOfTypeAll<FreePlayMenuManager>()[0].gameObject.SetActive(true);
            base.OnEnable();
        }
    }
}
