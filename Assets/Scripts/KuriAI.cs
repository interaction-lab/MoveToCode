using System;
using UnityEngine;

namespace MoveToCode {
    public abstract class KuriAI : MonoBehaviour {
        // All possible Kuri AIs
        public enum KURIAI {
            None = 0,
            RuleBased = 1,
            Utility = 2,
            BehaviorTreeRand = 3
        }
        public abstract void Tick();
        public abstract void ForceHelpfulAction();
    }
}
