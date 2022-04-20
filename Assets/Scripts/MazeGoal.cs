using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class MazeGoal : MonoBehaviour {
        #region members
        Collider _collider;
        Collider MyCollider {
            get {
                if (_collider == null) {
                    _collider = GetComponent<Collider>();
                }
                return _collider;
            }
        }
        ParticleSystem ps;
        ParticleSystem Particles {
            get {
                if (ps == null) {
                    ps = GetComponentInChildren<ParticleSystem>();
                }
                return ps;
            }
        }

        MazeManager mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (mazeManager == null) {
                    mazeManager = MazeManager.instance;
                }
                return mazeManager;
            }
        }
        #endregion

        #region unity
        bool hasBeenInitialized = false;
        private void OnEnable() {
            if (transform.name.Contains("sol")) {
                return; // Hackiest thing ever that defintely won't lead to problems down the road
            }
            if (!hasBeenInitialized) {
                MazeManagerInstance.OnBKAtGoal.AddListener(OnBKAtGoal);
                hasBeenInitialized = true;
            }
            MyCollider.enabled = true;
            MyCollider.isTrigger = true;
            Particles.Stop();
            var main = Particles.main;
            main.loop = false;
        }

        private void OnDisable() {
            MyCollider.enabled = false;
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnBKAtGoal() {
            KuriTextManager.instance.Addline("Maze completed!");
            Particles.Play();
            AudioManager.instance.PlaySoundAtObject(transform, AudioManager.correctAudioClip);
        }
        #endregion
    }
}
