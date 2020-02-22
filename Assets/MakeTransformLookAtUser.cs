using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class MakeTransformLookAtUser : MonoBehaviour {
        public AnimationCurve animCurve;

        IEnumerator SlowTurn() {
            Quaternion start = transform.rotation, goal = Quaternion.LookRotation(Camera.main.transform.forward);
            float curTime = 0f, totalTime = 2.0f;

            while (curTime < totalTime) {
                float percent = curTime / totalTime;
                transform.localRotation = Quaternion.Lerp(start, goal, animCurve.Evaluate(percent));
                yield return new WaitForSeconds(Time.deltaTime);
                curTime += Time.deltaTime;
            }
        }
        public void LookAtUser() {
            Debug.Log("Turn");
            StartCoroutine(SlowTurn());
        }
    }
}
