﻿using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class MakeTransformLookAtUser : MonoBehaviour {
        public Transform t;
        public AnimationCurve animCurve;
        Transform camTransform;

        private void Awake() {
            if (t == null) {
                t = transform;
            }
            camTransform = Camera.main.transform;
        }
        IEnumerator Turn() {
            Quaternion start = transform.rotation, goal = Quaternion.LookRotation(camTransform.forward);
            float curTime = 0f, totalTime = 0.5f;

            while (curTime < totalTime) {
                float percent = curTime / totalTime;
                t.localRotation = Quaternion.Lerp(start, goal, animCurve.Evaluate(percent));
                yield return new WaitForSeconds(Time.deltaTime);
                curTime += Time.deltaTime;
            }
        }

        public void LookAtUser() {
            StartCoroutine(Turn());
        }
    }
}
