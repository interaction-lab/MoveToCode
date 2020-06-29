using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace MoveToCode {
    public class KuriPhraseManager : Singleton<KuriPhraseManager> {
        private List<KuriPhrase> phrases; 
        private List<KuriPhrase> preloadEncouragement;
        private List<KuriPhrase> preloadCongratulation;
        private AWSPollyGetter apg;
        private int idCount;
        public bool onlineMode = true;


        [HideInInspector] public AudioSource audioSource;

        private KuriPhrase empty = new KuriPhrase(-1, "", KuriPhrase.USECASE.Other, "emptyfile");
        
        public KuriPhrase GetPhrase(string lyric) {
            foreach (KuriPhrase kp in phrases) {
                if(kp.lyric.Equals(lyric)) {
                    return kp;
                }
            }
            //if phrase does not exist yet
            if(apg.isWorking) {
                string filepath;
                try {
                    filepath = apg.PullPhrase(lyric);
                } catch(FileLoadException e) {
                    KuriTextManager ktm = FindObjectOfType<KuriTextManager>();
                    ktm.Addline(lyric);
                    return empty; 
                }
                KuriPhrase newPhrase = new KuriPhrase(idCount, lyric, KuriPhrase.USECASE.Other, filepath);
                idCount++;
                AssetDatabase.Refresh(); //IMPORTANT
                phrases.Add(newPhrase);

                using (StreamWriter sw = new StreamWriter(
                    File.Open("Assets/Resources/" + ResourcePathConstants.SpeechCacheFolder + "cachePhrase.json", FileMode.Truncate))) {
                    sw.WriteLine(JsonConvert.SerializeObject(phrases, Formatting.None));
                    sw.Close();
                };

                return newPhrase;
            } else {
                KuriTextManager ktm = FindObjectOfType<KuriTextManager>();
                ktm.Addline(lyric);
                return empty; 
            }
        }

        public KuriPhrase GetPhrase(KuriPhrase.USECASE ucase) {
            if(ucase != KuriPhrase.USECASE.Other) {
                if(ucase == KuriPhrase.USECASE.Congratulation) {
                    int temp = (int)(Random.value * preloadCongratulation.Count);
                    Debug.Log(temp);
                    return preloadCongratulation[temp];
                    
                } else if(ucase == KuriPhrase.USECASE.Encouragement) {
                    return preloadEncouragement[(int)(Random.value * preloadEncouragement.Count)];
                } else {
                    throw new System.Exception("You have some problem with the KuriPhrase.USECASE enum");
                }
            } else {
                Debug.LogWarning("Warning: \"other\" usecase phrase was specifically requested");
                return empty;
            }
        }

        void Awake() { 
            if (onlineMode) {
                apg = new AWSPollyGetter(onlineMode);
            }
            else {
                apg = new AWSPollyGetter(onlineState: false);
            }
            idCount = 0;
            phrases = new List<KuriPhrase>(); 
            StreamReader reader = new StreamReader("Assets/Resources/" + ResourcePathConstants.SpeechFolder + "preloadEncouragement.json"); 
            //TODO filepaths that work post-build 
            preloadEncouragement = JsonConvert.DeserializeObject<List<KuriPhrase>>(reader.ReadToEnd());

            reader = new StreamReader("Assets/Resources/" + ResourcePathConstants.SpeechFolder + "preloadCongratulation.json");
            preloadCongratulation = JsonConvert.DeserializeObject<List<KuriPhrase>>(reader.ReadToEnd());

            try {
                reader = new StreamReader("Assets/Resources/" + ResourcePathConstants.SpeechCacheFolder + "cachePhrase.json");
                phrases = JsonConvert.DeserializeObject<List<KuriPhrase>>(reader.ReadToEnd());
            } catch(FileNotFoundException e) { // no cached phrases
                phrases = new List<KuriPhrase>();
            }

            audioSource = GetComponent<AudioSource>();
        }
        
    }
}
