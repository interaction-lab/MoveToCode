using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using MoveToCode;
using RosSharp.RosBridgeClient;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {
    public Animator anim;

    ARTrackingManager arTrackingManager;
    ARTrackingManager ARTrackingManagerInstance {
        get {
            if (arTrackingManager == null) {
                arTrackingManager = ARTrackingManager.instance;
            }
            return arTrackingManager;
        }
    }
    void Start() {
        anim = GetComponent<Animator>();
    }


    void Update() {
        if (Input.GetKeyDown("h")) {
            //anim.SetTrigger("HighFive");
            Interpreter.instance.ResetCodeState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            // Interpreter.instance.RunNextInstruction();
            // KuriManager.instance
            FindObjectOfType<VirtualKuriController>().GetComponent<VirtualKuriController>().TakeMovementAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            //  MenuManager.instance.FakePressPlay();
            //  HumanStateManager.instance.DebugLogData();
            //FindObjectOfType<KuriEmoteStringPublisher>().PubRandomNegative();
        }
        // Debug.Log(Interpreter.instance.IsInResetState());

    }
}
