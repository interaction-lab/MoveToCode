using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriUtilityAI : MonoBehaviour {
        // Other components we need
        HumanStateManager humanStateManager;


        // Animation curves
        public AnimationCurve movementCurve;

        void Awake() {
            humanStateManager = HumanStateManager.instance;
        }

    

         void Update() {
            Debug.Log(movementCurve.Evaluate(humanStateManager.GetMovementNorm()));
        }
        

    }
}
