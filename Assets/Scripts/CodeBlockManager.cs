using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockManager : Singleton<CodeBlockManager> {
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

        private void SetCompatibleColliderState(CodeBlock cIn, bool desiredActiveState) {
            IArgument internalArg = cIn.GetMyIArgument();
            foreach (SnapCollider sc in GetAllSnapColliders()) {
                if (sc.HasCompatibleType(internalArg) && !sc.MyCodeBlock.GetIsMenuBlock()) {
                    sc.gameObject.SetActive(desiredActiveState);
                }
            }
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

        bool hasBeenInitialized = false;
        void OnEnable() {
            if (!hasBeenInitialized) {
                hasBeenInitialized = true;
                LoggingManagerInstance.AddLogColumn(codeBlockJsonCol, "");
                ExerciseManager.instance.OnCyleNewExercise.AddListener(LogAllCodeBlocks);
                LogAllCodeBlocks();
            }
            PositionNextToBKMazePiece();
        }

        private void PositionNextToBKMazePiece() {
            // move code blocks close to BKMazePiece
            transform.position = MazeManager.instance.BKMazePiece.transform.position +
                                (Vector3.up * 0.5f + Vector3.left * 0.2f); // arbitrrary scaling factor
            // find the direction toward the user
            Vector3 directionToUser = UserTransform.position - transform.position;
            // rotate the code block to face the user
            transform.rotation = Quaternion.LookRotation(-directionToUser);
        }

        public void LogAllCodeBlocks() {
            List<string> codeBlockJsonList = new List<string>();
            foreach (Transform t in transform) {
                CodeBlock c = t.GetComponent<CodeBlock>();
                if (c != null) {
                    codeBlockJsonList.Add(c.GetMyIArgument().ToJSON());
                }
            }
            LoggingManagerInstance.UpdateLogColumn(codeBlockJsonCol, "[" + string.Join(",", codeBlockJsonList) + "]");
        }

    }
}