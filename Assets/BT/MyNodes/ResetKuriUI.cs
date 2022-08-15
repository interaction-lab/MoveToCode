using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE{
    public class ResetKuriUI : ActionNode
    {
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
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            dialogueManager.ResetAllUI();
            return State.Success;
        }
    }
}
