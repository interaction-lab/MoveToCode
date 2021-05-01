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

        PoseStampedPublisher poseStampPub;
        PoseStampedPublisher PoseStampPub {
            get {
                if (poseStampPub == null) {
                    poseStampPub = GetComponent<PoseStampedPublisher>();
                }
                return poseStampPub;
            }
        }

        public override string DoAction(EMOTIONS e) {
            KuriEmoteStringPub.PublishAction(e);
            return e.ToString();
        }

        public override string DoRandomPositiveAction() {
            return KuriEmoteStringPub.PubRandomPositive();
        }

        public override string TakeMovementAction() {
            throw new System.NotImplementedException();
        }

        public override string TurnTowardsUser() {
            throw new System.NotImplementedException();
        }

        public override string DoRandomNegativeAction() {
            return KuriEmoteStringPub.PubRandomNegative();
        }
    }
}
