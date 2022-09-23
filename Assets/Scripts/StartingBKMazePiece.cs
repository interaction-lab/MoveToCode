using UnityEngine;

namespace MoveToCode {
    /// <summary>
    /// Used as an indicator of the starting point for baby Kuri to be reset to
    /// </summary>
    public class StartingBKMazePiece : MonoBehaviour {
        #region members
        #endregion

        #region unity
        private void Start() {
#if UNITY_EDITOR
            // remove trashbutton canvas so that we don't accidently remove the goal during debugging in the editor
            foreach (Transform t in transform) {
                if (t.name.Contains("Trash")) {
                    Destroy(t.gameObject);
                    break;
                }
            }
#endif
        }
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
