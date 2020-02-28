using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class HumanStateManager : Singleton<HumanStateManager> {
        public float time_window;
        float curiosity_t, movement_t, curiosity_max = 0.01f, movement_max = 0.01f;
        Queue<float> infoSeekingActionQueue;
        Queue<Vector3> headposeQueue;
        Vector3 lastHeadPoseEnqueued;

        public Queue<float> GetInforSeekingActionQueue() {
            if (infoSeekingActionQueue == null) {
                infoSeekingActionQueue = new Queue<float>();
            }
            return infoSeekingActionQueue;
        }

        public Queue<Vector3> GetHeadPoseQueue() {
            if (headposeQueue == null) {
                headposeQueue = new Queue<Vector3>();
                headposeQueue.Enqueue(Vector3.zero);
            }
            return headposeQueue;
        }

        public float GetKCt() {
            return 0.5f * (movement_t / movement_max) + 0.5f * (curiosity_t / curiosity_max);
        }

        public int GetTimeQueueSizeNormalized() {
            return (int)(time_window / Time.deltaTime);
        }

        public void UpdateKC() { // LoggingManager calls update, might need better way
            int timeLength = GetTimeQueueSizeNormalized();
            UpdateCuriosity(timeLength);
            UpdateMovement(timeLength);
        }

        public void DebugLogData() {
            Debug.Log("C " + curiosity_t.ToString());
            Debug.Log("M " + movement_t.ToString());
            Debug.Log("T " + GetTimeQueueSizeNormalized().ToString());
            Debug.Log("KCT " + GetKCt());
        }

        private void UpdateMovement(int len) {
            while (GetHeadPoseQueue().Count > len) {
                Vector3 popped = headposeQueue.Dequeue();
                movement_t -= Vector3.Distance(popped, headposeQueue.Peek());
            }
            Vector3 nextPos = Camera.main.transform.position;
            movement_t += Vector3.Distance(nextPos, lastHeadPoseEnqueued);
            if (movement_t > movement_max) {
                movement_max = movement_t;
            }
            headposeQueue.Enqueue(nextPos);
            lastHeadPoseEnqueued = nextPos;
        }

        private void UpdateCuriosity(int len) {
            while (GetInforSeekingActionQueue().Count > len) {
                curiosity_t -= infoSeekingActionQueue.Dequeue();
            }
            int result = 0;
            if (LoggingManager.instance.GetValueInRowAt(ManipulationLoggingManager.GetColName()) != "") {
                result = 1;
            }
            else if (LoggingManager.instance.GetValueInRowAt(SnapLoggingManager.GetSnapToColName()) != "") {
                result = 1;
            }
            curiosity_t += result;
            if (curiosity_t > curiosity_max) {
                curiosity_max = curiosity_t;
            }
            infoSeekingActionQueue.Enqueue(result);
        }


    }
}