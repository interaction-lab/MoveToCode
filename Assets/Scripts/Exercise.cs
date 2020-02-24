using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoveToCode {
    public class Exercise : MonoBehaviour {
        public string consoleStringGoal;

        public bool IsExerciseCorrect() {
            Debug.Log(ConsoleManager.instance.GetCleanedMainText());
            Debug.Log(consoleStringGoal);
            return ConsoleManager.instance.GetCleanedMainText() == consoleStringGoal;
        }

        private void SnapAllBlocksToBlockManager() {
            foreach (CodeBlock cb in GetComponentsInChildren<CodeBlock>()) {
                cb.transform.SnapToCodeBlockManager();
            }
        }

        public void UnsnapAllBlockFromBlockManager() {
            StartCodeBlock.instance.SetArgumentBlockAt(null, 0); // unsnap
            foreach (CodeBlock cb in CodeBlockManager.instance.GetAllCodeBlocks()) {
                if (cb != StartCodeBlock.instance) {
                    cb.transform.SnapToParent(transform);
                }
            }
        }

        private void OnEnable() {
            SnapAllBlocksToBlockManager();
        }


    }
}
