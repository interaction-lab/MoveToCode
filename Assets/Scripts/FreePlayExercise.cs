using UnityEngine;

namespace MoveToCode {
    public class FreePlayExercise : Exercise {
        protected override void OnEnable() {
            //kuriGoalString = "This is freePlay, feel free to explore"; //TODO: FixThis
            //consoleStringGoal = "IMPOSSIBLE";
            Resources.FindObjectsOfTypeAll<FreePlayMenuManager>()[0].gameObject.SetActive(true);
            base.OnEnable();
        }
    }
}
