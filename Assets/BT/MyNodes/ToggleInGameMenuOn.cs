using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class ToggleInGameMenuOn : ActionNode {
        OptionsButtonManager optionsButtonManager;
        OptionsButtonManager OptionsButtonManager_ {
            get {
                if (optionsButtonManager == null) {
                    optionsButtonManager = OptionsButtonManager.instance;
                }
                return optionsButtonManager;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            OptionsButtonManager_.ToggleStop(true);
            return State.Success;
        }
    }
}
