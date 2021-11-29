using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class HumanStateManager : Singleton<HumanStateManager> {
        public float timeWindow;

        public bool IsDoingAction {
            get {
                return ManipulationLogger.currentlyManipulating;
            }
        }

        public float LastTimeHumanDidAction { get; set; } = 0f;

        float curiosity_t = 0f, movement_t = 0f, curiosity_average = 0f, movement_average = 0f, curiosity_SSE = 0f, movement_SSE = 0f;
        Queue<float> infoSeekingActionQueue;
        Queue<Vector3> headposeQueue;
        Vector3 lastHeadPoseEnqueued;
        long totalTimeSteps = 1;

        static string humanCurtCol = "humanCurt", humanMovetCol = "humanMovet", humanMoveZScore = "humanMoveZScore", humanCurZScore = "humanCurZScore", humanCurAction = "humanCurAction";

        private void Start() {
            LoggingManager.instance.AddLogColumn(humanCurtCol, "");
            LoggingManager.instance.AddLogColumn(humanMoveZScore, "");
            LoggingManager.instance.AddLogColumn(humanMovetCol, "");
            LoggingManager.instance.AddLogColumn(humanCurZScore, "");
            LoggingManager.instance.AddLogColumn(humanCurAction, "");
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


        // constants
        float a1 = 0.254829592f;
        float a2 = -0.284496736f;
        float a3 = 1.421413741f;
        float a4 = -1.453152027f;
        float a5 = 1.061405429f;
        float p = 0.3275911f;
        public float NormalDist(float x) {

            float sign = 1f;
            if (x < 0) {
                sign = -1;
            }
            x = Mathf.Abs(x) / Mathf.Sqrt(2.0f);

            // A&S formula 7.1.26
            float t = 1.0f / (1.0f + p * x);
            float y = 1.0f - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Mathf.Exp(-x * x);

            return 0.5f * (1.0f + sign * y);
        }
        public float GetCuriosityCDF() {
            return NormalDist(GetZScoreCuriosity());
        }
        public float GetMovementCDF() {
            return NormalDist(GetZScoreMovement());
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

        void Update() {
            if(IsDoingAction){
                LastTimeHumanDidAction = Time.time;
                LoggingManager.instance.UpdateLogColumn(humanCurAction, 1); // TODO: get the actual aciton
            }
        }
        public void DebugLogData() {
            Debug.Log("C " + GetZScoreCuriosity().ToString());
            Debug.Log("M " + GetZScoreMovement().ToString());
            Debug.Log("T " + GetTimeQueueSizeNormalized().ToString());
            Debug.Log("KCT " + GetKCt());
            Debug.Log("movement_t " + movement_t.ToString());
        }

        public float GetZScoreMovement() {
            if (GetMovementVariance() == 0) {
                return 0;
            }
            return (movement_t - movement_average) / Mathf.Sqrt(GetMovementVariance());
        }

        public float GetMovementVariance() {
            return movement_SSE / (totalTimeSteps - 1);
        }

        public float GetZScoreCuriosity() {
            if (GetCuriosityVariance() == 0) {
                return 0;
            }
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

        // TODO: make this about novelty of action as opposed to just any action
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