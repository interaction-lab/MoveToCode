using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class VariableBlockCollection : MonoBehaviour {
        TextMeshPro textMesh;
        HashSet<VariableCodeBlock> variableCodeBlockSet;
        IDataType myData;
        string variableName;

        private void Awake() {
            variableCodeBlockSet = new HashSet<VariableCodeBlock>();
            textMesh = transform.GetChild(3).GetComponentInChildren<TextMeshPro>();
            if (GetComponent<ManipulationLogger>() == null) {
                gameObject.AddComponent<ManipulationLogger>();
            }
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
            GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePathConstants.VariableCodeBlockPrefab), CodeBlockManager.instance.transform) as GameObject;
            go.GetComponent<VariableCodeBlock>().SetParentBlockCollection(this);
            go.transform.position = transform.position + Vector3.back * 0.05f;
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