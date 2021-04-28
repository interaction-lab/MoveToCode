using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public abstract class SnapCollider : MonoBehaviour {
        CodeBlock myCodeBlock;
        public CodeBlock MyCodeBlock {
            get {
                if (myCodeBlock == null) {
                    myCodeBlock = transform.parent.parent?.GetComponent<CodeBlockObjectMesh>().GetMyCodeBlock();
                }
                return myCodeBlock;
            }
        }
        public CodeBlock MyCodeBlockArg {
            get {
                return transform.parent.GetComponentInChildrenOnlyDepthOne<CodeBlock>();
            }
        }
        static Material OutlineMaterial { get; set; }
        public MeshOutline MyMeshOutline { get; set; }
        MeshRenderer MeshRend { get; set; }
        public abstract Vector3 SnapPosition { get; }
        public abstract HashSet<Type> CompatibleArgTypes { get; }

        CodeBlockSnap collisionCodeBlockSnap;

        void Awake() {
            if (OutlineMaterial == null) {
                OutlineMaterial = Resources.Load<Material>(ResourcePathConstants.OutlineSnapColliderMaterial);
            }
            MyMeshOutline = gameObject.AddComponent(typeof(MeshOutline)) as MeshOutline;
            MyMeshOutline.OutlineMaterial = OutlineMaterial;
            MyMeshOutline.enabled = false;
            MeshRend = GetComponent<MeshRenderer>();
            MeshRend.enabled = false;
            GetComponent<Collider>().isTrigger = true;
            gameObject.layer = 2;
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
        public void DoSnapAction(CodeBlock collidedCodeBlock, bool humanDidIt = true) {
            SetCodeBlockArg(collidedCodeBlock);
        }

        public void SetCodeBlockArg(CodeBlock collidedCodeBlock) {
            RemoveCurrentBlockArg();
            if (collidedCodeBlock != null) {
                AddNewCodeBlockArg(collidedCodeBlock);
            }
        }

        private void RemoveCurrentBlockArg() {
            if (MyCodeBlockArg != null) {
                CodeBlock tmpargBlock = MyCodeBlockArg;
                if (MyCodeBlockArg.GetCodeBlockSnap() != CodeBlockSnap.CurrentlyDraggingCodeBlockSnap) {
                    MyCodeBlockArg.transform.localPosition = MyCodeBlockArg.transform.localPosition + new Vector3(0.25f, 1.1f, 1.25f);
                }
                tmpargBlock.transform.SnapToCodeBlockManager();
                tmpargBlock.GetCodeBlockObjectMesh().ResizeChain();
                AudioManager.instance.PlaySoundAtObject(MyCodeBlock.transform, AudioManager.popAudioClip);
                // TODO: probably needs a log
            }
        }

        private void AddNewCodeBlockArg(CodeBlock collidedCodeBlock) {
            SnapToParentCenter(collidedCodeBlock, transform.parent);
            AudioManager.instance.PlaySoundAtObject(MyCodeBlock.transform, AudioManager.snapAudioClip);
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
            Debug.Log(CodeBlockSnap.CurrentlyDraggingCodeBlockSnap);
            if (collisionCodeBlockSnap == CodeBlockSnap.CurrentlyDraggingCodeBlockSnap) {
                collisionCodeBlockSnap?.AddSnapColliderInContact(this);
            }
        }

        private void OnTriggerExit(Collider collision) {
            collisionCodeBlockSnap = GetCollidersCodeBlockSnap(collision);
            if (collisionCodeBlockSnap == CodeBlockSnap.CurrentlyDraggingCodeBlockSnap) {
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