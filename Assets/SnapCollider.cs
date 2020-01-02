using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public abstract class SnapCollider : MonoBehaviour {
        protected CodeBlockSnap myCodeBlockSnap, collisionCodeBlockSnap;
        public abstract void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock);
        public abstract bool IsSnappableToThisSnapColliderType(CodeBlock collidedCodeBlock);
        MeshRenderer meshRend;
        bool justEnabled = false;
        private void Awake() {
            meshRend = GetComponent<MeshRenderer>();
            meshRend.enabled = false;
            GetComponent<Collider>().isTrigger = true;
        }
        private void OnTriggerEnter(Collider collision) {
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            // Make sure it is of right type and not already a part of my code
            if (IsSnappableToThisSnapColliderType(collisionCodeBlockSnap?.GetMyCodeBlock())
                && !justEnabled) {
                meshRend.enabled = true;
                InitializeMyCodeBlockSnapIfNull();
                collisionCodeBlockSnap?.SetCollisionSnapCollider(this);
            }

        }
        private void OnTriggerExit(Collider collision) {
            ExitCollisionRoutine();
        }

        public void ExitCollisionRoutine() {
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

        IEnumerator HackyFixForEnablingTrigger() {
            yield return new WaitForFixedUpdate();
            justEnabled = false;
        }

        private void OnEnable() {
            meshRend.enabled = false;
            justEnabled = true;
            StartCoroutine(HackyFixForEnablingTrigger());
        }
    }
}