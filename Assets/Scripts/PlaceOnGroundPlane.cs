using UnityEngine;

namespace MoveToCode {
    public class PlaceOnGroundPlane : MonoBehaviour {
        #region members
        float SpeedMF { get; } = 1.5f; // normalized per second
        float DistThreshold { get; } = 0.05f; // 5 cm
        TutorKuriTransformManager tktm;
        TutorKuriTransformManager TKTransformManager {
            get {
                if (tktm == null) {
                    tktm = transform.parent.GetComponent<TutorKuriTransformManager>(); // flimsy and should get from the TutorKuriManager but oh well
                }
                return tktm;
            }
        }
        Transform groundPlane;
        #endregion

        #region unity
        #endregion
        #region public

        public Transform GetGroundPlane() {
            RaycastHit rayHitData;
            LayerMask lm = 1 << LayerMask.NameToLayer(LayerMaskConstants.SPATIALAWARENESS);
            Vector3 rayOrigin = transform.position;
            if (Physics.Raycast(rayOrigin, Vector3.down, out rayHitData, 10, lm)) {
                groundPlane = rayHitData.transform;
            }
            else if(Physics.Raycast(rayOrigin, Vector3.up, out rayHitData, 10, lm)){ // likely to cause issues
                groundPlane = rayHitData.transform;
            }
            return groundPlane; // note this can be null if no ground plane is found the first time it is called
        }
        #endregion
    }
}