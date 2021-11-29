﻿using UnityEngine;

namespace MoveToCode {
    public class VirtualKuriController : KuriController {
        Animator anim;
        Animator Anim {
            get {
                if (anim == null) {
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }

        MakeTransformLookAtUser mtlau;
        MakeTransformLookAtUser Mtlau {
            get {
                if (mtlau == null) {
                    mtlau = GetComponent<MakeTransformLookAtUser>();
                }
                return mtlau;
            }
        }

        public override string DoAction(EMOTIONS e) {
            string action = e.ToString();
            Anim.SetTrigger(action);
            return action;
        }

        public override string DoRandomNegativeAction() {
            return DoAction(NegativeEmotions[Random.Range(0, NegativeEmotions.Length)]);
        }

        public override string DoRandomPositiveAction() {
            return DoAction(PositiveEmotions[Random.Range(0, NegativeEmotions.Length)]);
        }

        public override string TakeMovementAction() {
            throw new System.NotImplementedException();
        }

        public override void TurnTowardsUser() {
            Mtlau.LookAtUser();
        }


        protected override bool UpdateCurrentActionString() {
            string doingAnim = Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            CurAction = "";
            if(doingAnim != "neutral"){
                CurAction = actionSeperator + doingAnim;
            }
            if(kuriTextManager.IsTalking){
                CurAction += actionSeperator + kuriTextManager.CurTextCommand.ToString();
            }
            // TODO: Movement when doing the movement actions
            return CurAction != "";
        }
    }
}
