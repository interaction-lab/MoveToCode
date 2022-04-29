using System.Collections.Generic;
using System.Linq;
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
        List<CodeBlock> _activeCBs;
        List<CodeBlock> ActiveCodeBlocks {
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
            foreach (CodeBlock c in ActiveCodeBlocks) {
                codeBlockJsonList.Add(c.GetMyIArgument().ToJSON());
            }
            _activeCBs = null; // reset every time we log, hacky but works for now
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
            Vector3 fbcbT = ActiveCodeBlocks.First().transform.position;
            Vector3 highestPos = fbcbT;
            Vector3 lowestPos = fbcbT;
            Vector3 mostRightPos = fbcbT;
            Vector3 mostLeftPos = fbcbT;
            foreach (CodeBlock cb in ActiveCodeBlocks) {
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
            Vector2 centerPos = new Vector2(mostLeftPos.x + mostRightPos.x, lowestPos.y + highestPos.y) / 2;
            float bottomToCenter = centerPos.y - lowestPos.y;
            float rightToCenter = centerPos.x - mostLeftPos.x;
            // move code blocks close to BKMazePiece
            transform.position = UserTransform.position + UserTransform.forward * 0.75f + UserTransform.right * 0.2f + UserTransform.up * 0.2f;
            //MazeManager.instance.BKMazePiece.transform.position +
                                //(Vector3.up * .25f + Vector3.right * .2f);

            // find the direction toward the user
            Vector3 directionToUser = UserTransform.position - transform.position;
            // rotate the code block to face the user
            transform.rotation = Quaternion.LookRotation(-directionToUser);
            #endregion
        }
    }
}