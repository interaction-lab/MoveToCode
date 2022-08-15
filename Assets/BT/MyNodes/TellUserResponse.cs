using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class TellUserResponse : ActionNode {
        DialogueManager dialogueManager;
        public DialogueManager DialogueManager_ {
            get {
                if (dialogueManager == null) {
                    dialogueManager = DialogueManager.instance;
                }
                return dialogueManager;
            }
        }
        protected override void OnStart() {
            DialogueManager_.UpdateResponseText();
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (DialogueManager_.state == DialogueManager.State.SayingResponse) {
                return State.Running;
            }
            return State.Success;
        }
    }
}
