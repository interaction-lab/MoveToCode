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

        void Play(AudioSource auds) {
            AudioClip ac = Resources.Load<AudioClip>(filepath);
            auds.clip = ac;
            auds.Play();
        }

        public KuriPhrase(int id, string lyric, USECASE uc, string filepath) {
            this.id = id;
            this.lyric = lyric;
            this.uc = uc;
            this.filepath = filepath; 
        }
    }
}