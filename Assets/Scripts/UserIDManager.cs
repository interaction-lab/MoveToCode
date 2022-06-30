using System;
using UnityEngine;

namespace MoveToCode {
    public class UserIDManager {
        public static string EmbeddedDataForID = "SID";
        static string playerIDBackingVar = "";
        public static string PlayerId {
            get {
                if (playerIDBackingVar == "") {
                    playerIDBackingVar = Guid.NewGuid().ToString();
                }
                return playerIDBackingVar;
            }
        }
        public static string DeviceId {
            get {
                return SystemInfo.deviceUniqueIdentifier;
            }
        }
    }
}
