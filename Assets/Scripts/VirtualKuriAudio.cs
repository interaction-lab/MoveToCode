using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    [RequireComponent(typeof(AudioSource))]
    public class VirtualKuriAudio : MonoBehaviour {
        public static AudioClip iLoveYouAudioClip;
        AudioSource aos;

        private void Awake() {
            aos = GetComponent<AudioSource>();
            iLoveYouAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.KuriILoveYouSound);
        }

        public void PlayILoveYou() {
            PlayKuriAduioClip(iLoveYouAudioClip);
        }

        public void PlayKuriAduioClip(AudioClip ac) {
            aos.PlayOneShot(ac);
        }
    }
}