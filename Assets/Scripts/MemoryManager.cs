using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MemoryManager : Singleton<MemoryManager> {

        Canvas myCanvas;

        Dictionary<string, VariableBlockCollection> variables;
        Dictionary<string, IDataType> variableSaveState;

        private void Awake() {
            GetVariables();
        }

        public Canvas GetCanvas() {
            if (myCanvas == null) {
                myCanvas = GetComponentInChildren<Canvas>();
            }
            return myCanvas;
        }

        public Dictionary<string, VariableBlockCollection> GetVariables() {
            if (variables == null) {
                variables = new Dictionary<string, VariableBlockCollection>();
            }
            return variables;
        }

        public List<string> GetVariableNames() {
            return new List<string>(GetVariables().Keys);
        }

        public Dictionary<string, IDataType> GetVariableSaveState() {
            if (variableSaveState == null) {
                variableSaveState = new Dictionary<string, IDataType>();
            }
            return variableSaveState;
        }

        public void SaveMemoryState() {
            GetVariableSaveState().Clear();
            foreach (string varName in GetVariableNames()) {
                GetVariableSaveState()[varName] = GetVariables()[varName].GetVariableValueFromBlockCollection();
            }
        }

        public void ResetMemoryState() {
            if (GetVariableSaveState().Empty()) {
                return;
            }
            foreach (string varName in GetVariableNames()) {
                GetVariables()[varName].SetVariableValue(GetVariableSaveState()[varName]);
            }
        }

        public void RemoveVariable(string name) {
            Destroy(GetVariables()[name].gameObject);
            GetVariables().Remove(name); // TODO: get this to also remove all block collections?
        }

        public void RemoveAllVariables() {
            if (GetVariableSaveState().Empty()) {
                return;
            }
            foreach (string varName in GetVariableNames()) {
                RemoveVariable(varName);
            }
        }

        public void AddNewVariableCodeBlock(string varName, IDataType dIn = null) {
            GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePathConstants.VariableCodeBlockCollectionPrefab), CodeBlockManager.instance.transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<VariableBlockCollection>().SetVariableName(varName);
            go.GetComponent<VariableBlockCollection>().SetVariableValue(dIn);

            go.transform.SetParent(GetCanvas().transform);
            go.transform.localScale = Vector3.one;

            GetVariables()[varName] = go.GetComponent<VariableBlockCollection>();
        }




    }
}