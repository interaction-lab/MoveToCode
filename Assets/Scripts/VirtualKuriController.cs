using UnityEngine;
using System.Collections;

namespace MoveToCode {
    public class VirtualKuriController : KuriController {
        public float ForwardSpeed, BackwardSpeed, TurnSpeed;
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

        public override string DoAnimationAction(EMOTIONS e) {
            string action = e.ToString();
            Anim.SetTrigger(action);
            return action;
        }

        public override string DoRandomNegativeAction() {
            return DoAnimationAction(NegativeEmotions[Random.Range(0, NegativeEmotions.Length)]);
        }

        public override string DoRandomPositiveAction() {
            return DoAnimationAction(PositiveEmotions[Random.Range(0, NegativeEmotions.Length)]);
        }

        public override string TakeMovementAction() {
            //move to user
            Vector3 goal = Camera.main.transform.position;
            goal.y = KuriManager.instance.transform.position.y;
           // StartCoroutine(MoveTo(goal, 0.2f));
            return "moving";
        }

        // IEnumerator MoveFromTo(Vector3 goal, float distThreshold) {
        //     Vector3 curPos = transform.position;
        //     while (Vector3.Distance(curPos, goal) > distThreshold) {
                
        //         yield return null;
        //     }
        // }

        public override void TurnTowardsUser() {
            Mtlau.LookAtUser();
        }


        protected override bool UpdateCurrentActionString() {
            string doingAnim = Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            CurAction = "";
            if (doingAnim != "neutral") {
                CurAction = actionSeperator + doingAnim;
            }
            if (kuriTextManager.IsTalking) {
                CurAction += actionSeperator + kuriTextManager.CurTextCommand.ToString();
            }
            // TODO: Movement when doing the movement actions
            return CurAction != "";
        }
    }
}
