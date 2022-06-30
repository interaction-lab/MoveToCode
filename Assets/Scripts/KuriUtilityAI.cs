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
            KuriLastActionEnded,
            KuriLastActionStarted,
            KuriDoingAction,
            HumanDoingAction,
            HumanLastActionEnded
        }

        enum COMPOSITE_SCORES {
            Idle,
            VirtualISA,
            Move
        }

        // Other components we need
        HumanStateManager humanStateManager;
        TutorKuriManager kuriManager;

        // Animation curves
        public AnimationCurve movementCurve, curiosityCurve, doNothingCurve;

        void Awake() {
            humanStateManager = HumanStateManager.instance;
            kuriManager = TutorKuriManager.instance;
        }

        public override void Tick() {
            if (!kuriManager.kuriController.IsDoingAction) {
                ChooseNewAction();
            }
        }

        float[] compositeScores = new float[Enum.GetValues(typeof(COMPOSITE_SCORES)).Length];
        void ChooseNewAction() {
            foreach (COMPOSITE_SCORES i in Enum.GetValues(typeof(COMPOSITE_SCORES))) {
                compositeScores[(int)i] = GetCompositeScore(i);
            }
            // Debug.Log(((COMPOSITE_SCORES)compositeScores.MaxIndex()).ToString());
        }

        float GetCompositeScore(COMPOSITE_SCORES cs) {
            if (cs == COMPOSITE_SCORES.Idle) {
                return doNothingCurve.Evaluate(GetRawScore(RAW_SCORES.KuriLastActionEnded));
            }
            else if (cs == COMPOSITE_SCORES.Move) {
                return movementCurve.Evaluate(GetRawScore(RAW_SCORES.Movement)) - GetRawScore(RAW_SCORES.HumanDoingAction);
            }
            else if (cs == COMPOSITE_SCORES.VirtualISA) {
                return curiosityCurve.Evaluate(GetRawScore(RAW_SCORES.Curiosity)) - GetRawScore(RAW_SCORES.HumanDoingAction);
            }

            throw new NotImplementedException("Composite Score enum not supported");
        }

        // All scores 0 - 1
        float timeSinceNormDenom = 30.0f;
        float GetRawScore(RAW_SCORES sc) {
            if (sc == RAW_SCORES.Curiosity) {
                return humanStateManager.GetCuriosityCDF();
            }
            else if (sc == RAW_SCORES.Movement) {
                return humanStateManager.GetMovementCDF();
            }
            else if (sc == RAW_SCORES.KuriLastActionEnded) {
                return Mathf.Min(kuriManager.TimeLastActionEnded.TimeSince() /
                                timeSinceNormDenom,
                                1.0f);
            }
            else if (sc == RAW_SCORES.KuriLastActionStarted) {
                return Mathf.Min(kuriManager.TimeLastActionStarted.TimeSince() /
                                timeSinceNormDenom,
                                1.0f);
            }
            else if (sc == RAW_SCORES.KuriDoingAction) {
                return kuriManager.kuriController.IsDoingAction ? 1f : 0f;
            }
            else if (sc == RAW_SCORES.HumanDoingAction) {
                return humanStateManager.IsDoingAction ? 1f : 0f;
            }
            else if (sc == RAW_SCORES.HumanLastActionEnded) {
                Mathf.Min(humanStateManager.LastTimeHumanDidAction.TimeSince() /
                                timeSinceNormDenom,
                                1.0f);
            }
            throw new NotImplementedException("Raw Scoring enum not supported");
        }

        public override void ForceHelpfulAction() {
            throw new NotImplementedException();
        }
    }
}
