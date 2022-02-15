using UnityEngine;

namespace MoveToCode {
    public abstract class KuriAI : MonoBehaviour {
        // All possible Kuri AIs
        public enum KURIAI {
            RuleBased,
            Utility
        }

        public abstract void Tick();
    }
}
