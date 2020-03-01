using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveToCode {
    public class FreePlayMenuManager : Singleton<FreePlayMenuManager> {

        GameObject printBlock, conditionBlock, ifBlock, intBlock, mathBlock, setVarBlock, ;


        private void Awake() {
            Setup();
        }

        private void Setup() {
            printBlock = Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab);
        }

        public void InstanstiatePrintCodeBlock() {
            GameObject go = Instantiate(printBlock);
            go.transform.position = transform.position;
            go.transform.SnapToCodeBlockManager();
        }

        public void RotateTowardUser() {
            transform.RotateTowardsUser();
        }
    }
}
