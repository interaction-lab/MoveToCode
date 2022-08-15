using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class ToggleStartButton : ToggleButton {
        StartButtonManager startButtonManager;
        StartButtonManager StartButtonManager_ {
            get {
                if (startButtonManager == null) {
                    startButtonManager = StartButtonManager.instance;
                }
                return startButtonManager;
            }
        }
        protected override void SetButton() {
            button = StartButtonManager_.StartButton;
        }
    }
}
