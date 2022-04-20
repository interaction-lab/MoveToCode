using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class AlwaysFaceUser : MonoBehaviour {
        #region members
        Transform user;
        Transform User {
            get {
                if (user == null) {
                    user = Camera.main.transform;
                }
                return user;
            }
        }
        #endregion

        #region unity
        void Update() {
            transform.LookAt(User);
        }
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
