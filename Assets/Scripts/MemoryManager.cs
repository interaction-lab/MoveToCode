using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MemoryManager : Singleton<MemoryManager> {

        // Variable Block Collection Should be it's own prefab
        // Memory manager will create these
        // From this collection, clicking it will spawn a new variable code block
        // Think like Scratch variables

        Canvas myCanvas;

        Dictionary<string, VariableBlockCollection> variables;

        private void Awake() {
            variables = new Dictionary<string, VariableBlockCollection>();
        }

        public Canvas GetCanvas() {
            if (myCanvas == null) {
                myCanvas = GetComponentInChildren<Canvas>();
            }
            return myCanvas;
        }

        public void AddNewVariableCodeBlock(string varName, IDataType dIn = null) {
            GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePathConstants.VariableCodeBlockCollectionPrefab), CodeBlockManager.instance.transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<VariableBlockCollection>().SetVariableName(varName);
            go.GetComponent<VariableBlockCollection>().SetVariableValue(dIn);

            go.transform.SetParent(GetCanvas().transform);
            go.transform.localScale = Vector3.one;
        }

        public void SpawnVariableCB() {
            AddNewVariableCodeBlock("potato", new IntDataType(4));
        }

    }
}