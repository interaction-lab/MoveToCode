
using Microsoft.MixedReality.Toolkit;

namespace MoveToCode {
    public class EyeGazeLoggingManager : Singleton<EyeGazeLoggingManager> {
        static string objectColName = "EyeGazeObject", eyeGazeLocColNameX = "EyeGazeLocationX",
            eyeGazeLocColNameY = "EyeGazeLocationY", eyeGazeLocColNameZ = "EyeGazeLocationZ";

        private void Start() {
            LoggingManager.instance.AddLogColumn(objectColName, "");
            LoggingManager.instance.AddLogColumn(eyeGazeLocColNameX, "");
            LoggingManager.instance.AddLogColumn(eyeGazeLocColNameY, "");
            LoggingManager.instance.AddLogColumn(eyeGazeLocColNameZ, "");
        }

        private void Update() {
            if (CoreServices.InputSystem.EyeGazeProvider.GazeTarget != null) {
                LoggingManager.instance.UpdateLogColumn(objectColName, CoreServices.InputSystem.EyeGazeProvider.GazeTarget.name);
                LoggingManager.instance.UpdateLogColumn(eyeGazeLocColNameX, CoreServices.InputSystem.EyeGazeProvider.HitPosition.x.ToString());
                LoggingManager.instance.UpdateLogColumn(eyeGazeLocColNameY, CoreServices.InputSystem.EyeGazeProvider.HitPosition.y.ToString());
                LoggingManager.instance.UpdateLogColumn(eyeGazeLocColNameZ, CoreServices.InputSystem.EyeGazeProvider.HitPosition.z.ToString());
            }
        }

    }
}