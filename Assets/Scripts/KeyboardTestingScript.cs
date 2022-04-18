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


    void Update() {
        if (Input.GetKeyDown("h")) {
            //anim.SetTrigger("HighFive");


        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            MazeGraph myMaze = new MazeGraph(MazeManager.instance.BKMazePiece);
            MazeGraph solMaze = new MazeGraph(SolMazeManager.instance.BKMazePiece);
            foreach (MPEdge edge in myMaze.GetAllEdges()) {
                Debug.Log(edge);
            }
            foreach (MPEdge edge in solMaze.GetAllEdges()) {
                Debug.Log(edge);
            }
            Debug.Log("ContainsSubgraph: " + myMaze.ContainsSubgraph(solMaze));
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log(MazeManager.instance.GetPotentialNextMazePieceForward());
            Debug.Log(MazeManager.instance.GetPotentialNextMazePieceForward().Center);
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
