using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace MoveToCode {
    public class KuriUtilityAI : KuriAI {
        enum RAW_SCORES {
            Movement,
            Curiosity,
            Idle
        }

        enum COMPOSITE_SCORES {
            Idle,
            VirtualISA,
            Move
        }        

        // Other components we need
        HumanStateManager humanStateManager;
        KuriManager kuriManager;

        // Animation curves
        public AnimationCurve movementCurve, curiosityCurve, doNothingCurve;

        void Awake() {
            humanStateManager = HumanStateManager.instance;
            kuriManager = KuriManager.instance;
        }

        public override void Tick() {
            if (!kuriManager.kuriController.IsDoingAction) {
                ChooseNewAction();
            }
        }

        float[] compositeScores = new float[Enum.GetValues( typeof( COMPOSITE_SCORES ) ).Length];
        void ChooseNewAction() {
           foreach(COMPOSITE_SCORES i in Enum.GetValues(typeof(COMPOSITE_SCORES))){
               compositeScores[(int)i] = GetCompositeScore(i);
           }
           Debug.Log(((COMPOSITE_SCORES)compositeScores.MaxIndex()).ToString());
        }

        float GetCompositeScore(COMPOSITE_SCORES cs){
            if(cs == COMPOSITE_SCORES.Idle){
                return GetRawScore(RAW_SCORES.Idle);
            }
            else if(cs == COMPOSITE_SCORES.Move){
                return GetRawScore(RAW_SCORES.Movement);
            }
            else if(cs == COMPOSITE_SCORES.VirtualISA){
                return GetRawScore(RAW_SCORES.Curiosity);
            }
            
            throw new NotImplementedException("Composite Score enum not supported"); 
        }
        float GetRawScore(RAW_SCORES sc) {
            if (sc == RAW_SCORES.Curiosity) {
                return curiosityCurve.Evaluate(humanStateManager.GetCuriosityCDF());
            }
            else if (sc == RAW_SCORES.Movement) {
                return movementCurve.Evaluate(humanStateManager.GetMovementCDF());
            }
            else if (sc == RAW_SCORES.Idle) {
                return doNothingCurve.Evaluate(
                                kuriManager.TimeLastActionEnded.TimeSince() /
                                30.0f)
                                ;
            }
            throw new NotImplementedException("Raw Scoring enum not supported"); 
        }
    }
}
