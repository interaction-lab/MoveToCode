using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    /// <summary>
    /// Class that holds and handles a `CodeBlock`'s `SnapCollider`s. It is mainly used to turn this `CodeBlock`'s `SnapCollider`s on and off when the user grabs a the `CodeBlock` or lets it go.
    /// </summary>
    public class SnapColliderGroup : MonoBehaviour {

        /// <summary>
        /// Pointer to this `SnapColliderGroup` `CodeBlock`
        /// </summary>
        public CodeBlock MyCodeBlock
        {
            get { return transform.parent.GetComponent<CodeBlock>(); }
        }

        /// <summary>
        /// Set of all this `CodeBlock`'s `SnapCollider`s. Key's are a `KeyValuePair` of <`SnapCollider` Types (e.g. `SnapColliderNext`.), `int` for indexes (e.g. for array indexes)>
        /// </summary>
        public Dictionary<KeyValuePair<Type, int>, SnapCollider> SnapColliderSet { get; set; } = new Dictionary<KeyValuePair<Type, int>, SnapCollider>();

        /// <summary>
        /// Registers the `SnapCollider` of this `CodeBlock` to be enabled/disabled when `MyCodeBlock` is grabbed.
        /// </summary>
        /// <param name="snapCollider"></param>
        public void RegisterSnapCollider(KeyValuePair<Type, int> scKeyType, SnapCollider snapCollider){
            SnapColliderSet[scKeyType] = snapCollider;
        }

        /// <summary>
        /// Disables this CodeBlock's SnapColliders and recurssively does the tree of snap colliders
        /// </summary>
        public void DisableAllCollidersAndChildrenColliders() {
            SetCollidersAndChildrenState(false);
        }

        /// <summary>
        /// Enables this CodeBlock's SnapColliders and recurssively does the tree of snap colliders
        /// </summary>
        public void EnableAllCollidersAndChildrenColliders() {
            SetCollidersAndChildrenState(true);
        }

        /// <summary>
        /// Recurrsively sets all my `SnapColliderSet` and the children `CodeBlock`'s `SnapColliderGroup` off/on.
        /// </summary>
        /// <param name="desiredActiveState">Desired state of the `SnapCollider`s</param>
        private void SetCollidersAndChildrenState(bool desiredActiveState) {
            foreach (KeyValuePair<KeyValuePair<Type,int>, SnapCollider> sc in SnapColliderSet) {
                sc.Value.gameObject.SetActive(desiredActiveState);
                if (sc.Value.HasCodeBlockArgAttached()) {
                    (desiredActiveState ?
                    new Action(sc.Value.MyCodeBlockArg.GetSnapColliderGroup().EnableAllCollidersAndChildrenColliders) :
                              sc.Value.MyCodeBlockArg.GetSnapColliderGroup().DisableAllCollidersAndChildrenColliders)();
                }
            }
        }
    }
}