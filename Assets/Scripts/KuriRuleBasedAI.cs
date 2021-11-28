using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriRuleBasedAI : KuriAI {
        KuriManager kuriManager;
        KuriController kuriController;

        private void Awake() {
            kuriManager = KuriManager.instance;   
            kuriController = kuriManager.kuriController; 
        }

        public override void Tick(){
             if (kuriManager.TimeLastActionStarted.TimeSince() < kuriManager.TimeWindow) {
                return;
            }
            float kctS = HumanStateManager.instance.GetKCt();
            if (kctS < kuriManager.robotKC) {
                kuriController.TakeISAAction(); // this should be moved to kuri controller
                //TakeMovementAction();
            }
            else {
                kuriController.DoRandomPositiveAction();
            }
        }
    }
}
