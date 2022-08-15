using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class ToggleStopButton : ToggleButton {
        StopButtonManager stopButtonManager;
        StopButtonManager StopButtonManager_ {
            get {
                if (stopButtonManager == null) {
                    stopButtonManager = StopButtonManager.instance;
                }
                return stopButtonManager;
            }
        }
        protected override void SetButton() {
            button = StopButtonManager_.StopButton;
        }
    }
}
