using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockManager : Singleton<CodeBlockManager> {
        #region members
        Vector3 OriginalPos = Vector3.zero;

        HashSet<CodeBlock> codeBlocks;
        HashSet<SnapCollider> snapColliders;
        public static string codeBlockJsonCol = "codeBlockJson";
        LoggingManager _lm;
        LoggingManager LoggingManagerInstance {
            get {
                if (_lm == null) {
                    _lm = LoggingManager.instance;
                }
                return _lm;
            }
        }
        Transform _userTransform;
        Transform UserTransform {
            get {
                if (_userTransform == null) {
                    _userTransform = Camera.main.transform;
                }
                return _userTransform;
            }
        }
        ExerciseManager exerciseManagerInstance;
        ExerciseManager ExerciseManagerInstance {
            get {
                if (exerciseManagerInstance == null) {
                    exerciseManagerInstance = ExerciseManager.instance;
                }
                return exerciseManagerInstance;
            }
        }
        ViewPortManager _viewPortManager;
        ViewPortManager ViewPortManagerInstance {
            get {
                if (_viewPortManager == null) {
                    _viewPortManager = ViewPortManager.instance;
                }
                return _viewPortManager;
            }
        }
        ArrowPointPrefab startBlockArrowPoint;
        Transform startBlockTransform;
        Transform StartBlockTransform {
            get {
                if (startBlockTransform == null) {
                    startBlockTransform = StartCodeBlock.instance.transform;
                }
                return startBlockTransform;
            }
        }

        public HashSet<CodeBlock> GetAllCodeBlocks() {
            if (codeBlocks == null) {
                codeBlocks = new HashSet<CodeBlock>();
            }
            return codeBlocks;
        }
        public HashSet<SnapCollider> GetAllSnapColliders() {
            if (snapColliders == null) {
                snapColliders = new HashSet<SnapCollider>();
            }
            return snapColliders;
        }
        bool hasBeenInitialized = false;
        List<CodeBlock> _activeCBs;
        public List<CodeBlock> ActiveCodeBlocks {
            get {
                if (_activeCBs == null) {
                    _activeCBs = new List<CodeBlock>();
                    foreach (Transform t in transform) {
                        CodeBlock c = t.GetComponent<CodeBlock>();
                        if (c != null) {
                            _activeCBs.Add(c);
                        }
                    }
                }
                return _activeCBs;
            }
        }
        #endregion

        #region unity
        void OnEnable() {
            if (!hasBeenInitialized) {
                hasBeenInitialized = true;
                OriginalPos = transform.position;
                LoggingManagerInstance.AddLogColumn(codeBlockJsonCol, "");
                ExerciseManagerInstance.OnCyleNewExercise.AddListener(OnCycleNewExercise);
                SwitchModeButton.instance.OnSwitchToCodingMode.AddListener(OnSwitchToCodingMode);
                SwitchModeButton.instance.OnSwitchToMazeBuildingMode.AddListener(OnSwitchToMazeBuildingMode);
                SpawnArrowPrefab();
                LogAllCodeBlocks();
            }
            else {
                UpdateStartBlockLocation();
            }
        }
        #endregion

        #region public
        public void RegisterCodeBlock(CodeBlock cIn) {
            GetAllCodeBlocks().Add(cIn);
        }
        public void RegisterSnapCollider(SnapCollider sIn) {
            GetAllSnapColliders().Add(sIn);
        }
        public void DeregisterCodeBlock(CodeBlock cIn) {
            GetAllCodeBlocks().Remove(cIn);
        }
        public void DeregisterSnapCollider(SnapCollider sIn) {
            GetAllSnapColliders().Remove(sIn);
        }
        public void EnableCollidersCompatibleCodeBlock(CodeBlock cIn) {
            SetCompatibleColliderState(cIn, true);
        }
        // Maybe make this "disable all active colliders by keeping track of them?
        public void DisableCollidersCompatibleCodeBlock(CodeBlock cIn) {
            SetCompatibleColliderState(cIn, false);
        }
        public void ResetAllCodeBlockInternalState() {
            foreach (CodeBlock c in GetAllCodeBlocks()) {
                c.ResetInstructionInternalState();
            }
        }
        public void HideCodeBlocks() {
            gameObject.SetActive(false);
        }
        public void ShowCodeBlocks() {
            gameObject.SetActive(true);
        }
        public void LogAllCodeBlocks() {
            _activeCBs = null; // reset every time we log, hacky but works for now
            List<string> codeBlockJsonList = new List<string>();
            foreach (CodeBlock c in ActiveCodeBlocks) {
                codeBlockJsonList.Add(c.GetMyIArgument().ToJSON());
            }
            _activeCBs = null; // reset every time we log, hacky but works for now
            LoggingManagerInstance.UpdateLogColumn(codeBlockJsonCol, "[" + string.Join(",", codeBlockJsonList) + "]");
        }

        #endregion

        #region private
        private void SpawnArrowPrefab() {
            startBlockArrowPoint = ViewPortManagerInstance.SpawnNewArrowPoint(StartBlockTransform,
                Vector3.zero,
                Color.white, // outer color
                new Color32(0x0F, 0xBD, 0x8C, 0xFF),  // inner color, hardcoded from start block color, should be dynamic but nobody got time for that rn
                "Start Block Is Behind You");
            TurnOffArrow();
        }
        void OnSwitchToCodingMode() {
            ShowCodeBlocks();
            SetUpArrow();
        }
        void OnSwitchToMazeBuildingMode() {
            HideCodeBlocks();
            TurnOffArrow();
        }

        void TurnOffArrow() {
            ViewPortManagerInstance.TurnOffArrow(StartBlockTransform);
        }

        void SetUpArrow() {
            // if startblock is out of view, turn on arrow and subscribe to exit event
            if (startBlockArrowPoint.IsInViewPort) {
                TurnOffArrow();
            }
            else {
                ViewPortManagerInstance.TurnOnArrow(StartBlockTransform);
                startBlockArrowPoint.OnEnterViewPort.AddListener(OnStartBlockEnterViewPort);
            }
        }

        void OnStartBlockEnterViewPort() {
            TurnOffArrow();
        }

        private void OnCycleNewExercise() {
            LogAllCodeBlocks();
            transform.position = OriginalPos;
            transform.rotation = Quaternion.identity;
            StartCodeBlock.instance.ResetToLocalStartLocation(); // TODO: double check where this goes
            StartLocationSet = false;
        }

        bool StartLocationSet = false;
        private void UpdateStartBlockLocation() {
            if (!StartLocationSet) {
                SpawnStartBlockInFrontOfPlayer();
                StartLocationSet = true;
            }
        }

        private void SetCompatibleColliderState(CodeBlock cIn, bool desiredActiveState) {
            IArgument internalArg = cIn.GetMyIArgument();
            foreach (SnapCollider sc in GetAllSnapColliders()) {
                if (sc.HasCompatibleType(internalArg) && !sc.MyCodeBlock.GetIsMenuBlock()) {
                    sc.gameObject.SetActive(desiredActiveState);
                }
            }
        }

        void SpawnStartBlockInFrontOfPlayer() {


            // get goal piece
            MazePiece goalPiece = MazeManager.instance.BKMazePiece;
            // checked that is has been tracked
            Vector3 goalPos = UserTransform.position + UserTransform.forward.normalized * 0.3f; // should be player to goal piece
            if (goalPiece.HasBeenTracked) {
                Vector3 flatPlayer = UserTransform.position;
                flatPlayer.y = 0;
                Vector3 flatGoal = goalPiece.transform.position;
                flatGoal.y = 0;
                Vector3 playerToGoal = flatGoal - flatPlayer;
                Vector3 playerToGoalNormalized = playerToGoal.normalized * 0.2f; // 20cm forward
                playerToGoalNormalized.y =  0.5f; // 50cm above ground
                goalPos = goalPiece.transform.position + playerToGoalNormalized;
            }

            // find the direction toward the user
            Vector3 directionToUser = UserTransform.position - goalPos;
            // rotate the code block to face the user
            transform.rotation = Quaternion.LookRotation(-directionToUser);

            transform.position = goalPos;
            // // check if they are upside down relative to the user
            // if (Vector3.Dot(transform.up, UserTransform.up) < 0) {
            //     // flip the code block
            //     transform.Rotate(Vector3.right, 180);
            // }
        }
        #endregion
    }
}