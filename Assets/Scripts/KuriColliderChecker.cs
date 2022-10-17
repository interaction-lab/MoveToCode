using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriColliderChecker : Singleton<KuriColliderChecker> {
        #region members
        Transform playerT;
        Transform PlayerT {
            get {
                if (playerT == null) {
                    playerT = Camera.main.transform;
                }
                return playerT;
            }
        }
        float minDistFromPlayer = 0.75f;

        Transform mt;
        Transform moveAwayTransform {
            get {
                if (mt == null) {
                    mt = MoveAwayFromMazeObj.instance.transform;
                }
                return mt;
            }
        }

        TutorKuriManager tutorKuriManager;
        TutorKuriManager TutorKuriManagerInstance {
            get {
                if (tutorKuriManager == null) {
                    tutorKuriManager = TutorKuriManager.instance;
                }
                return tutorKuriManager;
            }
        }

        #endregion

        #region unity

        private void FixedUpdate() {
            // check if doing action first
            if (TutorKuriManagerInstance.KController.IsDoingAction) {
                return;
            }

            // check distance from player
            Vector3 playerPos = PlayerT.position;
            playerPos.y = 0;
            Vector3 kuriPos = transform.position;
            kuriPos.y = 0;
            if (Vector3.Distance(playerPos, kuriPos) < minDistFromPlayer) {
                MoveAway(PlayerT);
            }
        }
        private void OnTriggerStay(Collider other) {
            if (TutorKuriManagerInstance.KController.IsDoingAction) {
                return;
            }
            MoveAway(other.transform);
        }

        #endregion

        #region public
        #endregion

        #region private

        private bool IsLeftOfLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
            return ((lineEnd.x - lineStart.x) * (point.z - lineStart.z) - (lineEnd.z - lineStart.z) * (point.x - lineStart.x)) > 0;
        }
        void MoveAway(Transform col) {
            // calculate a position from user to move away to
            if (col == PlayerT) {
                // move MoveAwayTarget 1 meter in front of the player and use that
                Vector3 forwardVec = PlayerT.forward;
                forwardVec.y = 0;
                forwardVec.Normalize();
                moveAwayTransform.position = PlayerT.position + forwardVec;
                col = moveAwayTransform;
            }

            Vector3 playerP = PlayerT.position;
            playerP.y = TutorKuriTransformManager.instance.GroundYCord;
            Vector3 colP = col.position;
            colP.y = TutorKuriTransformManager.instance.GroundYCord;



            // calculate line from PlayerT to col
            Vector3 line = (colP - playerP).normalized;

            // calculate line of player forward vector
            Vector3 playerForwardLine = PlayerT.forward;
            playerForwardLine.y = 0;
            playerForwardLine.Normalize();

            // calculate average of the two lines
            Vector3 avgLine = (line + playerForwardLine).normalized;


            // check if Kuri is to the left or to the right
            Vector3 kuriP = TutorKuriTransformManager.instance.Position.normalized;
            float angle = IsLeftOfLine(playerP, colP, kuriP) ? -20 : 20;

            line = Quaternion.Euler(0, angle, 0) * avgLine;

            // calculate global position of rotated line
            Vector3 newPos = playerP + avgLine * 2.0f;

            moveAwayTransform.position = newPos;

            // move to that position
            TutorKuriManagerInstance.MoveAway(moveAwayTransform);
        }
        #endregion
    }
}
