using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class PhysicalKuriController : KuriController {

        KuriEmoteStringPublisher kuriEmoteStringPub;
        KuriEmoteStringPublisher KuriEmoteStringPub {
            get {
                if (kuriEmoteStringPub == null) {
                    kuriEmoteStringPub = GetComponent<KuriEmoteStringPublisher>();
                }
                return kuriEmoteStringPub;
            }
        }

        public override string DoAction(EMOTIONS e) {
            KuriEmoteStringPub.PublishAction(e);
            return e.ToString();
        }

        public override string DoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa) {
            return "";
        }

        public override void DoRandomPositiveAction() {
            throw new System.NotImplementedException();
        }

        public override void TakeMovementAction() {
            throw new System.NotImplementedException();
        }
    }
}
