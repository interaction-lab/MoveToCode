using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class OnScreenPlayCodeButton : MonoBehaviour {
        void Update(){
            if (MazeManager.instance.IsLocked) {
                                // make sure my parent is active
                transform.parent.gameObject.SetActive(true);
            }
        }
    }

}
