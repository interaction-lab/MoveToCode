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
        #endregion

        #region unity
        bool hasBeenInitialized = false;
        private void OnEnable() {
            if (!IsBKMazePiece) {
                return; 
            }
            if (!hasBeenInitialized) {
                MazeManager.instance.OnBKGoalEnter.AddListener(OnBKGoalEnter);
                MazeManager.instance.OnBKGoalExit.AddListener(OnBKGoalExit);
                hasBeenInitialized = true;
            }
            Particles.Stop();
            var main = Particles.main;
            main.loop = false;
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnBKGoalEnter() {
            KuriTextManager.instance.Addline("Maze completed!");
            Particles.Play();
            AudioManager.instance.PlaySoundAtObject(transform, AudioManager.correctAudioClip);
        }

        private void OnBKGoalExit() {
            Particles.Stop(); // in case they quickly move to the next exercise
        }
        #endregion
    }
}
