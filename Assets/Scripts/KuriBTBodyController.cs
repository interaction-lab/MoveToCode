using System;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace MoveToCode {
    public class KuriBTBodyController : KuriController {
        #region members
        KuriBTEventRouter _kbter;
        KuriBTEventRouter KEventRouter {
            get {
                if (_kbter == null) {
                    _kbter = GetComponent<KuriBTEventRouter>();
                }
                return _kbter;
            }
        }
        Blackboard blackboard;
        Blackboard KuriBlackBoard {
            get {
                if (blackboard == null) {
                    blackboard = GetComponent<BehaviourTreeRunner>().tree.blackboard;
                }
                return blackboard;
            }
        }
        PlayerTransformManager ptm;
        PlayerTransformManager PlayerTransformManagerInstance {
            get {
                if (ptm == null) {
                    ptm = PlayerTransformManager.instance;
                }
                return ptm;
            }
        }

        float normalPointTime = 3f, pointUntilTime = 15f;
        ExerciseManager em;
        ExerciseManager ExerciseManagerInstance {
            get {
                if (em == null) {
                    em = ExerciseManager.instance;
                }
                return em;
            }
        }
        bool initialized = false;
        KuriAIBTRandom kuriAIBTRandom;
        KuriAIBTRandom KuriAIBTRandomInstance {
            get {
                if (kuriAIBTRandom == null) {
                    kuriAIBTRandom = GetComponent<KuriAIBTRandom>();
                }
                return kuriAIBTRandom;
            }
        }
        Animator BodyAnimator {
            get {
                return KuriAIBTRandomInstance.BodyAnimator;
            }
        }
        Animator ArmAnimator {
            get {
                return KuriAIBTRandomInstance.ArmAnimator;
            }
        }
        #endregion

        #region unity
        private void OnEnable() {
            if (!initialized) {
                ExerciseManagerInstance.OnCyleNewExercise.AddListener(OnCycleNewExercise);
                initialized = true;
            }
        }
        #endregion

        #region public
        // totally forgot what the string return is for..., probably a logger and worth looking at later -> yes it is
        public override string DoAnimationAction(EMOTIONS e) {
            _DoAnimation(e);
            return e.ToString();
        }

        public override string DoRandomNegativeAction() {
            EMOTIONS e = NegativeEmotions[UnityEngine.Random.Range(0, NegativeEmotions.Length)];
            _DoAnimation(e);
            return e.ToString();
        }

        public override string DoRandomPositiveAction() {
            EMOTIONS e = PositiveEmotions[UnityEngine.Random.Range(0, PositiveEmotions.Length)];
            _DoAnimation(e);
            return e.ToString();
        }

        public override void HighFive() {
            _HighFive();
        }

        public override string PointAtObj(Transform obj, float time = -1) {
            _LookAtObj(obj);
            _PointAtObj(obj);
            return obj.name;
        }

        public void PointUntilInteract(Transform obj) {
            _LookAtObj(obj);
            _PointUntilInteract(obj);
        }

        public void MoveAwayFromMaze() {
            _TurnMoveTurn(MoveAwayFromMazeObj.instance.transform);
        }

        public override string TakeISAAction() {
            throw new System.NotImplementedException();
        }

        public override string TakeMovementAction(int option = -1) {
            throw new System.NotImplementedException();
        }

        public override void TurnTowardsUser() {
            MoveToObj(PlayerTransformManagerInstance.OriginT);
        }
        public override string MoveToObj(Transform obj) {
            if (obj == MoveAwayFromMazeObj.instance.transform) {
                MoveAwayFromMaze();
            }
            else {
                _TurnToObj(obj);
                _MoveToObj(obj);
            }
            return "Moving to " + obj.name;
        }

        public void TurnAndLookAtObj(Transform obj) {
            _TurnToObj(obj);
            _LookAtObj(obj);
        }

        public void OnlyTurnToObj(Transform obj) {
            _TurnToObj(obj);
        }

        public void OnlyLookAtObj(Transform obj) {
            _LookAtObj(obj);
        }

        public void StopAllBeh() {
            _EndAllSeq();
        }
        public void Clap() {
            _DoAnimation(EMOTIONS.clap);
        }
        public void PointToPaper(string mpName) {
            _PointToPaper(mpName);
        }

        public void MoveToThenPoint(Transform obj) {
            _MoveThenPoint(obj);
        }

        #endregion

        #region protected

        protected override bool UpdateIsDoingAction() {
            return KuriBlackBoard.BodyAnimatorSemaphoreCount != 0 ||
                KuriBlackBoard.ArmAnimatorSemaphoreCount != 0;
        }


        UnityEvent OnMoveToObj = new UnityEvent();
        UnityEvent OnTurnToObj = new UnityEvent();
        UnityEvent OnPointToObj = new UnityEvent();
        UnityEvent OnLookAtObj = new UnityEvent();
        UnityEvent OnDoAnimation = new UnityEvent();
        UnityEvent OnEndAllSeq = new UnityEvent();
        UnityEvent OnStartH5 = new UnityEvent();
        UnityEvent OnPointUntilInteract = new UnityEvent();
        UnityEvent OnPointToPaper = new UnityEvent();
        UnityEvent OnTurnMoveTurn = new UnityEvent();
        UnityEvent OnMoveThenPoint = new UnityEvent();
        protected override void Init() {
            // add all necessary Unity Events
            KEventRouter.AddEvent(EventNames.OnMoveToObj, OnMoveToObj);
            KEventRouter.AddEvent(EventNames.OnTurnToObj, OnTurnToObj);
            KEventRouter.AddEvent(EventNames.OnPointToObj, OnPointToObj);
            KEventRouter.AddEvent(EventNames.OnLookAtObj, OnLookAtObj);
            KEventRouter.AddEvent(EventNames.OnDoAnimation, OnDoAnimation);
            KEventRouter.AddEvent(EventNames.OnEndAllSeq, OnEndAllSeq);
            KEventRouter.AddEvent(EventNames.OnStartH5, OnStartH5);
            KEventRouter.AddEvent(EventNames.OnPointUntilInteract, OnPointUntilInteract);
            KEventRouter.AddEvent(EventNames.OnPointToPaper, OnPointToPaper);
            KEventRouter.AddEvent(EventNames.OnTurnMoveTurn, OnTurnMoveTurn);
            KEventRouter.AddEvent(EventNames.OnMoveThenPoint, OnMoveThenPoint);
        }
        #endregion

        #region private
        void _TurnToObj(Transform obj) {
            KuriBlackBoard.objToTurnTo = obj;
            OnTurnToObj.Invoke();
        }
        void _LookAtObj(Transform obj) {
            KuriBlackBoard.objToLookAt = obj;
            OnLookAtObj.Invoke();
        }
        void _MoveToObj(Transform obj) {
            KuriBlackBoard.objToMoveTo = obj;
            OnMoveToObj.Invoke();
        }
        void _PointAtObj(Transform obj) {
            KuriBlackBoard.objToPointTo = obj;
            KuriBlackBoard.timeToPoint = normalPointTime;
            KEventRouter.ResetEvent(EventNames.OnInteractWith);
            OnPointToObj.Invoke();
        }

        void _PointUntilInteract(Transform obj) {
            // if player, just point normally
            if (obj == PlayerTransformManager.instance.OriginT) {
                _PointAtObj(obj);
                return;
            }
            KuriBlackBoard.objToPointTo = obj;
            KuriBlackBoard.timeToPoint = pointUntilTime; // long point time, still ends if they don't interact
            UntilInteract untilInteract = obj.GetComponent<UntilInteract>();
            if (untilInteract == null) {
                untilInteract = obj.gameObject.AddComponent<UntilInteract>();
            }

            KEventRouter.AddEvent(EventNames.OnInteractWith, untilInteract.OnInteract);
            OnPointUntilInteract.Invoke();
        }

        void _DoAnimation(EMOTIONS e) {
            KuriBlackBoard.emotion = e;
            OnDoAnimation.Invoke();
        }

        private void _HighFive() {
            KuriBlackBoard.emotion = EMOTIONS.h5_start;
            _LookAtObj(PlayerTransformManagerInstance.OriginT); // look at user to start
            OnStartH5.Invoke();
        }
        void _EndAllSeq() {
            OnEndAllSeq.Invoke();
            _ResetAnimators();
        }

        void _ResetAnimators() {
            KuriBlackBoard.BodyAnimatorSemaphoreCount = 0;
            KuriBlackBoard.ArmAnimatorSemaphoreCount = 0;
            BodyAnimator.Play("neutral");
            ArmAnimator.Play("Idle");
        }

        void OnCycleNewExercise() {
            _EndAllSeq();
        }

        void _PointToPaper(string mpName) {
            KuriBlackBoard.pointToPaperName = mpName;
            KuriBlackBoard.objToPointTo = MazePaper.instance.transform;

            _TurnToObj(PlayerTransformManager.instance.OriginT);
            _LookAtObj(KuriBlackBoard.objToPointTo);
            OnPointToPaper.Invoke();
            OnPointToObj.Invoke();
        }

        void _TurnMoveTurn(Transform obj) {
            KuriBlackBoard.objToTurnTo = obj;
            KuriBlackBoard.objToMoveTo = obj;
            OnTurnMoveTurn.Invoke();
        }

        void _MoveThenPoint(Transform obj) {
            KuriBlackBoard.objToMoveTo = obj;
            KuriBlackBoard.objToPointTo = obj;
            KuriBlackBoard.timeToPoint = normalPointTime;
            OnMoveThenPoint.Invoke();
        }
        #endregion
    }
}
