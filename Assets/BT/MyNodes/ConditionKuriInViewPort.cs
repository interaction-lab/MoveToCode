using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class ConditionKuriInViewPort : MonitorCondition {
        ViewPortManager _viewPortManager;
        ViewPortManager viewPortManager {
            get {
                if (_viewPortManager == null) {
                    _viewPortManager = ViewPortManager.instance;
                }
                return _viewPortManager;
            }
        }
        TutorKuriTransformManager _tutorKuriTransformManager;
        TutorKuriTransformManager tutorKuriTransformManager {
            get {
                if (_tutorKuriTransformManager == null) {
                    _tutorKuriTransformManager = TutorKuriTransformManager.instance;
                }
                return _tutorKuriTransformManager;
            }
        }
        ArrowPointPrefab _arrowPointPrefab = null;
        protected override void OnStart() {
            _arrowPointPrefab = viewPortManager.GetArrowPoint(tutorKuriTransformManager.OriginT);
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return _arrowPointPrefab.IsInViewPort ? State.Success : State.Failure;
        }
    }
}
