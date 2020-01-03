using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MemoryManager : Singleton<MemoryManager> {
        public GameObject variableCB;

        //memory manager spawns a variable set
        // variable set can spawn code blocks

        Dictionary<string, VariableBlockCollection> variables;

        private void Awake() {
            variables = new Dictionary<string, VariableBlockCollection>();
        }

        public void AddNewVariableCodeBlock(string varName, IDataType dIn = null) {
            GameObject go = Instantiate(variableCB, CodeBlockManager.instance.transform.position, Quaternion.identity) as GameObject;
            go.transform.SetParent(CodeBlockManager.instance.transform);
            if (variables.ContainsKey(varName)) {
                variables[varName].AddCodeBlock(cbIn);
            }
            else {
                variables[varName] = new VariableBlockCollection(dIn, varName);
            }
        }

        public void SpawnVariableCB() {


            //yield return new WaitForEndOfFrame();
            AddNewVariableCodeBlock("potato", go.GetComponent<VariableCodeBlock>(), new StringDataType("p"));
            Debug.Log("spawned");
        }

    }
}