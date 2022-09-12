using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class PlayerTransformManager : TransformManager {
        #region members
        public override Vector3 Forward {
            get {
                return OriginT.forward;
            }
        }
        public override Vector3 Position {
            get {
                return OriginT.position;
            }
            set {
                throw new System.NotImplementedException("Cannot set player position");
            }
        }
		public override Quaternion Rotation {
			get {
				return OriginT.rotation;
			}
			set {
				OriginT.rotation = value;
			}
		}

        TutorKuriTransformManager _kuriTransformManager;
        TutorKuriTransformManager KuriT {
            get {
                if (_kuriTransformManager == null) {
                    _kuriTransformManager = TutorKuriTransformManager.instance;
                }
                return _kuriTransformManager;
            }
        }

        public override float GroundYCord {
            get {
                return KuriT.GroundYCord;
            }
            set {
                KuriT.GroundYCord = value;
            }
        }

        
        #endregion

        #region unity
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
