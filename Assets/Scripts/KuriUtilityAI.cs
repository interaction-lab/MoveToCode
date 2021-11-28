using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriUtilityAI : KuriAI {
        // Other components we need
        HumanStateManager humanStateManager;
        KuriManager kuriManager;

        // Animation curves
        public AnimationCurve movementCurve;

        void Awake() {
            humanStateManager = HumanStateManager.instance;
            kuriManager = KuriManager.instance;
        }

        public override void Tick() {
            Debug.Log(movementCurve.Evaluate(humanStateManager.GetMovementCDF()));
        }
    }
}
