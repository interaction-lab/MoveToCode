using MoveToCode;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {

    IntDataType i;
    PrintInstruction pi;
    PrintInstruction p2;
    void Start() {
        i = new IntDataType(4);
        pi = new PrintInstruction(i);
        p2 = new PrintInstruction(i);
        pi.SetNextInstruction(p2);
        Interpreter.instance.AddToInstructionStack(pi);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Interpreter.instance.RunNextInstruction();
        }
    }
}
