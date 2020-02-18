using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using MoveToCode;
using RosSharp.RosBridgeClient;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {

    IntDataType i, i2;
    PrintInstruction pi;
    PrintInstruction p2;
    SubtractionInstruction st;

    public CodeBlock pb1, pb2, pb3, dt1, dt2, dt3, subCB;
    public static string colname = "object";

    void Start() {
        // Need to redo codeblock structure/adding args
        /* i = new IntDataType(4);
         i2 = new IntDataType(9);
         pi = new PrintInstruction(i);
         p2 = new PrintInstruction(i);
         st = new SubtractionInstruction(i, i2);
         pi.SetNextInstruction(p2);
         pi.SetArgumentAt(st, 0);*/
        //Interpreter.instance.AddToInstructionStack(pb1.GetMyInstruction());
        LoggingManager.instance.AddLogColumn(colname, "");
    }



    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Interpreter.instance.RunNextInstruction();
        }

        // use full name usually

    }
}
