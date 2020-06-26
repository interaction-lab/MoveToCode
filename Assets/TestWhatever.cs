using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class TestWhatever : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            KuriPhraseManager kpm = FindObjectOfType<KuriPhraseManager>();

            kp = kpm.GetPhrase("Yes this works!");
            kp.Play(kpm.audioSource);

        }


    }
}
