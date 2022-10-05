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

        #endregion

        #region unity
        #endregion

        #region public
        // totally forgot what the string return is for..., probably a logger and worth looking at later
        public override string DoAnimationAction(EMOTIONS e) {
            throw new System.NotImplementedException();
        }

        public override string DoRandomNegativeAction() {
            throw new System.NotImplementedException();
        }

        public override string DoRandomPositiveAction() {
            throw new System.NotImplementedException();
        }

        public override string PointAtObj(Transform obj, float time) {
            KuriBlackBoard.objToPointTo = obj;
            KuriBlackBoard.objToLookAt = obj;
            OnPointToObj.Invoke();
            return "Pointing to " + obj.name;
        }

        public override string TakeISAAction() {
            throw new System.NotImplementedException();
        }

        public override string TakeMovementAction(int option = -1) {
            throw new System.NotImplementedException();
        }

        public override void TurnTowardsUser() {
            KuriBlackBoard.objToTurnTo = PlayerTransformManagerInstance.OriginT;
            OnTurnToObj.Invoke();
        }
        public override string MoveToObj(Transform obj) {
            KuriBlackBoard.objToMoveTo = obj;
            OnMoveToObj.Invoke();
            return "Moving to " + obj.name;
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
        }
        #endregion


        #region private
        #endregion
    }
}
