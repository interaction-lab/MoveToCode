using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriUtilityAI : MonoBehaviour {
        // Other components we need
        HumanStateManager humanStateManager;
        KuriManager kuriManager;


        // Animation curves
        public AnimationCurve movementCurve;

        // variables we care about
        //  movement
        //  curiosity
        //  time since last action
        //  rolling time window threshold
        // variable lists
        //  novelty of each
        // action list
        //  variable actions
        //      virtual ISA
        //      hint/scaffold dialogue
        //      PPA
        //      move
        //  hardcoded actions
        //      give exercise
        //      congrats dialogue  

        void Awake() {
            humanStateManager = HumanStateManager.instance;
            kuriManager = KuriManager.instance;
        }

    

         void Update() {
            Debug.Log(movementCurve.Evaluate(humanStateManager.GetMovementCDF()));
        }
        

    }
}
