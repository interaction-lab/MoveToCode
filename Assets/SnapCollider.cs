using UnityEngine;

namespace MoveToCode {
    public abstract class SnapCollider : MonoBehaviour {
        protected CodeBlockSnap myCodeBlockSnap, collisionCodeBlockSnap;
        public abstract void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock);

        private void OnTriggerEnter(Collider collision) {
            InitializeMyCodeBlockSnapIfNull();
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            collisionCodeBlockSnap?.SetCollisionSnapCollider(this);
        }
        private void OnTriggerExit(Collider collision) {
            collisionCodeBlockSnap?.SetCollisionSnapCollider(null);
            collisionCodeBlockSnap = null;
        }

        public CodeBlock GetMyCodeBlock() {
            InitializeMyCodeBlockSnapIfNull();
            return myCodeBlockSnap.GetMyCodeBlock();
        }

        protected void InitializeMyCodeBlockSnapIfNull() {
            if (myCodeBlockSnap == null) {
                myCodeBlockSnap = transform.parent.parent.GetComponent<CodeBlockSnap>();
            }
        }
    }
}