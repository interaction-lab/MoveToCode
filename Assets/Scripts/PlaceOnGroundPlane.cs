using UnityEngine;

namespace MoveToCode {
    public class PlaceOnGroundPlane : MonoBehaviour {
        float SpeedMF { get; } = 1.5f; // normalized per second
        float DistThreshold { get; } = 0.05f; // 5 cm
        void FixedUpdate() {
            CheckForAndPlaceOnGround();
        }

        void CheckForAndPlaceOnGround() {
            RaycastHit rayHitData;
            LayerMask lm = 1 << LayerMask.NameToLayer(LayerMaskConstants.SPATIALAWARENESS);
            Vector3 rayOrigin = transform.position;
            if (Physics.Raycast(rayOrigin, Vector3.down, out rayHitData, 10, lm)) {
                MoveToGroundPlane(rayHitData.transform, Vector3.down);
            }
            else if (Physics.Raycast(rayOrigin, Vector3.up, out rayHitData, 10, lm)) { // this might cause lots of issues
                MoveToGroundPlane(rayHitData.transform, Vector3.up);
            }
        }
        void MoveToGroundPlane(Transform groundTransform, Vector3 direction) {
            if (Vector3.Distance(transform.position, groundTransform.position) > DistThreshold) {
                transform.position = transform.position + direction * SpeedMF * Time.fixedDeltaTime; // changes rate based on frame rate
            }
        }
    }
}