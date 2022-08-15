using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.UI;

namespace NRISVTE {
    public abstract class ToggleButton : ActionNode {
        protected Button button;
        public bool toggleOn;
        protected abstract void SetButton();
        protected override void OnStart() {
            SetButton();
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            button.interactable = toggleOn;
            return State.Success;
        }
    }
}
