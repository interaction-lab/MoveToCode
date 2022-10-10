using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using MoveToCode;
using RosSharp.RosBridgeClient;
using UnityEngine;

public class KeyboardTestingScript : MonoBehaviour {
    public Transform t1, t2;
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
    KuriAIBTRandom kuriAIBTRandom;
    KuriAIBTRandom KuriAIBTRandomInstance {
        get {
            if (kuriAIBTRandom == null) {
                kuriAIBTRandom = FindObjectOfType<KuriAIBTRandom>();
            }
            return kuriAIBTRandom;
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
            KuriAIBTRandomInstance.Clap();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            // print distance from t1 to t2
            Debug.Log(Vector3.Distance(t1.position, t2.position));
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            //FindObjectOfType<GoalTrackBeh>().UpdateAlphaTest();
        }
        // Debug.Log(Interpreter.instance.IsInResetState());

    }
}
