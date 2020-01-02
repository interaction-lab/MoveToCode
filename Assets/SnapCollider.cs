using UnityEngine;

namespace MoveToCode {
    public abstract class SnapCollider : MonoBehaviour {
        protected CodeBlockSnap myCodeBlockSnap, collisionCodeBlockSnap;
        public abstract void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock);
        public abstract bool IsSnappableToThisSnapColliderType(CodeBlock collidedCodeBlock);
        MeshRenderer meshRend;
        private void Awake() {
            meshRend = GetComponent<MeshRenderer>();
            meshRend.enabled = false;
            GetComponent<Collider>().isTrigger = true;
        }
        private void OnTriggerEnter(Collider collision) {
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            if (IsSnappableToThisSnapColliderType(collisionCodeBlockSnap?.GetMyCodeBlock())) {
                meshRend.enabled = true;
                InitializeMyCodeBlockSnapIfNull();
                collisionCodeBlockSnap?.SetCollisionSnapCollider(this);
            }

        }
        private void OnTriggerExit(Collider collision) {
            if (IsSnappableToThisSnapColliderType(collisionCodeBlockSnap?.GetMyCodeBlock())) {
                meshRend.enabled = false;
                collisionCodeBlockSnap?.SetCollisionSnapCollider(null);
                collisionCodeBlockSnap = null;
            }
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