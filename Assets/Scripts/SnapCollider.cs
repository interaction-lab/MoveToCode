using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapCollider : MonoBehaviour {
        public int myArgumentPosition = 0; //only used for Arrays
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
            return transform.parent.parent?.GetComponent<CodeBlockObjectMesh>().GetMyCodeBlock(); // TODO: this is mega hack, clean up when rewriting snap
        }

        CodeBlockSnap GetMyCodeBlockSnap() {
            return GetMyCodeBlock()?.GetCodeBlockSnap();
        }

        // TODO: humanDidIt is such a hack
        public void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock, bool humanDidIt=true) {
            Transform parentTransform = transform.parent;
            if (myCodeBlock.GetType() == typeof(ArrayCodeBlock)) //Array codeblocks are the only special exception
                myCodeBlock.SetArrayArg(myArgumentPosition, collidedCodeBlock, humanDidIt);
            else //myArgumentPosition is always 0 otherwise which is IArg.Next
                myCodeBlock.SetArg(IARG.Next, collidedCodeBlock, humanDidIt); 

            Vector3 centerPos = collidedCodeBlock.GetCodeBlockObjectMesh().GetCenterPosition();
            centerPos.x = centerPos.x / parentTransform.localScale.x;
            collidedCodeBlock.transform.SnapToParent(parentTransform, snapPosition - centerPos);
        }

        protected HashSet<Type> GetMyCompatibleArgTypes() {
            if (myCompatibleArgTypes == null) {
                myCompatibleArgTypes = GetMyCodeBlock().GetArgCompatibility(IARG.Next); //TODO: its not just Next...
            } else if (GetMyCodeBlock().GetType() == typeof(ArrayCodeBlock)) { //first input sets array type, could later be generalized for all data structures?
                myCompatibleArgTypes = GetMyCodeBlock().GetArgCompatibility(IARG.ArrayElement);
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


        private void OnEnable() {
            meshRend.enabled = true;
            //CodeBlockManager.instance.RegisterSnapCollider(this);
        }

        private void OnDisable() {
            meshRend.enabled = false;
        }

        private void OnDestroy() {
            if (CodeBlockManager.instance != null && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterSnapCollider(this);
            }
        }

        public void SetMyArgumentPosition(int position) {
            myArgumentPosition = position;
        }
    }
}