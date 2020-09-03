using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using MoveToCode;
using RosSharp.RosBridgeClient;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Interpreter.instance.RunNextInstruction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            MenuManager.instance.FakePressPlay();
            HumanStateManager.instance.DebugLogData();
            //FindObjectOfType<KuriEmoteStringPublisher>().PubRandomNegative();
        }

    }
}
