using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapCollider : MonoBehaviour {
        CodeBlock myCodeBlockArg;

        public SNAPCOLTYPEDESCRIPTION mySnapColTypeDescription;
        public Vector3 snapPosition;

        HashSet<Type> myCompatibleArgTypes;
        CodeBlockSnap collisionCodeBlockSnap;

        static Material outlineMaterial;
        MeshRenderer meshRend;
        MeshOutline meshOutline;

        private void Awake() {
            meshRend = GetComponent<MeshRenderer>();
            meshRend.enabled = false;
            GetComponent<Collider>().isTrigger = true;
            gameObject.layer = 2;
            CodeBlockManager.instance.RegisterSnapCollider(this);
            gameObject.SetActive(false);
            GetMyCodeBlockSnap();
            RegisterWithMyCodeBlockArgDict();
        }

        private void RegisterWithMyCodeBlockArgDict() {
            GetMyCodeBlock().GetMyIArgument().RegisterSnapCollider(mySnapColTypeDescription, this);
        }

        public MeshOutline GetMeshOutline() {
            if (outlineMaterial == null) {
                outlineMaterial = Resources.Load<Material>(ResourcePathConstants.OutlineSnapColliderMaterial) as Material;
            }
            if (meshOutline == null) {
                meshOutline = gameObject.AddComponent(typeof(MeshOutline)) as MeshOutline;
                meshOutline.OutlineMaterial = outlineMaterial;
            }
            return meshOutline;
        }

        private CodeBlockSnap GetCollidersCodeBlockSnap(Collider collision) {
            return collision.transform.parent.parent.GetComponent<CodeBlockSnap>();
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

        internal SNAPCOLTYPEDESCRIPTION GetIArgType() {
            return mySnapColTypeDescription;
        }

        public CodeBlock GetMyCodeBlock() {
            return transform.parent.parent?.GetComponent<CodeBlockObjectMesh>().GetMyCodeBlock();
        }

        CodeBlockSnap GetMyCodeBlockSnap() {
            return GetMyCodeBlock()?.GetCodeBlockSnap();
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
            CodeBlock curArg = GetMyCodeBlockArg();
            if (curArg != null) {
                AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.popAudioClip);
                if (CodeBlockSnap.lastDraggedCBS != curArg) {
                    curArg.transform.localPosition = curArg.transform.localPosition + new Vector3(0.25f, 1.1f, 1.25f);
                }
                transform.SnapToCodeBlockManager();
                curArg.GetCodeBlockObjectMesh().ResizeChain();
                curArg.transform.SnapToCodeBlockManager();
                myCodeBlockArg = null;
                // TODO: probably needs a log
            }
        }

        private void AddNewCodeBlockArg(CodeBlock collidedCodeBlock) {
            myCodeBlockArg = collidedCodeBlock;
            SnapToParentCenter(collidedCodeBlock, transform.parent);
            AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.snapAudioClip);
            GetMyCodeBlock().GetCodeBlockObjectMesh().ResizeChain();
        }

        public CodeBlock GetMyCodeBlockArg() {
            return myCodeBlockArg;
        }

        public bool HasCodeBlockArgAttached() {
            return myCodeBlockArg != null;
        }

        private void SnapToParentCenter(CodeBlock collidedCodeBlock, Transform parentTransform) {
            Vector3 centerPos = collidedCodeBlock.GetCodeBlockObjectMesh().GetCenterPosition();
            centerPos.x = centerPos.x / parentTransform.localScale.x;
            collidedCodeBlock.transform.SnapToParent(parentTransform, snapPosition - centerPos);
        }

        protected HashSet<Type> GetMyCompatibleArgTypes() {
            if (myCompatibleArgTypes == null) {
                myCompatibleArgTypes = GetMyCodeBlock().GetArgCompatibility(mySnapColTypeDescription);
            }
            return myCompatibleArgTypes;
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
            if (GetMyCompatibleArgTypes() == null || GetMyCompatibleArgTypes().Count == 0) return true;
            foreach (Type T in GetMyCompatibleArgTypes()) {
                if (T == null && argTypeIn == null ||
                    argTypeIn.IsAssignableFrom(T) ||
                    T.IsAssignableFrom(argTypeIn)) {
                    return true;
                }
            }
            return false;
        }


        private void OnEnable() {
            meshRend.enabled = true;
            CodeBlockManager.instance.RegisterSnapCollider(this);
        }

        private void OnDisable() {
            meshRend.enabled = false;
        }

        private void OnDestroy() {
            if (CodeBlockManager.instance != null && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterSnapCollider(this);
            }
        }

    }
}