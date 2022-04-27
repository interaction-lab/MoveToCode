using System.Collections;
using System.Collections.Generic;
using System; // Exceptions
using UnityEngine;
#if PLATFORM_IOS
using UnityEngine.iOS;
using UnityEngine.Apple.ReplayKit;
#endif

namespace MoveToCode {
    public class RecordingManager : MonoBehaviour {
        #region members
        #endregion

        #region unity
        private void Awake() {
#if !PLATFORM_IOS
            Destroy(gameObject); // destroy if not on iOS
#endif
#if PLATFORM_IOS
            try {
                ReplayKit.ReplayKit.StartRecording(true, true);
            }
            catch (Exception e) {
                Debug.Log(e.Message);
            }
#endif
        }

        private void OnApplicationQuit() {
#if PLATFORM_IOS
            try {
                ReplayKit.ReplayKit.StopRecording();
            }
            catch (Exception e) {
                Debug.Log(e.Message);
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
