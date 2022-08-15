using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    // really should be called say object
    public class SayObjectQuestion : ActionNode {
        DialogueManager _dialogueManager;
        DialogueManager dialogueManager {
            get {
                if (_dialogueManager == null) {
                    _dialogueManager = DialogueManager.instance;
                }
                return _dialogueManager;
            }
        }
        protected override void OnStart() {
            dialogueManager.UpdateQuestionText();
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            // wait for dialogue to end
            if (dialogueManager.state == DialogueManager.State.SayingQuestion) {
                return State.Running;
            }
            return State.Success;
        }
    }
}
