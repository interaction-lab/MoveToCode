using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Events;

namespace MoveToCode {
    public class KuriAIBTRandom : KuriAI {
        #region members
        UnityEvent OnStartPointToObj = new UnityEvent();

        float timeLastActionStarted = 0;
        Blackboard blackboard;
        Blackboard KuriBlackBoard {
            get {
                if (blackboard == null) {
                    blackboard = GetComponent<BehaviourTreeRunner>().tree.blackboard;
                }
                return blackboard;
            }
        }
        KuriBTEventRouter _eventRouter;
        KuriBTEventRouter eventRouter {
            get {
                if (_eventRouter == null) {
                    _eventRouter = KuriBTEventRouter.instance;
                }
                return _eventRouter;
            }
        }
        #endregion

        #region unity
        void Awake() {
            eventRouter.AddEvent(EventNames.StartPointToObj, OnStartPointToObj);
        }
        #endregion

        #region public
        public override void ForceHelpfulAction() {
            throw new System.NotImplementedException();
        }

        public override void Tick() {
            // launch event every 15 seconds
            if (Time.time - timeLastActionStarted > 15) {
                DoRandomBTAction();
                timeLastActionStarted = Time.time;
            }
        }

        public void DoRandomBTAction() {
            // going to do point at obj
            PointAtObj(Camera.main.transform);
        }
        #endregion

        #region private



        private void PointAtObj(Transform obj) {
            KuriBlackBoard.objToPointTo = obj;
            Debug.Log(KuriBlackBoard.objToPointTo);
            KuriBlackBoard.objToLookAt = obj;
            // fire event
            OnStartPointToObj.Invoke();

        }
        #endregion
    }
}
