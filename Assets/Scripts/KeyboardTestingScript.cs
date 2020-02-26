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
            Debug.Log(Time.time);
            //FindObjectOfType<KuriEmoteStringPublisher>().PubRandomPositive();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {

            //FindObjectOfType<KuriEmoteStringPublisher>().PubRandomNegative();
        }

    }
}
