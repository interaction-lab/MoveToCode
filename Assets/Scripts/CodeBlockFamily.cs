using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockFamily : MonoBehaviour {

        public List<GameObject> blocksInFamily;
        MeshRenderer shelfMeshRenderer;

        private void Awake() {
            CodeBlockMenuManager.instance.SetFamily(this);
            shelfMeshRenderer = gameObject.transform.parent.parent.transform.GetChild(0).GetComponent<MeshRenderer>();
            HideFamily();
        }

        private void Start() {
            AddBlocksToFamily();
            InstantiateBlocksInFamily();
        }

        public void ShowFamily() {
            gameObject.SetActive(true);
        }

        public void HideFamily() {
            gameObject.SetActive(false);
        }

        private void AddBlocksToFamily() {
            if (blocksInFamily[0].GetComponent<ConditionalCodeBlock>() != null) {
                for (int i = 1; i < System.Enum.GetNames(typeof(ConditionalCodeBlock.OPERATION)).Length; i++) {
                    //blocksInFamily.Add(blocksInFamily[0].GetComponent<);
                    SetConditionalOperation(blocksInFamily[0], i);
                    GameObject go = Instantiate(blocksInFamily[0]);
                    SetBlockPosition(go, 0, i);
                }
            } else if (blocksInFamily[0].GetComponent<MathOperationCodeBlock>() != null) {
                for (int i = 1; i < System.Enum.GetNames(typeof(MathOperationCodeBlock.OPERATION)).Length; i++) {
                    SetMathOperation(blocksInFamily[0], i);
                    GameObject go = Instantiate(blocksInFamily[0]);
                    SetBlockPosition(go, 0, i);
                }
            }
        }

        public void InstantiateBlocksInFamily() {
            if (blocksInFamily[0].GetComponent<ConditionalCodeBlock>() != null) {
                for (int i = 0; i < System.Enum.GetNames(typeof(ConditionalCodeBlock.OPERATION)).Length; i++) {
                    SetConditionalOperation(blocksInFamily[0], i);
                    GameObject go = Instantiate(blocksInFamily[0]);
                    SetBlockPosition(go, 0, i);
                }
            } else if (blocksInFamily[0].GetComponent<MathOperationCodeBlock>() != null) {
                for (int i = 0; i < System.Enum.GetNames(typeof(MathOperationCodeBlock.OPERATION)).Length; i++) {
                    SetMathOperation(blocksInFamily[0], i);
                    GameObject go = Instantiate(blocksInFamily[0]);
                    SetBlockPosition(go, 0, i);
                }
            } else {
                for (int i = 0; i < blocksInFamily.Count; i++) {
                    GameObject go = Instantiate(blocksInFamily[i]);
                    SetBlockPosition(go, i, i);
                }
            }
        }

        private void SetBlockPosition(GameObject block, int index, int blockNum) {
            block.transform.position = new Vector3(
                    shelfMeshRenderer.bounds.center.x,
                    shelfMeshRenderer.bounds.extents.y - 0.25f - blockNum * shelfMeshRenderer.bounds.size.y / (blocksInFamily.Count + 1),
                    transform.position.z);

            //set codeblocks as blocks in menu
            block.AddComponent<CloneOnDrag>().SetCodeBlockType(blocksInFamily[index]);
            block.GetComponent<CodeBlock>().SetIsMenuBlock(true);
            block.transform.SnapToParent(transform);
            block.transform.localScale = Vector3.one;
        }

        private void SetMathOperation(GameObject block, int index) {
            block.GetComponent<MathOperationCodeBlock>().SetOperation((MathOperationCodeBlock.OPERATION)index);
        }

        private void SetConditionalOperation(GameObject block, int index) {
            block.GetComponent<ConditionalCodeBlock>().SetOperation((ConditionalCodeBlock.OPERATION)index);
        }

        private float GetBlockHeight(GameObject goIn) {
            float height = goIn.GetComponentInChildren<CodeBlockObjectMesh>().GetBlockVerticalSize();
            return height;
        }
    }
}