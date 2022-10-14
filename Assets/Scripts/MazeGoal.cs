using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazeGoal : MonoBehaviour {
        #region members

        ParticleSystem ps;
        ParticleSystem Particles {
            get {
                if (ps == null) {
                    ps = GetComponentInChildren<ParticleSystem>();
                }
                return ps;
            }
        }

        bool IsBKMazePiece {
            get {
                return !transform.name.Contains("sol"); // Hackiest thing ever that defintely won't lead to problems down the road, got to love string comparisons of transform names that will never change for sure
            }
        }
        Interpreter _interpreter;
        Interpreter InterpreterInstance {
            get {
                if (_interpreter == null) {
                    _interpreter = Interpreter.instance;
                }
                return _interpreter;
            }
        }
        MazeManager mm;
        MazeManager MazeManagerInstance {
            get {
                if (mm == null) {
                    mm = MazeManager.instance;
                }
                return mm;
            }
        }
        #endregion

        #region unity
        bool hasBeenInitialized = false;
        private void OnEnable() {
            if (!IsBKMazePiece) {
                return;
            }
            if (!hasBeenInitialized) {
                // this should really be about oncodecompleted
                InterpreterInstance.OnCodeEnd.AddListener(OnCodeEnd);
                InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
                hasBeenInitialized = true;
            }
            Particles.Stop();
            var main = Particles.main;
            main.loop = false;
#if UNITY_EDITOR
            StartCoroutine(RemoveTrashOnNextFrame());

#endif
        }
        #endregion

        #region public
        #endregion

        #region private

#if UNITY_EDITOR
        IEnumerator RemoveTrashOnNextFrame() {
            yield return null;
            yield return null;
            // remove trashbutton canvas so that we don't accidently remove the goal during debugging in the editor
            foreach (Transform t in transform) {
                if (t.name.Contains("Trash")) {
                    Destroy(t.gameObject);
                    break;
                }
            }
        }
#endif
        void OnCodeEnd() {
            if (MazeManagerInstance.ExerciseInFullyCompleteState) {
                KuriTextManager.instance.Addline("Maze completed!");
                Particles.Play();
                AudioManager.instance.PlaySoundAtObject(transform, AudioManager.correctAudioClip);
            }
        }

        private void OnCodeReset() {
            Particles.Stop(); // in case they quickly move to the next exercise
        }
        #endregion
    }
}
