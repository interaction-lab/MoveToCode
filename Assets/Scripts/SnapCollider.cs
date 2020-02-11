using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapCollider : MonoBehaviour {
        public int myArgumentPosition = 0;

        List<Type> myCompatibleArgTypes;
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
        }

        // Collision/outline // this needs to be moved to object mesh idk wait is this the snapcolliders?
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

        // Todo: rewire snap colliding
        private CodeBlockSnap GetCollidersCodeBlockSnap(Collider collision) {
            return collision.transform.parent.parent.GetComponent<CodeBlockSnap>();
        }

        private void OnTriggerEnter(Collider collision) {
            collisionCodeBlockSnap = GetCollidersCodeBlockSnap(collision); // TODO: mega hack, need to find codeblock snap of other
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

        public CodeBlock GetMyCodeBlock() {
            return transform.parent.parent.GetComponent<CodeBlockObjectMesh>().GetMyCodeBlock(); // TODO: this is mega hack, clean up when rewriting snap
        }

        CodeBlockSnap GetMyCodeBlockSnap() {
            return GetMyCodeBlock().GetCodeBlockSnap();
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

        public void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            myCodeBlock.SetArgumentBlockAt(collidedCodeBlock, myArgumentPosition, transform.localPosition);
        }

        protected List<Type> GetMyCompatibleArgTypes() {
            if (myCompatibleArgTypes == null) {
                myCompatibleArgTypes = GetMyCodeBlock().GetArgCompatabilityAt(myArgumentPosition);
            }
            return myCompatibleArgTypes;
        }

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