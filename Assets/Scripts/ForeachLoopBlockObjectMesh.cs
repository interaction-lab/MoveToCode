using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class ForeachLoopBlockObjectMesh : ControlFlowBlockObjectMesh {
        Transform top, argLeft, middle, argRight, side, bot;
        Vector3 origScaleArgLeft;
        Vector3 origScaleArgRight;
        Vector3 origPositionArgLeft;
        Vector3 origPositionArgRight;
        GameObject inTextGameObject;
        TextMeshPro inTextMesh;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            argLeft = transform.GetChild(1);
            middle = transform.GetChild(2);
            argRight = transform.GetChild(3);
            side = transform.GetChild(4);
            bot = transform.GetChild(5);

            origScaleArgRight = argRight.localScale;
            origPositionArgRight = argRight.localPosition;
        }

        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                top.gameObject.AddComponent<MeshOutline>(),
                argLeft.gameObject.AddComponent<MeshOutline>(),
                middle.gameObject.AddComponent<MeshOutline>(),
                argRight.gameObject.AddComponent<MeshOutline>(),
                side.gameObject.AddComponent<MeshOutline>(),
                bot.gameObject.AddComponent<MeshOutline>()
             };
            UpdateText();
        }

        private void UpdateText() {
            if (inTextMesh == null) {
                InstantiateToText();
                RepositionInText();
                StartCoroutine(UpdateTextNextFrame());
            }
            if (inTextMesh == null) {
                inTextMesh = middle.GetChild(0).GetComponent<TextMeshPro>();
            }
            else {
                inTextMesh.SetText("in");
                ForceTextUpdate();
            }
        }

        private void ForceTextUpdate() {
            inTextMesh.enabled = false;
            inTextMesh.enabled = true;
        }

        private void InstantiateToText() {
            inTextGameObject = Instantiate(
                Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), GetMyCodeBlock().transform) as GameObject;
            inTextGameObject.transform.SnapToParent(middle);
        }

        private void RepositionInText() {
            Vector3 tmp = inTextGameObject.transform.localPosition;
            tmp.x = 0;
            inTextGameObject.transform.localPosition = tmp;
        }

        // This is needed to wait for the gameobject to spawn
        private IEnumerator UpdateTextNextFrame() {
            yield return new WaitForEndOfFrame();
            UpdateText();
        }

        public override float GetBlockVerticalSize() {
            return GetSizeOfInsideInstructionChain() + GetSizeOfExitInstructionChain() + top.localScale.y + bot.localScale.y;
        }

        public override float GetBlockHorizontalSize() {
            return side.localScale.x + top.localScale.x + argRight.localScale.x;
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero;
        }

        protected override void ResizeObjectMesh() {
            ResizeSide();
        }

        // private helpers
        private float GetSizeOfInsideInstructionChain() {
            return FindChainSize(GetMyCodeBlock().GetArgumentFromDict(SNAPCOLTYPEDESCRIPTION.Nested));
        }
        private float GetSizeOfExitInstructionChain() {
            return FindChainSize(GetMyCodeBlock().GetArgumentFromDict(SNAPCOLTYPEDESCRIPTION.Next)) + 0.5f;
        }

        private void ResizeSide() {
            float internalSize = GetSizeOfInsideInstructionChain();

            Vector3 scaler = side.localScale;
            scaler.y = internalSize + 1.5f;
            side.localScale = scaler;

            scaler = bot.localPosition;
            scaler.y = -(internalSize + 1f);
            bot.localPosition = scaler;

            scaler = side.localPosition;
            scaler.y = (top.localPosition.y + bot.localPosition.y) / 2.0f;
            side.localPosition = scaler;
        }
    }
}
