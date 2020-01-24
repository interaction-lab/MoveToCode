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
            gameObject.layer = 2;
        }
        private void OnTriggerEnter(Collider collision) {
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            // Make sure it is of right type and not already a part of my code
            if (IsSnappableToThisSnapColliderType(collisionCodeBlockSnap?.GetMyCodeBlock())
                && !justEnabled) {
                meshRend.enabled = true;
                InitializeMyCodeBlockSnapIfNull();
                collisionCodeBlockSnap?.AddCollisionSnapCollider(this);
            }

        }
        private void OnTriggerExit(Collider collision) {
            ExitCollisionRoutine();
        }

        public void ExitCollisionRoutine(bool isBeingDisabled = false) {
            justEnabled = true;
            if (gameObject.activeSelf && enabled && !isBeingDisabled) {
                StartCoroutine(HackyFixForEnablingTrigger());
            }
            collisionCodeBlockSnap?.RemoveCollisionSnapCollider(this);
            if (meshRend != null) {
                meshRend.enabled = false;
            }
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
            if (myCodeBlockSnap == null) {
                myCodeBlockSnap = transform.parent.parent.parent.GetComponent<CodeBlockSnap>(); // TODO: this is so jank, need fix for object mesh abstraction on all instructions
            }
        }

        IEnumerator HackyFixForEnablingTrigger() {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            justEnabled = false;
        }

        private void OnEnable() {
            ExitCollisionRoutine();
        }

        private void OnDisable() {
            ExitCollisionRoutine(true);
        }
    }
}