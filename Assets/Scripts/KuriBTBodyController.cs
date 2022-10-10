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
                    ptm = Camera.main.GetComponent<PlayerTransformManager>();
                }
                return ptm;
            }
        }

        float normalPointTime = 3f, pointUntilTime = 15f;
        #endregion

        #region unity
        #endregion

        #region public
        // totally forgot what the string return is for..., probably a logger and worth looking at later
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
            _TurnToObj(obj);
            _MoveToObj(obj);
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

        #endregion

        #region protected

        protected override bool UpdateCurrentActionString() {
            return false; // TODO: figure this out later lol
        }


        UnityEvent OnMoveToObj = new UnityEvent();
        UnityEvent OnTurnToObj = new UnityEvent();
        UnityEvent OnPointToObj = new UnityEvent();
        UnityEvent OnLookAtObj = new UnityEvent();
        UnityEvent OnDoAnimation = new UnityEvent();
        UnityEvent OnEndAllSeq = new UnityEvent();
        UnityEvent OnStartH5 = new UnityEvent();
        UnityEvent OnPointUntilInteract = new UnityEvent();
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
            if (obj == Camera.main.transform) {
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
            OnStartH5.Invoke();
        }
        void _EndAllSeq() {
            // check if time has at least passed a little bit
            if (Time.time < 2f) {
                return;
            }
            OnEndAllSeq.Invoke();
        }
        #endregion
    }
}
