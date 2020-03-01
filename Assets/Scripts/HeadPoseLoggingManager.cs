using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class HeadPoseLoggingManager : Singleton<HeadPoseLoggingManager> {
        static string headPoseLocColNameX = "HeadPoseX",
           headPoseLocColNameY = "HeadPoseY", headPoseLocColNameZ = "HeadPoseZ";

        Transform headTransform;

        private void Start() {
            LoggingManager.instance.AddLogColumn(headPoseLocColNameX, "");
            LoggingManager.instance.AddLogColumn(headPoseLocColNameY, "");
            LoggingManager.instance.AddLogColumn(headPoseLocColNameZ, "");
            headTransform = Camera.main.transform;
        }

        private void Update() {
            LoggingManager.instance.UpdateLogColumn(headPoseLocColNameX, headTransform.position.x.ToString());
            LoggingManager.instance.UpdateLogColumn(headPoseLocColNameY, headTransform.position.y.ToString());
            LoggingManager.instance.UpdateLogColumn(headPoseLocColNameZ, headTransform.position.z.ToString());
        }
    }
}

