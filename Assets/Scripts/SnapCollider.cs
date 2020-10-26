using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class SnapCollider : MonoBehaviour {
        public CodeBlock MyCodeBlock { get { return transform.parent.parent?.GetComponent<CodeBlockObjectMesh>().GetMyCodeBlock(); } }
        public CodeBlock MyCodeBlockArg { get { return transform.parent.GetComponentInChildren<CodeBlock>(); } }

        static Material OutlineMaterial { get; set; }
        public MeshOutline MyMeshOutline { get; set; }
        MeshRenderer MeshRend { get; set; }

        public abstract Vector3 SnapPosition {get;}

        public abstract HashSet<Type> CompatibleArgTypes { get; }

        CodeBlockSnap collisionCodeBlockSnap;

        void Awake() {
            if (OutlineMaterial == null) {
                OutlineMaterial = Resources.Load<Material>(ResourcePathConstants.OutlineSnapColliderMaterial);
            }
            MyMeshOutline = gameObject.AddComponent(typeof(MeshOutline)) as MeshOutline;
            MyMeshOutline.OutlineMaterial = OutlineMaterial;
            MeshRend = GetComponent<MeshRenderer>();
            MeshRend.enabled = false;
            GetComponent<Collider>().isTrigger = true;
            gameObject.layer = 2;
            CodeBlockManager.instance.RegisterSnapCollider(this);
            gameObject.SetActive(false);
            RegisterToSnapColliderGroup();
            CodeBlockManager.instance.RegisterSnapCollider(this);
        }

        protected abstract void RegisterToSnapColliderGroup();

        private CodeBlockSnap GetCollidersCodeBlockSnap(Collider collision) {
            return collision.transform.parent.parent.GetComponent<CodeBlockSnap>();
        }

        // TODO: humanDidIt is such a hack
        // TODO: fix for arrays
        public void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock, bool humanDidIt = true) {
            SetMyCodeBlockArg(collidedCodeBlock);
        }

        public void SetMyCodeBlockArg(CodeBlock collidedCodeBlock) {
            RemoveCurrentBlockArg();
            if (collidedCodeBlock != null) {
                AddNewCodeBlockArg(collidedCodeBlock);
            }
        }

        private void RemoveCurrentBlockArg() {
            CodeBlock curArg = MyCodeBlockArg;
            if (curArg != null) {
                AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.popAudioClip);
                if (CodeBlockSnap.lastDraggedCBS != curArg) {
                    curArg.transform.localPosition = curArg.transform.localPosition + new Vector3(0.25f, 1.1f, 1.25f);
                }
                curArg.transform.SnapToCodeBlockManager();
                curArg.GetCodeBlockObjectMesh().ResizeChain();
                // TODO: probably needs a log
            }
        }

        private void AddNewCodeBlockArg(CodeBlock collidedCodeBlock) {
            SnapToParentCenter(collidedCodeBlock, transform.parent);
            AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.snapAudioClip);
            MyCodeBlock.GetCodeBlockObjectMesh().ResizeChain();
        }

        public bool HasCodeBlockArgAttached() {
            return MyCodeBlockArg != null;
        }

        private void SnapToParentCenter(CodeBlock collidedCodeBlock, Transform parentTransform) {
            Vector3 centerPos = collidedCodeBlock.GetCodeBlockObjectMesh().GetCenterPosition();
            centerPos.x = centerPos.x / parentTransform.localScale.x;
            collidedCodeBlock.transform.SnapToParent(parentTransform, SnapPosition - centerPos);
        }

        public bool HasCompatibleType(IArgument argIn) {
            if (argIn as Variable != null) {
                if (CheckArgCompatibleType((argIn as Variable).GetMyData().GetType())) {
                    return true;
                }
            }
            return CheckArgCompatibleType(argIn?.GetType());
        }

        private bool CheckArgCompatibleType(Type argTypeIn) {
            if (CompatibleArgTypes.Count == 0) {
                return true;
            }
            foreach (Type T in CompatibleArgTypes) {
                if (T == null && argTypeIn == null ||
                    argTypeIn.IsAssignableFrom(T) ||
                    T.IsAssignableFrom(argTypeIn)) {
                    return true;
                }
            }
            return false;
        }

        private void OnTriggerEnter(Collider collision) {
            collisionCodeBlockSnap = GetCollidersCodeBlockSnap(collision);
            if (collisionCodeBlockSnap == CodeBlockSnap.currentlyDraggingCBS) {
                collisionCodeBlockSnap?.AddSnapColliderInContact(this);
            }
        }

        private void OnTriggerExit(Collider collision) {
            collisionCodeBlockSnap = GetCollidersCodeBlockSnap(collision);
            if (collisionCodeBlockSnap == CodeBlockSnap.currentlyDraggingCBS) {
                collisionCodeBlockSnap?.RemoveAsCurSnapColliderInContact(this);
            }
        }

        private void OnEnable() {
            MeshRend.enabled = true;
        }

        private void OnDisable() {
            MeshRend.enabled = false;
        }

        private void OnDestroy() {
            if (CodeBlockManager.instance != null && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterSnapCollider(this);
            }
        }

    }
}