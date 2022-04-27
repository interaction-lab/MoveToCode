using UnityEngine;

namespace MoveToCode {
    public abstract class KuriAI : MonoBehaviour {
        // All possible Kuri AIs
        public enum KURIAI {
            RuleBased,
            Utility,
            None
        }
        public abstract void Tick();
    }
}
