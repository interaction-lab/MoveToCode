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

    private void OnEnable() {
        ARTrackingManagerInstance.OnTrackingStarted.AddListener(StartTracking);
        ARTrackingManagerInstance.OnTrackingEnded.AddListener(StopTracking);
    }

    private void OnDisable() {
        ARTrackingManagerInstance.OnTrackingStarted.RemoveListener(StartTracking);
        ARTrackingManagerInstance.OnTrackingEnded.RemoveListener(StopTracking);
    }

    private void StartTracking() {
        KuriTextManager.instance.Addline("Start Tracking");
    }
    private void StopTracking() {
        KuriTextManager.instance.Addline("Stop Tracking");
    }


    void Update() {
        if (Input.GetKeyDown("h")) {
            //anim.SetTrigger("HighFive");
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
