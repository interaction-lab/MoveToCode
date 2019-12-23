using MoveToCode;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {

    IntDataType i, i2;
    PrintInstruction pi;
    PrintInstruction p2;
    SubtractionInstruction st;

    public CodeBlock pb1, pb2, pb3, dt1, dt2, dt3, subCB;

    void Start() {
        /* i = new IntDataType(4);
         i2 = new IntDataType(9);
         pi = new PrintInstruction(i);
         p2 = new PrintInstruction(i);
         st = new SubtractionInstruction(i, i2);
         pi.SetNextInstruction(p2);
         pi.SetArgumentAt(st, 0);*/
        Interpreter.instance.AddToInstructionStack(pb1.GetInstruction());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Interpreter.instance.RunNextInstruction();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            subCB.SetArgumentBlockAt(dt1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            subCB.SetArgumentBlockAt(dt2, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            pb1.SetArgumentBlockAt(subCB, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            pb1.SetNextCodeBlock(pb2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            pb2.SetArgumentBlockAt(dt3, 0);
        }
    }
}
