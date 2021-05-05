using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    [RequireComponent(typeof(AudioSource))]
    public class VirtualKuriAudio : MonoBehaviour {
        public static AudioClip iLoveYouAudioClip, greetingAudioClip, yippeAudioClip, bangDownAudioClip, fartAudioClip, ponderSadAudioClip;
        AudioSource aos;

        private void Awake() {
            aos = GetComponent<AudioSource>();
            iLoveYouAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.KuriILoveYouSound);
            greetingAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.KuriGreetingSound);
            yippeAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.KuriYippeSound);

            bangDownAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.KuriBangDownSound);
            fartAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.KuriFart);
            ponderSadAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.KuriPonderSad);
        }

        public void PlayILoveYou() {
            PlayKuriAduioClip(iLoveYouAudioClip);
        }

        public void PlayGreeting() {
            PlayKuriAduioClip(greetingAudioClip);
        }

        public void PlayYippe() {
            PlayKuriAduioClip(yippeAudioClip);
        }

        public void PlayBangDown() {
            PlayKuriAduioClip(bangDownAudioClip);
        }

        public void PlayFart() {
            PlayKuriAduioClip(fartAudioClip);
        }

        public void PlayPonderSad() {
            PlayKuriAduioClip(ponderSadAudioClip);
        }

        public void PlayKuriAduioClip(AudioClip ac) {
            aos.PlayOneShot(ac);
        }
    }
}