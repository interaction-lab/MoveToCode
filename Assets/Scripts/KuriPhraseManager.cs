using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace MoveToCode {
    public class KuriPhraseManager : Singleton<KuriPhraseManager> {
        private List<KuriPhrase> phrases;
        private List<KuriPhrase> preloadEncouragement;
        private List<KuriPhrase> preloadCongratulation;
        private AWSPollyGetter apg;
        private int idCount; 
        public bool onlineMode = true;
        
        KuriPhrase getPhrase(string lyric) {
            foreach (KuriPhrase kp in phrases) {
                if(kp.lyric.Equals(lyric)) {
                    return kp;
                }
            }
            //if phrase does not exist
            if(apg.isWorking) {
                string filepath = apg.PullPhrase(lyric);
                KuriPhrase newPhrase = new KuriPhrase(idCount, lyric, KuriPhrase.USECASE.Other, filepath);
                idCount++;
                AssetDatabase.Refresh(); //IMPORTANT
                return newPhrase;
            } else {
                throw new System.Exception(); // TODO implement alternative text 
            }
        }

        KuriPhrase getPhrase(KuriPhrase.USECASE ucase) {
            if(ucase != KuriPhrase.USECASE.Other) {
                if(ucase == KuriPhrase.USECASE.Congratulation) {
                    return preloadCongratulation[(int)(Random.value * preloadCongratulation.Count)];
                } else if(ucase == KuriPhrase.USECASE.Encouragement) {
                    return preloadEncouragement[(int)(Random.value * preloadCongratulation.Count)];
                } else {
                    throw new System.Exception("You have some problem with the KuriPhrase.USECASE enum");
                }
            } else {
                Debug.LogWarning("Warning: \"other\" usecase phrase was specifically requested");
                return phrases[0];
            }
        }

        public KuriPhraseManager() {
            if (onlineMode) {
                apg = new AWSPollyGetter(onlineMode);
            } 
            else {
                apg = new AWSPollyGetter(onlineState: false);
            }
            idCount = 0;

            //todo import from json 
            StreamReader reader = new StreamReader("Assets/Resources/Audio/Speech/preloadEncouragement.json");
            preloadEncouragement = JsonUtility.FromJson<List<KuriPhrase>>(reader.ReadToEnd());
        }
        
    }
}
