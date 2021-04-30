using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public override void DoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa) {
            Anim.SetTrigger("GotItTrigger");
            return "GotItTrigger";
        }

        public override void TakeMovementAction() {
            return;
        }
    }
}
