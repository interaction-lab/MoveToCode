using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class CodeBlockSnap : MonoBehaviour {
        CodeBlock myCodeBlock;
        Ray upRay, downRay;
        RaycastHit upHit, downHit;
        public float snapDist = 1.0f;
        bool snapped = false;
        ManipulationHandler i;

        private void Awake() {
            myCodeBlock = GetComponent<CodeBlock>();
            i = GetComponent<ManipulationHandler>();
            //  Interactable b = GetComponent<Interactable>();
            // b.OnClick.AddListener(Idk());
            i.OnManipulationStarted.AddListener(Idk);
        }

        void Idk(ManipulationEventData call) {
            Debug.Log(transform.name);
        }

        private void Update() {
            upRay = new Ray(transform.position, Vector3.up);
            downRay = new Ray(transform.position, Vector3.down);

            if (!snapped && Physics.Raycast(upRay, out upHit, snapDist) && myCodeBlock.IsInstructionCodeBlock()) {
                CodeBlock otherblock = upHit.transform.GetComponent<CodeBlock>();
                if (otherblock != null && otherblock.IsInstructionCodeBlock()) {
                    otherblock.SetNextCodeBlock(myCodeBlock);
                }
            }

        }
    }
}