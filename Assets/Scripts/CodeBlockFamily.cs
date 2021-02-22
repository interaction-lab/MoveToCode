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
            AddVariantBlocksToFamily();
            InstantiateBlocksInFamily();
        }

        public void ShowFamily() {
            gameObject.SetActive(true);
        }

        public void HideFamily() {
            gameObject.SetActive(false);
        }

        private void AddVariantBlocksToFamily() {
            if (blocksInFamily[0].GetComponent<ConditionalCodeBlock>() != null) {
                for (int i = 1; i < System.Enum.GetNames(typeof(ConditionalCodeBlock.OPERATION)).Length; i++) {
                    blocksInFamily.Add(blocksInFamily[0]);
                }
            } else if (blocksInFamily[0].GetComponent<MathOperationCodeBlock>() != null) {
                for (int i = 1; i < System.Enum.GetNames(typeof(MathOperationCodeBlock.OPERATION)).Length; i++) {
                    blocksInFamily.Add(blocksInFamily[0]);
                }
            }
        }

        private void InstantiateBlocksInFamily() {
            
            for (int i = 0; i < blocksInFamily.Count; i++) {
                SetMathOperation(blocksInFamily[0], i);
                SetConditionalOperation(blocksInFamily[0], i);
                GameObject go = Instantiate(blocksInFamily[i]);
                SetBlockPosition(go, i);
            }
        }

        private void SetBlockPosition(GameObject block, int index) {
            block.transform.SnapToParent(transform);
            block.transform.localScale = Vector3.one;
            block.transform.localPosition = new Vector3(
                    shelfMeshRenderer.bounds.center.x,
                    shelfMeshRenderer.bounds.extents.y - 0.2f - (index * shelfMeshRenderer.bounds.size.y * 4f) / (blocksInFamily.Count + 0.25f),
                    transform.localPosition.z + 1);

            //set codeblocks as blocks in menu
            block.GetComponent<CloneOnDrag>().SetCodeBlockType(blocksInFamily[index]);
            block.GetComponent<CodeBlock>().SetIsMenuBlock(true);
            
        }

        private void SetMathOperation(GameObject block, int index) {
            block.GetComponent<MathOperationCodeBlock>()?.SetOperation((MathOperationCodeBlock.OPERATION)index);
        }

        private void SetConditionalOperation(GameObject block, int index) {
            block.GetComponent<ConditionalCodeBlock>()?.SetOperation((ConditionalCodeBlock.OPERATION)index);
        }
    }
}