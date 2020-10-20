using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class ArrayIndexObjectMesh : CodeBlockObjectMesh {
        Transform variable, leftBracket, index, rightBracket;
        Vector3 origScaleVariable, origScaleIndex;
        Vector3 origPosVariable, origPosIndex, origPosLeftBracket, origPosRightBracket;
        GameObject leftBrackCodeBlockTextGameObject, rightBrackCodeBlockTextGameObject;

        TextMeshPro leftBracketTextMesh;
        TextMeshPro rightBracketTextMesh;

        public override void SetUpObject() {
            variable = transform.GetChild(0);
            leftBracket = transform.GetChild(1);
            index = transform.GetChild(2);
            rightBracket = transform.GetChild(3);
            origScaleVariable = variable.localScale;
            origPosVariable = variable.localPosition;
            origScaleIndex = index.localScale;
            origPosIndex = index.localPosition;
            origPosLeftBracket = leftBracket.localPosition;
            origPosRightBracket = rightBracket.localPosition;
        }

        private void UpdateText() {
            if (rightBracketTextMesh == null && leftBracketTextMesh == null) {
                InstantiateBracketText();
                RepositionBracketText();
                RescaleBracketText();
                StartCoroutine(UpdateTextNextFrame());
            }
            if (rightBracketTextMesh == null && leftBracketTextMesh == null) {
                leftBracketTextMesh = leftBracket.GetChild(0).GetComponent<TextMeshPro>();
                rightBracketTextMesh = rightBracket.GetChild(0).GetComponent<TextMeshPro>();
            }
            else {
                leftBracketTextMesh.SetText("[");
                rightBracketTextMesh.SetText("]");
                // Forces Text update
                rightBracketTextMesh.enabled = false;
                rightBracketTextMesh.enabled = true;
                leftBracketTextMesh.enabled = false;
                leftBracketTextMesh.enabled = true;
            }
        }

        private void InstantiateBracketText() {
            leftBrackCodeBlockTextGameObject = Instantiate(
                Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), GetMyCodeBlock().transform) as GameObject;
            rightBrackCodeBlockTextGameObject = Instantiate(
                Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), GetMyCodeBlock().transform) as GameObject;
            leftBrackCodeBlockTextGameObject.transform.SnapToParent(leftBracket);
            rightBrackCodeBlockTextGameObject.transform.SnapToParent(rightBracket);
        }

        private void RepositionBracketText() {
            Vector3 tmpRight = rightBrackCodeBlockTextGameObject.transform.localPosition;
            Vector3 tmpLeft = leftBrackCodeBlockTextGameObject.transform.localPosition;
            tmpRight.x = tmpLeft.x = 0.25f;
            tmpRight.y = tmpLeft.y = -0.25f;
            rightBrackCodeBlockTextGameObject.transform.localPosition = tmpRight;
            leftBrackCodeBlockTextGameObject.transform.localPosition = tmpLeft;
        }

        private void RescaleBracketText() {
            rightBrackCodeBlockTextGameObject.transform.localScale =
                    new Vector3(rightBrackCodeBlockTextGameObject.transform.localScale.x * 2.5f,
                    rightBrackCodeBlockTextGameObject.transform.localScale.y * 1.5f,
                    rightBrackCodeBlockTextGameObject.transform.localScale.z);
            leftBrackCodeBlockTextGameObject.transform.localScale =
                new Vector3(leftBrackCodeBlockTextGameObject.transform.localScale.x * 2.5f,
                leftBrackCodeBlockTextGameObject.transform.localScale.y * 1.5f,
                leftBrackCodeBlockTextGameObject.transform.localScale.z);
        }

        // This is needed to wait for the gameobject to spawn
        private IEnumerator UpdateTextNextFrame() {
            yield return new WaitForEndOfFrame();
            UpdateText();
        }

        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                variable.gameObject.AddComponent<MeshOutline>(),
                leftBracket.gameObject.AddComponent<MeshOutline>(),
                index.gameObject.AddComponent<MeshOutline>(),
                rightBracket.gameObject.AddComponent<MeshOutline>()
                };
            UpdateText();
        }

        public override float GetBlockHorizontalSize() {
            return index.localScale.x + variable.localScale.x + leftBracket.localScale.x + rightBracket.localScale.x;
        }

        public override float GetBlockVerticalSize() {
            return variable.localScale.y;
        }

        public override Vector3 GetCenterPosition() {
            Vector3 leftmostB = variable.localPosition;
            leftmostB.x -= (variable.localScale.x / 2.0f);
            Vector3 rightmostB = rightBracket.localPosition;
            rightmostB.x += (rightBracket.localScale.x / 2.0f);
            return (rightmostB + leftmostB) / 2.0f;
        }

        protected override void ResizeObjectMesh() {
            ResizeVariable();
            RepositionLeftBracket();
            ResizeIndex();
            RepositionRightBracket();
        }

        // private functions
        private void ResizeVariable() {
            Vector3 rescale = origScaleVariable;
            Vector3 reposition = origPosVariable;
            // float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock(string.Array)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            float? horizontalSize = 1; // TODO: fix arrays later
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

        private float GetIndexBlockHorizontalAddition() {
            return (index.localPosition.x - origPosIndex.x) * 2.0f;
        }

        private void RepositionLeftBracket() {
            Vector3 reposition = origPosLeftBracket;
            reposition.x += GetVariableBlockHorizontalAddition();
            leftBracket.localPosition = reposition;
        }

        private void ResizeIndex() {
            Vector3 rescale = origScaleIndex;
            Vector3 reposition = origPosIndex;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock("ArrayElement")?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f + GetVariableBlockHorizontalAddition();
            }
            index.localPosition = reposition;
            index.localScale = rescale;
        }

        private void RepositionRightBracket() {
            Vector3 reposition = origPosRightBracket;
            reposition.x += (GetVariableBlockHorizontalAddition() + GetIndexBlockHorizontalAddition());
            rightBracket.localPosition = reposition;
        }
    }
}
