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
        public static UnityEvent OnGoalReached;
        #endregion

        #region unity
        private void OnEnable() {
            if (transform.name.Contains("sol")) {
                return; // Hackiest thing ever that defintely won't lead to problems down the road
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

        private void OnTriggerEnter(Collider other) {
            if (other.name == "BKBody" && !Interpreter.instance.CodeIsAtStart()) {
                KuriTextManager.instance.Addline("You win!");
                Particles.Play();
                AudioManager.instance.PlaySoundAtObject(transform, AudioManager.correctAudioClip);
                OnGoalReached.Invoke();
            }
        }
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
