using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class AlwaysFaceUser : MonoBehaviour {
        #region members
        public bool ReverseIt = false;
        Transform user;
        Transform User {
            get {
                if (user == null) {
                    user = Camera.main.transform;
                }
                return user;
            }
        }
        public float offsetInDeg;
        #endregion

        #region unity
        void Update() {
            transform.LookAt(User);
            transform.Rotate(offsetInDeg, (ReverseIt ? 180 : 0), 0);
        }
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
