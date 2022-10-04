using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    // Need to swith the job of ViewPortManager to just the following:
    // 1. Spawn/hold ArrowPointPrefabs
    // 2. Enable/disable arrowpointers
    // 3. Know what arrow is currently active (only 1 at a time)
    // to do this, I need to put the object into ArrowPointPrefab
    public class ViewPortManager : Singleton<ViewPortManager> {
        #region members
        Dictionary<Transform, ArrowPointPrefab> gameObjToArrowPointDict = new Dictionary<Transform, ArrowPointPrefab>();
        public static ArrowPointPrefab ActiveArrowPoint = null;
        #endregion
        #region unity
        #endregion
        #region public
        // needs methods for spawning ArrowPointPrefabs
        public ArrowPointPrefab SpawnNewArrowPoint(Transform target, Vector3 offSet, Color outerColor, Color innerColor, string text) {
            ArrowPointPrefab newArrowPoint = Instantiate(Resources.Load<ArrowPointPrefab>(ResourcePathConstants.ArrowPointPrefab), transform);
            newArrowPoint.Set(target.transform, offSet, outerColor, innerColor, text);
            gameObjToArrowPointDict.Add(target, newArrowPoint);
            return newArrowPoint;
        }

        public void TurnOffArrow(Transform target) {
            gameObjToArrowPointDict[target].TurnOff();
            ActiveArrowPoint = null;
        }
        public void TurnOnArrow(Transform target) {
            if (ActiveArrowPoint != null) {
                ActiveArrowPoint.TurnOff(); // only enable 1 at a time to avoid confusion/overwhelming the player (also avoids double text issues)
            }
            gameObjToArrowPointDict[target].TurnOn();
            ActiveArrowPoint = gameObjToArrowPointDict[target];
        }

        public ArrowPointPrefab GetArrowPoint(Transform t) {
            return gameObjToArrowPointDict[t];
        }
        #endregion
        #region private
        #endregion
    }
}
