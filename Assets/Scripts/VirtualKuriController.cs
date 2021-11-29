using UnityEngine;
using System.Collections;
using UnityMovementAI;

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

        FollowPathUnit followPathUnitM;
        FollowPathUnit FollowPathUnitM {
            get {
                if (followPathUnitM == null) {
                    followPathUnitM = GetComponent<FollowPathUnit>();
                }
                return followPathUnitM;
            }
        }

        FollowPath followPathM;
        FollowPath FollowPathM {
            get {
                if (followPathM == null) {
                    followPathM = GetComponent<FollowPath>();
                }
                return followPathM;
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
            StartCoroutine(GoToUser());
            return "moving";
        }

        IEnumerator GoToUser() {
            Vector3 goal = Camera.main.transform.position;
            goal.y = KuriManager.instance.transform.position.y;
            FollowPathUnitM.path = new LinePath(new[] { KuriManager.instance.transform.position, goal });
            FollowPathUnitM.enabled = true;
            yield return null; // wait frame to be enabled
            while (!FollowPathM.IsAtEndOfPath(FollowPathUnitM.path)) {
                LoggingManager.instance.UpdateLogColumn(kuriMovementActionCol, transform.position.ToString());
                yield return null;
            }
            followPathUnitM.enabled = false;
        }

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
