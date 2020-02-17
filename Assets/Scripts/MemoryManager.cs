using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MemoryManager : Singleton<MemoryManager> {

        Canvas myCanvas;

        Dictionary<string, VariableBlockCollection> variables;

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
            return new List<string>(variables.Keys);
        }

        public void RemoveVariable(string name) {
            GetVariables().Remove(name); // TODO: get this to also remove all block collections?
        }

        public void AddNewVariableCodeBlock(string varName, IDataType dIn = null) {
            GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePathConstants.VariableCodeBlockCollectionPrefab), CodeBlockManager.instance.transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<VariableBlockCollection>().SetVariableName(varName);
            go.GetComponent<VariableBlockCollection>().SetVariableValue(dIn);

            go.transform.SetParent(GetCanvas().transform);
            go.transform.localScale = Vector3.one;

            GetVariables().Add(varName, go.GetComponent<VariableBlockCollection>());
        }

        // TODO: Make this much better
        public void SpawnVariableCB() {
            AddNewVariableCodeBlock("potato", new IntDataType(4));
        }

    }
}