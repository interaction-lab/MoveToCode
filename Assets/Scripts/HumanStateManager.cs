using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class HumanStateManager : Singleton<HumanStateManager> {
        public float timeWindow;
        float curiosity_t = 0f, movement_t = 0f, curiosity_average = 0f, movement_average = 0f, curiosity_SSE = 0f, movement_SSE = 0f;
        Queue<float> infoSeekingActionQueue;
        Queue<Vector3> headposeQueue;
        Vector3 lastHeadPoseEnqueued;
        long totalTimeSteps = 0;

        static string humanCurtCol = "humanCurt", humanMovetCol = "humanMovet", humanMoveZScore = "humanMoveZScore", humanCurZScore = "humanCurZScore";

        private void Start() {
            LoggingManager.instance.AddLogColumn(humanCurtCol, "");
            LoggingManager.instance.AddLogColumn(humanMoveZScore, "");
            LoggingManager.instance.AddLogColumn(humanMovetCol, "");
            LoggingManager.instance.AddLogColumn(humanCurZScore, "");
            //StartCoroutine(WaitForScoresToAverageOut());
        }

        IEnumerator WaitForScoresToAverageOut() {
            yield return new WaitForSeconds(timeWindow);
        }

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
            return 0.5f * GetZScoreMovement() + 0.5f * GetZScoreCuriosity();
        }

        public int GetTimeQueueSizeNormalized() {
            return (int)(timeWindow / Time.deltaTime);
        }

        public void UpdateKC() {
            int timeLength = GetTimeQueueSizeNormalized();
            ++totalTimeSteps;
            UpdateCuriosity(timeLength);
            UpdateMovement(timeLength);
        }

        public void DebugLogData() {
            Debug.Log("C " + curiosity_t.ToString());
            Debug.Log("M " + movement_t.ToString());
            Debug.Log("T " + GetTimeQueueSizeNormalized().ToString());
            Debug.Log("KCT " + GetKCt());
        }

        public float GetZScoreMovement() {
            return (movement_t - movement_average) / Mathf.Sqrt(GetMovementVariance());
        }

        public float GetMovementVariance() {
            return movement_SSE / (totalTimeSteps - 1);
        }

        public float GetZScoreCuriosity() {
            return (curiosity_t - curiosity_average) / Mathf.Sqrt(GetCuriosityVariance());
        }

        public float GetCuriosityVariance() {
            return curiosity_SSE / (totalTimeSteps - 1);
        }

        private void UpdateMovement(int len) {
            while (GetHeadPoseQueue().Count > len) {
                Vector3 popped = headposeQueue.Dequeue();
                movement_t -= Vector3.Distance(popped, headposeQueue.Peek());
            }
            Vector3 nextPos = Camera.main.transform.position;
            float newDist = Vector3.Distance(nextPos, lastHeadPoseEnqueued);
            movement_t += newDist;

            float et = movement_t - movement_average;
            movement_average += et / totalTimeSteps;
            movement_SSE += et * (movement_t - movement_average);

            headposeQueue.Enqueue(nextPos);
            lastHeadPoseEnqueued = nextPos;

            LoggingManager.instance.UpdateLogColumn(humanMovetCol, movement_t.ToString("F3"));

            float zMove = GetZScoreMovement();
            if (!float.IsInfinity(zMove) && !float.IsNaN(zMove)) {
                LoggingManager.instance.UpdateLogColumn(humanMoveZScore, zMove.ToString("F3"));
            }
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
            else if (LoggingManager.instance.GetValueInRowAt(SnapLoggingManager.GetSnapRemoveFromColName()) != "") {
                result = 1;
            }
            curiosity_t += result;

            float et = curiosity_t - curiosity_average;
            curiosity_average += et / totalTimeSteps;
            curiosity_SSE += et * (curiosity_t - curiosity_average);

            infoSeekingActionQueue.Enqueue(result);


            LoggingManager.instance.UpdateLogColumn(humanCurtCol, curiosity_t.ToString("F3"));
            float zCur = GetZScoreCuriosity();
            if (!float.IsInfinity(zCur) && !float.IsNaN(zCur)) {
                LoggingManager.instance.UpdateLogColumn(humanCurZScore, zCur.ToString("F3"));
            }
        }


    }
}