using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
namespace MoveToCode {
    public class FreePlayMenuManager : Singleton<FreePlayMenuManager> {

        GameObject printBlock, conditionBlock, ifBlock, intBlock, mathBlock, setVarBlock, stringBlock, whileBlock, charBlock, arrayBlock, arrayIndexBlock;
        PressableButtonHoloLens2[] pressableButtons;

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
            charBlock = Resources.Load<GameObject>(ResourcePathConstants.CharCodeBlockPrefab);
            arrayBlock = Resources.Load<GameObject>(ResourcePathConstants.ArrayCodeBlockPrefab);
            arrayIndexBlock = Resources.Load<GameObject>(ResourcePathConstants.ArrayIndexCodeBlockPrefab);
        }

        public GameObject InstantiateBlock(GameObject block) {
            GameObject go = Instantiate(block);
            go.transform.position = transform.position;
            go.transform.SnapToCodeBlockManager();
            return go;
        }

        public void TurnMenuOn() {
            gameObject.SetActive(true);
        }

        public string FakePressRandomButton() {
            if (pressableButtons == null) {
                pressableButtons = GetComponentsInChildren<PressableButtonHoloLens2>();
            }
            PressableButtonHoloLens2 button = pressableButtons[Random.Range(0, pressableButtons.Length)];
            FakePressButton fbp = button.GetComponent<FakePressButton>();
            if (fbp == null) {
                fbp = button.gameObject.AddComponent<FakePressButton>();
            }
            return fbp.PressButton();
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
            GameObject go = InstantiateBlock(intBlock);
            (go.GetComponent<IntCodeBlock>().GetMyInternalIArgument() as IntDataType).SetValue(Random.Range(-5, 5));
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
        public void InstanstiateCharCodeBlock() {
            GameObject go = InstantiateBlock(charBlock);
            (go.GetComponent<CharCodeBlock>().GetMyInternalIArgument() as CharDataType).SetValue(Random.Range('a', 'z'));
        }
        public void InstantiateVariableBlockCollection() {
            MemoryManager.instance.AddNewVariableCodeBlock(string.Join("", "Var", MemoryManager.instance.GetNumVariables().ToString()), new IntDataType(null, 0));
        }
        public void InstantiateArrayCodeBlock() {
            InstantiateBlock(arrayBlock);
        }
        public void InstantiateArrayIndexCodeBlock() {
            InstantiateBlock(arrayIndexBlock);
        }
    }
}
