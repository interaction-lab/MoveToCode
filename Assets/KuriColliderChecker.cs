using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Legacy;
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
            Debug.Log(Vector3.Distance(playerPos, kuriPos));
            if (Vector3.Distance(playerPos, kuriPos) < minDistFromPlayer) {
                MoveAway(PlayerT);
            }
        }
        private void OnCollisionStay(Collision collision) {
            if (TutorKuriManagerInstance.KController.IsDoingAction) {
                return;
            }
            MoveAway(collision.transform);
        }
        #endregion

        #region public
        #endregion

        #region private
        void MoveAway(Transform col) {
            // calculate a position from user to move away to
            if (col == PlayerT) {
                col = MazeManager.instance.BKMazePiece.transform;
            }

            Vector3 playerP = PlayerT.position;
            playerP.y = TutorKuriTransformManager.instance.GroundYCord;
            Vector3 colP = col.position;
            colP.y = TutorKuriTransformManager.instance.GroundYCord;

            // calculate line from PlayerT to col
            Vector3 line = colP - playerP;
            // rotate line left 15 degrees
            line = Quaternion.Euler(0, 15, 0) * line;

            // calculate global position of rotated line
            Vector3 newPos = playerP + line;

            moveAwayTransform.position = newPos;

            // move to that position
            TutorKuriManagerInstance.MoveAway(moveAwayTransform);
        }
        #endregion
    }
}
