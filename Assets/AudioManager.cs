using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class AudioManager : Singleton<AudioManager> {
        public static AudioClip correctAudioClip, incorrectAudioClip, poofAudioClip, popAudioClip, snapAudioClip, spwanAudioClip;

        void Awake() {
            correctAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.CorrectSound);
            incorrectAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.IncorrectSound);
            poofAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.PoofSound);
            popAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.PopSound);
            snapAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.SnapSound);
            spwanAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.SpawnSound);
        }

        public void PlaySoundAtObject(Transform tran, AudioClip ac) {
            PlaySoundAtObject(tran.gameObject, ac);
        }

        public void PlaySoundAtObject(GameObject go, AudioClip ac) {
            AudioSource aos = go.GetComponent<AudioSource>();
            if (aos == null) {
                aos = go.AddComponent<AudioSource>();
            }
            StartCoroutine(NextFrame(aos, ac));
        }

        IEnumerator NextFrame(AudioSource aos, AudioClip ac) {
            yield return new WaitForEndOfFrame();
            Debug.Log(ac);
            aos.PlayOneShot(ac, 1.0f);
            Debug.Log("played");
        }
    }
}

