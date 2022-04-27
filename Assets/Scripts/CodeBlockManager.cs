using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockManager : Singleton<CodeBlockManager> {
        #region members
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
        bool hasBeenInitialized = false;
        #endregion

        #region unity
        void OnEnable() {
            if (!hasBeenInitialized) {
                hasBeenInitialized = true;
                LoggingManagerInstance.AddLogColumn(codeBlockJsonCol, "");
                ExerciseManager.instance.OnCyleNewExercise.AddListener(LogAllCodeBlocks);
                LogAllCodeBlocks();
            }
            PositionNextToBKMazePiece();
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
            List<string> codeBlockJsonList = new List<string>();
            foreach (Transform t in transform) {
                CodeBlock c = t.GetComponent<CodeBlock>();
                if (c != null) {
                    codeBlockJsonList.Add(c.GetMyIArgument().ToJSON());
                }
            }
            LoggingManagerInstance.UpdateLogColumn(codeBlockJsonCol, "[" + string.Join(",", codeBlockJsonList) + "]");
        }
        #endregion

        #region private
        private void SetCompatibleColliderState(CodeBlock cIn, bool desiredActiveState) {
            IArgument internalArg = cIn.GetMyIArgument();
            foreach (SnapCollider sc in GetAllSnapColliders()) {
                if (sc.HasCompatibleType(internalArg) && !sc.MyCodeBlock.GetIsMenuBlock()) {
                    sc.gameObject.SetActive(desiredActiveState);
                }
            }
        }
        private void PositionNextToBKMazePiece() {
            Vector3 highestPos = Vector3.zero;
            Vector3 lowestPos = Vector3.zero;
            Vector3 mostRightPos = Vector3.zero;
            Vector3 mostLeftPos = Vector3.zero;
            highestPos.y = -100;
            lowestPos.y = 100;
            mostRightPos.x = -100;
            mostLeftPos.x = 100;
            foreach (CodeBlock cb in GetAllCodeBlocks()) {
                if (cb.GetIsMenuBlock()) {
                    continue;
                }
                Vector3 cbPos = cb.transform.position;
                if (cbPos.y > highestPos.y) {
                    highestPos = cbPos;
                }
                if (cbPos.y < lowestPos.y) {
                    lowestPos = cbPos;
                }
                if (cbPos.x > mostRightPos.x) {
                    mostRightPos = cbPos;
                }
                if (cbPos.x < mostLeftPos.x) {
                    mostLeftPos = cbPos;
                }
            }
            // find the average of the highest, lowest, most right, and most left code block positions
            Vector3 averagePos = (highestPos + lowestPos + mostRightPos + mostLeftPos) / 4;
            float lowestToaverageDiff = averagePos.y - lowestPos.y;
            // move code blocks close to BKMazePiece
            transform.position = MazeManager.instance.BKMazePiece.transform.position +
                                (Vector3.up * lowestToaverageDiff + Vector3.left * 0.2f); // arbitrrary scaling factor
                                                                                          // find the direction toward the user
                                                                                          // rotate the code block to face the user
            foreach (CodeBlock cb in GetAllCodeBlocks()) {
                Vector3 directionToUser = UserTransform.position - cb.transform.position;
                cb.transform.rotation = Quaternion.LookRotation(-directionToUser);
            }
            #endregion
        }
    }
}