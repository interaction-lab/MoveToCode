using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class SetValueObjectMesh : CodeBlockObjectMesh {
        Transform top, variable, middle, valueTo;
        Vector3 origScaleVariable, origScaleValueTo;
        Vector3 origPosVariable, origPosValueTo, origPosMiddle;

        TextMeshPro textMesh;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            variable = transform.GetChild(1);
            middle = transform.GetChild(2);
            valueTo = transform.GetChild(3);
            origScaleVariable = variable.localScale;
            origPosVariable = variable.localPosition;
            origScaleValueTo = valueTo.localScale;
            origPosValueTo = valueTo.localPosition;
            origPosMiddle = middle.localPosition;
        }

        // Jank fix for text pos, maybe move it to it's codeblock?
        private void UpdateText() {
            if (textMesh == null) {
                textMesh = GetComponentInChildren<TextMeshPro>(true);
            }
            if (textMesh == null) {
                GameObject codeBlockTextGameObject = Instantiate(
                    Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), GetMyCodeBlock().transform) as GameObject;
                codeBlockTextGameObject.transform.SnapToParent(middle);
                Vector3 tmp = codeBlockTextGameObject.transform.localPosition;
                tmp.x = 0;
                codeBlockTextGameObject.transform.localPosition = tmp;
                StartCoroutine(UpdateTextNextFrame());
            }
            else {
                textMesh.SetText("To");
                // Forces Text update
                textMesh.enabled = false;
                textMesh.enabled = true;
            }
        }

        // This is needed to wait for the gameobject to spawn
        private IEnumerator UpdateTextNextFrame() {
            yield return new WaitForEndOfFrame();
            UpdateText();
        }

        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                top.gameObject.AddComponent<MeshOutline>(),
                variable.gameObject.AddComponent<MeshOutline>(),
                middle.gameObject.AddComponent<MeshOutline>(),
                valueTo.gameObject.AddComponent<MeshOutline>()
                };
            UpdateText();
        }

        public override float GetBlockHorizontalSize() {
            return top.localScale.x + variable.localScale.x + middle.localScale.x + valueTo.localScale.x;
        }

        public override float GetBlockVerticalSize() {
            return top.localScale.y + FindChainSize(GetMyCodeBlock().GetArgumentFromDict(IARG.Next));
        }

        public override Vector3 GetCenterPosition() {
            return new Vector3(0.25f, 0, 0f);
        }

        protected override void ResizeObjectMesh() {
            ResizeVariable();
            RepositionMiddle();
            ResizeValueTo();
        }

        // private funcitons
        private void ResizeVariable() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleVariable;
            Vector3 reposition = origPosVariable;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock(IARG.Variable)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f;
            }
            variable.localPosition = reposition;
            variable.localScale = rescale;
        }

        private float GetVariableBlockHorizontalAddition() {
            return (variable.localPosition.x - origPosVariable.x) * 2.0f;
        }

        private void RepositionMiddle() {
            Vector3 reposition = origPosMiddle;
            reposition.x += GetVariableBlockHorizontalAddition();
            middle.localPosition = reposition;
        }

        private void ResizeValueTo() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleValueTo;
            Vector3 reposition = origPosValueTo;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock(IARG.Value)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
            }
            reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f + GetVariableBlockHorizontalAddition();
            valueTo.localPosition = reposition;
            valueTo.localScale = rescale;
        }
    }
}
