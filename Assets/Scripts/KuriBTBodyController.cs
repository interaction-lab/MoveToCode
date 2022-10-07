using TheKiwiCoder;
using UnityEngine;
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

        UnityEvent OnMoveToObj = new UnityEvent();
        UnityEvent OnTurnToObj = new UnityEvent();
        UnityEvent OnPointToObj = new UnityEvent();
        UnityEvent OnLookAtObj = new UnityEvent();
        UnityEvent OnDoAnimation = new UnityEvent();
        UnityEvent OnEndAllSeq = new UnityEvent();

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
            throw new System.NotImplementedException();
        }

        public override string DoRandomPositiveAction() {
            throw new System.NotImplementedException();
        }

        public override string PointAtObj(Transform obj, float time) {
            _TurnToObj(obj);
            _PointAtObj(obj);
            return "Pointing to " + obj.name;
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

        #endregion

        #region protected

        protected override bool UpdateCurrentActionString() {
            return false; // TODO: figure this out later lol
        }

        protected override void Init() {
            // add all necessary Unity Events
            KEventRouter.AddEvent(EventNames.OnMoveToObj, OnMoveToObj);
            KEventRouter.AddEvent(EventNames.OnTurnToObj, OnTurnToObj);
            KEventRouter.AddEvent(EventNames.OnPointToObj, OnPointToObj);
            KEventRouter.AddEvent(EventNames.OnLookAtObj, OnLookAtObj);
            KEventRouter.AddEvent(EventNames.OnDoAnimation, OnDoAnimation);
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
            OnPointToObj.Invoke();
        }

        void _DoAnimation(EMOTIONS e) {
            KuriBlackBoard.emotion = e;
            OnDoAnimation.Invoke();
        }

        void _EndAllSeq() {
            OnEndAllSeq.Invoke();
        }
        #endregion
    }
}
