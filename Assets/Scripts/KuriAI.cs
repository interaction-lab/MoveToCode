using UnityEngine;

namespace MoveToCode {
    public abstract class KuriAI : MonoBehaviour {
        // All possible Kuri AIs
        public enum KURIAI {
            RuleBased = 0,
            Utility = 1,
            None = 2
        }
        public abstract void Tick();
        public abstract void ForceHelpfulAction();
    }
}
