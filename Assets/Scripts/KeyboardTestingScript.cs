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

    ARObjectManager _arObjectManager;
    ARObjectManager ARObjectManagerInstance {
        get {
            if (_arObjectManager == null) {
                _arObjectManager = FindObjectOfType<ARObjectManager>();
            }
            return _arObjectManager;
        }
    }
    MazePiece bkMP;
    MazePiece BKMazePiece {
        get {
            if (bkMP == null) {
                bkMP = FindObjectOfType<BabyKuriTrackBeh>().GetComponent<MazePiece>();
            }
            return bkMP;
        }
    }

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void TestButton() {
        Debug.Log("Button pressed");
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            foreach (CodeBlock cb in CodeBlockManager.instance.GetAllCodeBlocks()) {
                Debug.Log(cb.GetMyIArgument().ToJSON());
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            TutorKuriManager.instance.TurnOffArrowPoint();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            TutorKuriManager.instance.TurnOnArrowPoint();
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            //FindObjectOfType<GoalTrackBeh>().UpdateAlphaTest();
        }
        // Debug.Log(Interpreter.instance.IsInResetState());

    }
}
