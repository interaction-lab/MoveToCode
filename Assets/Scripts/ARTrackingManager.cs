namespace MoveToCode {
    public class ARTrackingManager : Singleton<ARTrackingManager> {
        #region members
        MazeManager mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (mazeManager == null) {
                    mazeManager = MazeManager.instance;
                }
                return mazeManager;
            }
        }

        public bool IsTracking {
            get {
                return !MazeManagerInstance.IsLocked; // moved to all state done by `MazeManager.cs`, not perfect but way nicer this way
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}