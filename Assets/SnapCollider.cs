using UnityEngine;

namespace MoveToCode {
    public abstract class SnapCollider : MonoBehaviour {
        protected CodeBlockSnap myCodeBlockSnap, collisionCodeBlockSnap;
        public abstract void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock);
        MeshRenderer meshRend;
        private void Awake() {
            meshRend = GetComponent<MeshRenderer>();
            meshRend.enabled = false;
        }
        private void OnTriggerEnter(Collider collision) {
            meshRend.enabled = true;
            InitializeMyCodeBlockSnapIfNull();
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            collisionCodeBlockSnap?.SetCollisionSnapCollider(this);
        }
        private void OnTriggerExit(Collider collision) {
            meshRend.enabled = false;
            collisionCodeBlockSnap?.SetCollisionSnapCollider(null);
            collisionCodeBlockSnap = null;
        }

        public CodeBlock GetMyCodeBlock() {
            InitializeMyCodeBlockSnapIfNull();
            return myCodeBlockSnap.GetMyCodeBlock();
        }

        // This is used because of race conditions, need better solution
        void InitializeMyCodeBlockSnapIfNull() {
            if (myCodeBlockSnap == null) {
                myCodeBlockSnap = transform.parent.parent.GetComponent<CodeBlockSnap>();
            }
        }

    }
}