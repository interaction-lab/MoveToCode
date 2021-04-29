using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class OptionSelectionManager : Singleton<OptionSelectionManager> {
        public bool logData = true;
        public bool usePhysicalKuri = true;


        void Awake() {
#if UNITY_EDITOR
            string options = string.Join("", "OptionSelectionManager options:\n1. logData: ", logData, "\n2.usePhysicalKuri: ", usePhysicalKuri);
            Debug.Log(options);
            LoggingManager.instance.logData = logData;
            KuriManager.instance.usePhysicalKuri = usePhysicalKuri;
            if (!usePhysicalKuri) {
                Debug.Log("Rosconnector will still attempt to connect, this should not cause errors in non-ROS scripts.");
            }
#elif UNITY_IOS || UNITY_ANDROID
            LoggingManager.instance.logData = true;
            KuriManager.instance.usePhysicalKuri = false;
#else
            LoggingManager.instance.logData = true;
            KuriManager.instance.usePhysicalKuri = true;
#endif
        }
    }
}
