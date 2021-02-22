using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode {
    public class CloneOnDrag : MonoBehaviour {
        ManipulationHandler manipulationHandler;
        Vector3 startingPosition;
        GameObject clone;
        GameObject codeBlockType;
        bool blockStillInMenu;

        private void Start() {
            startingPosition = transform.position;
        }

        private void OnEnable() {
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(StartedMotion);
            manipulationHandler.OnManipulationEnded.AddListener(StoppedMotion);
        }

        public void SetCodeBlockType(GameObject cb) {
            codeBlockType = cb;
        }

        public void SetBlockStillInMenu(bool option) {
            blockStillInMenu = option;
        }

        private void StoppedMotion(ManipulationEventData arg0) {
            //deactivate block if still on shelf/placed back on shelf
            if(blockStillInMenu) {
                Shelf.instance.DisableShelfOutline();
                gameObject.SetActive(false);
            }
        }

        private void StartedMotion(ManipulationEventData arg0) {
            if (transform.GetComponent<CodeBlock>().GetIsMenuBlock()) {
                transform.GetComponent<CodeBlock>().SetIsMenuBlock(false);
                //cannot directly clone gameobject because CodeBlock components are attached after instantiation
                CopyOperationOntoClonePrefab();
                clone = InstantiateBlock(codeBlockType, startingPosition);
                clone.GetComponent<CodeBlock>().SetIsMenuBlock(true);
                transform.SnapToCodeBlockManager();
            }
        }

        private GameObject InstantiateBlock(GameObject block, Vector3 spawnPos) {
            GameObject go = Instantiate(block, spawnPos, Quaternion.identity);
            go.GetComponent<CloneOnDrag>().SetCodeBlockType(codeBlockType);
            go.transform.SetParent(transform.parent);
            go.transform.localScale = Vector3.one;
            return go;
        }

        private void CopyMathOperation(GameObject block, MathOperationCodeBlock.OPERATION op) {
            block.GetComponent<MathOperationCodeBlock>()?.SetOperation(op);
        }

        private void CopyConditionalOperation(GameObject block, ConditionalCodeBlock.OPERATION op) {
            block.GetComponent<ConditionalCodeBlock>()?.SetOperation(op);
        }

        private void CopyOperationOntoClonePrefab() {
            if (codeBlockType.GetComponent<MathOperationCodeBlock>() != null) {
                CopyMathOperation(codeBlockType, gameObject.GetComponent<MathOperationCodeBlock>().op);
            } else if (codeBlockType.GetComponent<ConditionalCodeBlock>() != null) {
                CopyConditionalOperation(codeBlockType, gameObject.GetComponent<ConditionalCodeBlock>().op);
            }
        }
    }
}