using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MoveToCode {
    public class KuriUtilityAI : KuriAI {
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

        void ChooseNewAction() {
            List<float> scores = new List<float> { ScoreDoNothing(), ScoreCuriosity(), ScoreMovement() };
            Debug.Log(scores.MaxIndex());
        }

        float ScoreDoNothing() {
            return doNothingCurve.Evaluate(
                kuriManager.TimeLastActionEnded.TimeSince() /
                30.0f)
                ;
        }

        float ScoreMovement() {
            return movementCurve.Evaluate(humanStateManager.GetMovementCDF());
        }

        float ScoreCuriosity() {
            return curiosityCurve.Evaluate(humanStateManager.GetCuriosityCDF());
        }
    }
}
