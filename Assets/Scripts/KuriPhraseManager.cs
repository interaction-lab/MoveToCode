using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace MoveToCode {
    public class KuriPhraseManager : Singleton<KuriPhraseManager> {
        private List<KuriPhrase> phraseList; 
        private List<KuriPhrase> preloadEncouragementList;
        private List<KuriPhrase> preloadCongratulationList;
        private AWSPollyGetter awsPollyGetter;
        private int idCount;
        public bool onlineMode = true;

        private AudioSource audioSource;

        private KuriPhrase empty = new KuriPhrase(-1, "", KuriPhrase.USECASE.Other, "emptyfile");

        void Awake() {
            if (onlineMode) {
                awsPollyGetter = new AWSPollyGetter(onlineMode);
            }
            else {
                awsPollyGetter = new AWSPollyGetter(state: false);
            }
            idCount = 0;
            phraseList = new List<KuriPhrase>();
            StreamReader reader = new StreamReader("Assets/Resources/" + ResourcePathConstants.EncouragementPhrases + ".json"); //TODO filepaths that work post-build 
            preloadEncouragementList = JsonConvert.DeserializeObject<List<KuriPhrase>>(reader.ReadToEnd());

            reader = new StreamReader("Assets/Resources/" + ResourcePathConstants.CongratulationPhrases + ".json");
            preloadCongratulationList = JsonConvert.DeserializeObject<List<KuriPhrase>>(reader.ReadToEnd());

            if (File.Exists("Assets/Resources/" + ResourcePathConstants.CachePhrases + ".json")) {
                reader = new StreamReader("Assets/Resources/" + ResourcePathConstants.CachePhrases + ".json");
                phraseList = JsonConvert.DeserializeObject<List<KuriPhrase>>(reader.ReadToEnd());
            } 
            else { 
                phraseList = new List<KuriPhrase>();
                File.Create("Assets/Resources/" + ResourcePathConstants.CachePhrases + ".json").Dispose();
            }

            audioSource = GetComponent<AudioSource>();
        }

        public KuriPhrase GetPhrase(string lyric) {
            foreach (KuriPhrase kp in phraseList) {
                if(kp.lyric.Equals(lyric)) {
                    return kp;
                }
            }
            if(awsPollyGetter.GetFunctioningState()) {
                string filepath;
                try {
                    filepath = awsPollyGetter.PullPhrase(lyric);
                }
                catch (FileLoadException e) {
                    KuriTextManager ktm = KuriTextManager.instance;
                    ktm.Addline(lyric);
                    return empty; 
                }
                KuriPhrase newPhrase = new KuriPhrase(idCount, lyric, KuriPhrase.USECASE.Other, filepath);
                idCount++;
                AssetDatabase.Refresh(); //IMPORTANT
                phraseList.Add(newPhrase);

                using (StreamWriter sw = new StreamWriter(
                    File.Open("Assets/Resources/" + ResourcePathConstants.CachePhrases + ".json", FileMode.Truncate))) {
                    sw.WriteLine(JsonConvert.SerializeObject(phraseList, Formatting.None));
                    sw.Close();
                };

                return newPhrase;
            }
            else {
                KuriTextManager ktm = FindObjectOfType<KuriTextManager>();
                ktm.Addline(lyric);
                return empty; 
            }
        }

        public KuriPhrase GetPhrase(KuriPhrase.USECASE ucase) {
            if(ucase != KuriPhrase.USECASE.Other) {
                if(ucase == KuriPhrase.USECASE.Congratulation) {
                    return preloadCongratulationList[(int)(Random.value * preloadCongratulationList.Count)];
                    
                }
                else if(ucase == KuriPhrase.USECASE.Encouragement) {
                    return preloadEncouragementList[(int)(Random.value * preloadEncouragementList.Count)];
                }
                else {
                    throw new System.Exception("You have some problem with the KuriPhrase.USECASE enum");
                }
            }
            else {
                Debug.LogWarning("Warning: \"other\" usecase phrase was specifically requested");
                return empty;
            }
        }

        public AudioSource GetAudioSource() {
            return audioSource;
        }
    }
}
