using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class SnapCollider : MonoBehaviour {
        protected CodeBlockSnap myCodeBlockSnap, collisionCodeBlockSnap;
        public abstract void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock);
        Material m;
        MeshRenderer meshRend;
        bool justEnabled = false;
        private void Awake() {
            meshRend = GetComponent<MeshRenderer>();
            meshRend.enabled = false;
            GetComponent<Collider>().isTrigger = true;
            gameObject.layer = 2;
            CodeBlockManager.instance.RegisterSnapCollider(this);
            gameObject.AddComponent(typeof(MeshOutline));
            m = Resources.Load<Material>("Materials/OutlineSnapCollider") as Material;
            Debug.Log(m);
            GetComponent<MeshOutline>().OutlineMaterial = m;
            gameObject.SetActive(false);
        }

        public IARGSNAPTYPES[] compatibleArgs;
        protected List<Type> myCompatibleArgTypes;

        public enum IARGSNAPTYPES {
            Any,
            Instruction,
            ConditionalInstruction,
            IDataType,
            INumberDataType,
            NULL,
        }
        static Dictionary<IARGSNAPTYPES, Type> argCompatibleTypeLookUp;
        public Dictionary<IARGSNAPTYPES, Type> GetArgCompatibleTypeLookUp() {
            if (argCompatibleTypeLookUp == null) {
                argCompatibleTypeLookUp = new Dictionary<IARGSNAPTYPES, Type>();
                argCompatibleTypeLookUp.Add(IARGSNAPTYPES.Any, typeof(IArgument));
                argCompatibleTypeLookUp.Add(IARGSNAPTYPES.Instruction, typeof(Instruction));
                argCompatibleTypeLookUp.Add(IARGSNAPTYPES.ConditionalInstruction, typeof(ConditionalInstruction));
                argCompatibleTypeLookUp.Add(IARGSNAPTYPES.IDataType, typeof(IDataType));
                argCompatibleTypeLookUp.Add(IARGSNAPTYPES.INumberDataType, typeof(INumberDataType));
                argCompatibleTypeLookUp.Add(IARGSNAPTYPES.NULL, null);
            }
            return argCompatibleTypeLookUp;
        }

        public List<Type> GetMyCompatibleArgTypes() {
            if (myCompatibleArgTypes == null) {
                myCompatibleArgTypes = new List<Type>();
                foreach (IARGSNAPTYPES iastp in compatibleArgs) {
                    myCompatibleArgTypes.Add(GetArgCompatibleTypeLookUp()[iastp]);
                }
            }
            return myCompatibleArgTypes;
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

        public bool HasCompatibleType(IArgument argIn) {
            Type typeToTry = argIn as Variable != null ?
                            (argIn as Variable).GetMyData().GetType() :
                            argIn?.GetType();
            return CheckArgCompatibleType(typeToTry);
        }



        private void OnTriggerEnter(Collider collision) {
            collisionCodeBlockSnap = collision.transform.GetComponent<CodeBlockSnap>();
            // Make sure it is of right type and not already a part of my code
            if (!justEnabled) {
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
            meshRend.enabled = true;
        }


        private void OnDisable() {
            ExitCollisionRoutine(true);
            meshRend.enabled = false;
        }

        private void OnDestroy() {
            if (CodeBlockManager.instance && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterSnapCollider(this);
            }
        }
    }
}