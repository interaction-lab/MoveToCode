namespace MoveToCode {
    public class SnapLoggingManager : Singleton<SnapLoggingManager> {
        static string snapToColName = "SnapTo", snapRemoveFromColName = "SnapRemoveFrom";

        private void Start() {
            LoggingManager.instance.AddLogColumn(snapToColName, "");
            LoggingManager.instance.AddLogColumn(snapRemoveFromColName, "");
        }

        public static string GetSnapToColName() {
            return snapToColName;
        }
        public static string GetSnapRemoveFromColName() {
            return snapRemoveFromColName;
        }
    }
}
