using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriPhrase {
        public enum USECASE {
            Encouragement,
            Congratulation,
            Other
        };

        public int id;
        public string lyric;
        public USECASE uc;
        public string filepath;

        public void PlayWithAudiosource(AudioSource auds) { 
            AudioClip ac = Resources.Load<AudioClip>(filepath);
            auds.clip = ac;
            auds.Play();
        }

        public KuriPhrase(int identifier, string lyrics, USECASE usecase, string path) {
            id = identifier;
            lyric = lyrics;
            uc = usecase;
            filepath = path; 
        }
    }
}