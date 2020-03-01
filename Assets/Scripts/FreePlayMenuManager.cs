using UnityEngine;
namespace MoveToCode {
    public class FreePlayMenuManager : Singleton<FreePlayMenuManager> {

        GameObject printBlock, conditionBlock, ifBlock, intBlock, mathBlock, setVarBlock, stringBlock, whileBlock;


        private void Awake() {
            Setup();
        }

        private void Setup() {
            printBlock = Resources.Load<GameObject>(ResourcePathConstants.PrintCodeBlockPrefab);
            conditionBlock = Resources.Load<GameObject>(ResourcePathConstants.ConditionBlockPrefab);
            ifBlock = Resources.Load<GameObject>(ResourcePathConstants.IfCodeBlockPrefab);
            intBlock = Resources.Load<GameObject>(ResourcePathConstants.IntCodeBlockPrefab);
            mathBlock = Resources.Load<GameObject>(ResourcePathConstants.MathCodeBlockPrefab);
            setVarBlock = Resources.Load<GameObject>(ResourcePathConstants.SetVariableCodeBlockPrefab);
            stringBlock = Resources.Load<GameObject>(ResourcePathConstants.StringCodeBlockPrefab);
            whileBlock = Resources.Load<GameObject>(ResourcePathConstants.WhileCodeBlockPrefab);

        }

        public void InstantiateBlock(GameObject block) {
            GameObject go = Instantiate(block);
            go.transform.position = transform.position;
            go.transform.SnapToCodeBlockManager();
        }

        public void InstanstiatePrintCodeBlock() {
            InstantiateBlock(printBlock);
        }
        public void InstanstiateConditionCodeBlock() {
            InstantiateBlock(conditionBlock);
        }
        public void InstanstiateIfCodeBlock() {
            InstantiateBlock(ifBlock);
        }
        public void InstanstiateIntCodeBlock() {
            InstantiateBlock(intBlock);
        }
        public void InstanstiateMathCodeBlock() {
            InstantiateBlock(mathBlock);
        }
        public void InstanstiateSetVariableCodeBlock() {
            InstantiateBlock(setVarBlock);
        }
        public void InstanstiateStringCodeBlock() {
            InstantiateBlock(stringBlock);
        }
        public void InstanstiateWhileCodeBlock() {
            InstantiateBlock(whileBlock);
        }
    }
}
