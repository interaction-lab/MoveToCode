using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapCollider : MonoBehaviour {
        protected CodeBlockSnap myCodeBlockSnap, collisionCodeBlockSnap;

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
        }

        // Collision/outline
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

        public void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {

        }

        // still need to do a backup for double collision for this
        private void OnTriggerEnter(Collider collision) {
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            if (collisionCodeBlockSnap == CodeBlockSnap.currentlyDraggingCBS) {
                collisionCodeBlockSnap?.AddSnapColliderInContact(this);
            }
        }

        private void OnTriggerExit(Collider collision) {
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            if (collisionCodeBlockSnap == CodeBlockSnap.currentlyDraggingCBS) {
                collisionCodeBlockSnap?.RemoveAsCurSnapColliderInContact(this);
            }
        }

        public CodeBlock GetMyCodeBlock() {
            return GetMyCodeBlockSnap().GetMyCodeBlock();
        }

        CodeBlockSnap GetMyCodeBlockSnap() {
            if (myCodeBlockSnap == null) {
                myCodeBlockSnap = transform.parent.parent.GetComponent<CodeBlockSnap>();
            }
            if (myCodeBlockSnap == null) {
                myCodeBlockSnap = transform.parent.parent.parent.GetComponent<CodeBlockSnap>(); // TODO: this is so jank, need fix for object mesh abstraction on all instructions
            }
            return myCodeBlockSnap;
        }

        private void OnEnable() {
            meshRend.enabled = true;
        }

        private void OnDisable() {
            meshRend.enabled = false;
        }

        private void OnDestroy() {
            if (CodeBlockManager.instance && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterSnapCollider(this);
            }
        }

        // Compatability
        // Now moved to instruction level
        // It is a list but most every arg allows only 1 type
        protected abstract List<Type> GetMyCompatibleArgTypes();
        protected List<Type> myCompatibleArgTypes;

        public bool HasCompatibleType(IArgument argIn) {
            Type typeToTry = argIn as Variable != null ?
                            (argIn as Variable).GetMyData().GetType() :
                            argIn?.GetType();
            return CheckArgCompatibleType(typeToTry);
        }

        private bool CheckArgCompatibleType(Type argTypeIn) {
            foreach (Type T in GetMyCompatibleArgTypes()) {
                if (T == null) {
                    if (argTypeIn == null) {
                        return true;
                    }
                }
                else if (argTypeIn.IsAssignableFrom(T) || T.IsAssignableFrom(argTypeIn)) {
                    return true;
                }
            }
            return false;
        }
    }
}