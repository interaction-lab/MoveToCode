using MoveToCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {

    IntDataType i;
    PrintInstruction pi;
    void Start() {
        i = new IntDataType(4);
        pi = new PrintInstruction(i);

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Debug.Log("Run");
            pi.RunInstruction();
        }
    }
}
