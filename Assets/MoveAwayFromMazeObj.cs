using UnityEngine;
namespace MoveToCode {
    public class MoveAwayFromMazeObj : Singleton<MoveAwayFromMazeObj> {
        #region members
        #endregion

        #region unity
        private void OnEnable() {
#if !UNITY_EDITOR
            // get mesh renderer and turn it off
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if(mr != null){
                mr.enabled = false;
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
