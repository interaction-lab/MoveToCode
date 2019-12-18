using MoveToCode;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {

    IntDataType i, i2;
    PrintInstruction pi;
    PrintInstruction p2;
    SubtractionInstruction st;

    public CodeBlock pb1, pb2, pb3;

    void Start() {
        i = new IntDataType(4);
        i2 = new IntDataType(9);
        pi = new PrintInstruction(i);
        p2 = new PrintInstruction(i);
        st = new SubtractionInstruction(i, i2);
        pi.SetNextInstruction(p2);
        pi.SetArgumentAt(st, 0);
        Interpreter.instance.AddToInstructionStack(pb1.myInstruction);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Interpreter.instance.RunNextInstruction();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log(1);
            pb2.RemoveChildBlockAndAttachNew(pb1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log(2);
            pb2.RemoveChildBlockAndAttachNew(pb3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Debug.Log(3);
            pb3.RemoveChildBlockAndAttachNew(pb1);
        }
    }
}
