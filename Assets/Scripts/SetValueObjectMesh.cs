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
        GameObject codeBlockTextGameObjectTo, codeBlockTextGameObjectSetVar;

        TextMeshPro textMesh;
        TextMeshPro textMeshSetVar;

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
            if (textMesh == null && textMeshSetVar == null) {
                InstantiateText();
                RepositionText();
                StartCoroutine(UpdateTextNextFrame());
                textMeshSetVar = top.GetChild(1).GetComponent<TextMeshPro>();
                textMesh = middle.GetChild(0).GetComponent<TextMeshPro>();
            }
            else {
                textMesh.SetText("To");
                textMeshSetVar.SetText("Set\nVar");
                textMesh.ForceTextUpdate();
                textMeshSetVar.ForceTextUpdate();
            }
        }

        // This is needed to wait for the gameobject to spawn
        private IEnumerator UpdateTextNextFrame() {
            yield return new WaitForEndOfFrame();
            UpdateText();
        }

        private void InstantiateText() {
            codeBlockTextGameObjectTo = Instantiate(
                        Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), GetMyCodeBlock().transform) as GameObject;
            codeBlockTextGameObjectSetVar = Instantiate(
                    Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), GetMyCodeBlock().transform) as GameObject;
            codeBlockTextGameObjectSetVar.transform.SnapToParent(top);
            codeBlockTextGameObjectTo.transform.SnapToParent(middle);
        }

        private void RepositionText() {
            Vector3 tmpTo = codeBlockTextGameObjectTo.transform.localPosition;
            tmpTo.x = 0;
            codeBlockTextGameObjectTo.transform.localPosition = tmpTo;
            Vector3 tmpSetVar = codeBlockTextGameObjectSetVar.transform.localPosition;
            tmpSetVar.x = 0;
            codeBlockTextGameObjectSetVar.transform.localPosition = tmpSetVar;
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
            return top.localScale.y + FindChainSize(GetMyCodeBlock().GetArgumentFromDict("Next"));
        }

        public override Vector3 GetCenterPosition() {
            return new Vector3(-0.5f, 0, 0f);
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
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock("Variable")?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
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
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock("Value")?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
            }
            reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f + GetVariableBlockHorizontalAddition();
            valueTo.localPosition = reposition;
            valueTo.localScale = rescale;
        }
    }
}
