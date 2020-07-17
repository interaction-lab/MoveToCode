using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode {
    public class CloneOnDrag : MonoBehaviour {
        ManipulationHandler manipulationHandler;
        Vector3 startingPosition;
        GameObject clone;
        GameObject codeBlockType;
        bool blockStillInMenu;

        private void Awake() {
            startingPosition = transform.position;
            blockStillInMenu = true;
        }

        public void SetCodeBlockType(GameObject cb) {
            codeBlockType = cb;
        }

        public void OnEnable() {
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(StartedMotion);
            manipulationHandler.OnManipulationEnded.AddListener(StoppedMotion);
        }

        private void StoppedMotion(ManipulationEventData arg0) {
            //delete block if still on shelf/placed back on shelf
            if(blockStillInMenu) {
                Destroy(gameObject);
            }
        }

        private void StartedMotion(ManipulationEventData arg0) {
            if (transform.GetComponent<CodeBlock>().GetIsMenuBlock()) {
                transform.GetComponent<CodeBlock>().SetIsMenuBlock(false);
                clone = InstantiateBlock(codeBlockType, startingPosition);
                clone.GetComponent<CodeBlock>().SetIsMenuBlock(true);
                transform.SnapToCodeBlockManager();
            }
        }

        private GameObject InstantiateBlock(GameObject block, Vector3 spawnPos) {
            GameObject go = Instantiate(block, spawnPos, Quaternion.identity);
            go.AddComponent<CloneOnDrag>().SetCodeBlockType(codeBlockType);
            go.transform.SetParent(transform.parent);
            go.transform.localScale = Vector3.one;
            return go;
        }

        public void SetBlockStillInMenu(bool option) {
            blockStillInMenu = option;
        }
    }
}