using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class VariableBlockCollection : MonoBehaviour {
        TextMeshProUGUI textMesh;
        HashSet<VariableCodeBlock> variableCodeBlockSet;
        IDataType myData;
        string variableName;
        public GameObject variableBlock;

        private void Awake() {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
            UpdateText();
        }

        public IDataType GetVariableValueFromBlockCollection() {
            return myData;
        }

        public void SetVariableValue(IDataType valIn) {
            myData = valIn;
            UpdateText();
        }

        public void SetVariableName(string nameIn) {
            variableName = nameIn;
        }

        public void CreateNewVariableBlock() {
            GameObject go = Instantiate(variableBlock, CodeBlockManager.instance.transform.position, Quaternion.identity) as GameObject;
            go.transform.SetParent(CodeBlockManager.instance.transform);
        }

        public void AddCodeBlock(VariableCodeBlock cbIn) {
            variableCodeBlockSet.Add(cbIn);
        }

        public string GetVariableNameFromBlockCollection() {
            return variableName;
        }

        public void UpdateText() {
            textMesh.SetText(ToString());
        }

        public override string ToString() {
            return string.Join("", variableName, ": ", myData?.ToString());
        }
    }
}