using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace MoveToCode {
    public class BabyVirtualKuriController : MonoBehaviour {

        #region members
        public string CurMovementAction = "";
        public static string babyKuriMovementActionCol = "babyKuriMovementAction";
        private float moveSpeed = 1f, turnSpeed = 50f;
        public AnimationCurve speedCurve;
        public MeshRenderer bodyPlateRend;

        BabyKuriManager bkm;
        BabyKuriManager babyKuriManager {
            get {
                if (bkm == null) {
                    bkm = BabyKuriManager.instance;
                }
                return bkm;
            }
        }

        private BabyKuriTransformManager _bktransformmanager;
        public BabyKuriTransformManager BKTransformManager {
            get {
                if (_bktransformmanager == null) {
                    _bktransformmanager = transform.parent.GetComponent<BabyKuriTransformManager>();
                }
                return _bktransformmanager;
            }
        }

        public static string MoveLogString { get; } = "Moving ";
        public static string TurnLogString { get; } = "Turning ";

        private Regex moveRe = new Regex(@"^" + MoveLogString);
        private Regex turnRe = new Regex(@"^" + TurnLogString);
        public bool IsMoving {
            get {
                return (moveRe.IsMatch(CurMovementAction) || turnRe.IsMatch(CurMovementAction)) && !interpreter.CodeIsAtStart();
            }
        }
        Interpreter _interpreter;
        Interpreter interpreter {
            get {
                if (_interpreter == null) {
                    _interpreter = Interpreter.instance;
                }
                return _interpreter;
            }
        }
        MazeManager _mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (_mazeManager == null) {
                    _mazeManager = MazeManager.instance;
                }
                return _mazeManager;
            }
        }

        /// <summary>
        /// Type is the type of movement
        /// float is the amount of movement (degrees for turns, dist in meters for moving)
        /// </summary>
        Queue<KeyValuePair<Type, float>> MoveQueue { get; set; } = new Queue<KeyValuePair<Type, float>>();

        #endregion

        #region unity
        private void Awake() {
            LoggingManager.instance.AddLogColumn(babyKuriMovementActionCol, "");
        }
        private void FixedUpdate() {
            LoggingManager.instance.UpdateLogColumn(babyKuriMovementActionCol, CurMovementAction);
        }
        #endregion

        #region public
        public void MoveOverTime(float dist) {
            MoveQueue.Enqueue(new KeyValuePair<Type, float>(typeof(CodeBlockEnums.Move), dist));
            StartNextMovement();
        }

        public void TurnOverTime(float degrees) {
            MoveQueue.Enqueue(new KeyValuePair<Type, float>(typeof(CodeBlockEnums.Turn), degrees));
            StartNextMovement();
        }

        public void ResetToOrigState() {
            BKTransformManager.ResetToOriginalState();
        }

        public void SetColor(Color color) {
            bodyPlateRend.material.color = color;
        }

        #endregion

        #region private
        private void StartNextMovement() {
            if (!MoveQueue.Empty() && !IsMoving) {
                KeyValuePair<Type, float> p = MoveQueue.Dequeue();
                if (p.Key == typeof(CodeBlockEnums.Move)) {
                    // Note this only works for moving forward, not backward, will fix later
                    CodeBlockEnums.Move move = CodeBlockEnums.Move.Forward;
                    Enum.TryParse(Convert.ToInt32(p.Key).ToString(), out move);

                    MazePiece potentialNextPiece = MazeManagerInstance.GetPotentialNextMP(move);
                    if(potentialNextPiece == null){
                        throw new Exception($"Kuri moving {move.ToString()}, but no next piece");
                    }
                    StartCoroutine(GoToPosition(MazeManagerInstance.GetPotentialNextMP(move).Center, move == CodeBlockEnums.Move.Forward));
                }
                else if (p.Key == typeof(CodeBlockEnums.Turn)) {
                    StartCoroutine(TurnToAngle(Quaternion.Euler(BKTransformManager.KuriRot.eulerAngles + BKTransformManager.Up * p.Value), p.Value > 0));
                }
            }
        }

        private float goalDistDelta = 0.02f;
        public IEnumerator GoToPosition(Vector3 goal, bool forward) {
            if (IsMoving) {
                throw new InvalidOperationException("Baby Kuri already moving, check moveQueue queueing code");
            }
            CurMovementAction = MoveLogString + (forward ? "FORWARD" : "BACKWARD") + " to " + goal.ToString();

            // need to use this curve nicely for different connections in the maze
            Bezier curve = new Bezier(Bezier.BezierType.Quadratic, new Vector3[]{BKTransformManager.KuriPos, BKTransformManager.KuriPos + BKTransformManager.Up * 0.5f, goal, goal});
            float t = 0f, totalTime = 1f;
            while (Vector3.Distance(BKTransformManager.KuriPos, goal) > goalDistDelta) {
                BKTransformManager.KuriPos = curve.GetBezierPoint(t/totalTime);
                t += Time.deltaTime;
                yield return null;
            }
            if (IsMoving) {
                BKTransformManager.KuriPos = goal;
            }
            ResetCurMovementAction();
            StartNextMovement();
        }

        private float goalDegreeDelta = 3f;
        public IEnumerator TurnToAngle(Quaternion goal, bool right) {
            if (IsMoving) {
                throw new InvalidOperationException("Baby Kuri already moving, check moveQueue queueing code");
            }
            CurMovementAction = MoveLogString + (right ? "RIGHT" : "LEFT") + " to " + goal.ToString();
            int spinDir = right ? 1 : -1;
            float totalDist = Quaternion.Angle(BKTransformManager.KuriRot, goal);
            float curDist = totalDist;
            while (Mathf.Abs(curDist) > goalDegreeDelta && IsMoving) {
                float curSpeed = spinDir * turnSpeed * speedCurve.Evaluate(curDist / totalDist);
                BKTransformManager.KuriRot = Quaternion.Euler(BKTransformManager.KuriRot.eulerAngles + BKTransformManager.Up * curSpeed * Time.deltaTime);
                curDist = Quaternion.Angle(BKTransformManager.KuriRot, goal);
                yield return null;
            }
            if (IsMoving) {
                BKTransformManager.KuriRot = goal;
            }
            ResetCurMovementAction();
            StartNextMovement();
        }

        private void ResetCurMovementAction() {
            CurMovementAction = "";
        }
        #endregion
    }
}
