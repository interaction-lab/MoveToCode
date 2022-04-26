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
        if (Input.GetKeyDown("h")) {
            //anim.SetTrigger("HighFive");
            (TutorKuriManager.instance.kuriController as VirtualKuriController).GoToUser();

        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            foreach (CodeBlock cb in CodeBlockManager.instance.GetAllCodeBlocks()) {
                Debug.Log(cb.GetMyIArgument().ToJSON());
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log(MazeManager.instance.GetPotentialNextMP(CodeBlockEnums.Move.Forward)?.Center);
            Debug.Log(MazeManager.instance.GetPotentialNextMP(CodeBlockEnums.Move.Backward)?.Center);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            //  MenuManager.instance.FakePressPlay();
            //  HumanStateManager.instance.DebugLogData();
            //FindObjectOfType<KuriEmoteStringPublisher>().PubRandomNegative();
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            //FindObjectOfType<GoalTrackBeh>().UpdateAlphaTest();
        }
        // Debug.Log(Interpreter.instance.IsInResetState());

    }
}
