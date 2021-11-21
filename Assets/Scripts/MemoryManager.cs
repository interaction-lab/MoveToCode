using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MemoryManager : Singleton<MemoryManager> {

        Canvas myCanvas;

        Dictionary<string, VariableBlockCollection> variables;
        Dictionary<string, IDataType> variableSaveState;
        Transform memoryHeader;
        float scaleForSetting = 0.175f;

        private void Awake() {
            GetVariables();
            memoryHeader = GetCanvas().transform.GetChild(0);
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

        public IDataType GetVariableValue(string name) {
            return GetVariables()[name].GetVariableValueFromBlockCollection();
        }

        public int GetNumVariables() {
            return GetVariableNames().Count;
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
            if (GetNumVariables() == 0) {
                memoryHeader.gameObject.SetActive(false);
            }
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
            GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePathConstants.VariableCodeBlockCollectionPrefab), CodeBlockManager.instance.transform.position, Quaternion.identity);
            go.GetComponent<VariableBlockCollection>().SetVariableName(varName);
            go.GetComponent<VariableBlockCollection>().SetVariableValue(dIn);

            go.transform.SnapToParent(GetCanvas().transform, new Vector3(0, -1 * GetNumVariables() * scaleForSetting, -0.0f));

            GetVariables()[varName] = go.GetComponent<VariableBlockCollection>();
            if (!memoryHeader.gameObject.activeSelf) {
                memoryHeader.gameObject.SetActive(true);
            }
        }

    }
}