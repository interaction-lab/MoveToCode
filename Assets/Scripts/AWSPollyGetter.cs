using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.Polly;
using Amazon.Polly.Model;
using System.IO;
using System;
using System.Text;

namespace MoveToCode {
    public class AWSPollyGetter : Singleton<AWSPollyGetter> {
        private const string APIPath = "Assets/Resources/PollyAPI1.txt";

        private const string speakstyle = "pitch=\"high\" rate=\"120%\"";
        private VoiceId speakvoice = VoiceId.Kimberly;

        private AmazonPollyClient apc;
        public bool isWorking = true;
        private bool onlineState;

        public AWSPollyGetter(bool onlineState) {
            this.onlineState = onlineState;
            if (onlineState) { 
                StreamReader reader = new StreamReader(APIPath);
                try {
                    apc = new AmazonPollyClient(reader.ReadLine(), reader.ReadLine(), Amazon.RegionEndpoint.USWest1);
                }
                catch (Exception e) {
                    Debug.LogWarning("Error Generating Polly client with credentials: " + e.Message);
                    Debug.Log("Please make sure the API key is correctly placed and formatted");
                    Debug.Log("Please make sure you have internet connection if you want to use online API");
                    isWorking = false;
                }
            } else {
                isWorking = false;
            }
        }

        public string PullPhrase(string lyric) {
            if (onlineState && isWorking) {
                SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest() {
                    TextType = TextType.Ssml,
                    Text = "<speak><prosody " + speakstyle + ">" + lyric + "</prosody></speak>",
                    VoiceId = speakvoice,
                    OutputFormat = OutputFormat.Mp3
                };

                StringBuilder sb = new StringBuilder();
                foreach (char c in lyric) { //TODO stop the spaghetti 
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_') {
                        sb.Append(c);
                    }
                }

                OnlineRequestPolly(sreq, sb.ToString());
                return ResourcePathConstants.SpeechFolder + sb.ToString();
            } else { 
                return "TODO fix states";
            }
        }

        private void OnlineRequestPolly(SynthesizeSpeechRequest sreq, string name) {
            SynthesizeSpeechResponse sres = apc.SynthesizeSpeech(sreq);

            try {
                using (var fileStream = File.Create(@"Assets\Resources\Audio\Speech\" + name + ".mp3")) {
                    sres.AudioStream.CopyTo(fileStream);
                    fileStream.Flush();
                    fileStream.Close();
                    Debug.Log("New speech file created succesfully");
                }
            }
            catch (Exception e) {
                Debug.Log("Error writing speech to file: " + e.Message);
            }
        }

        public bool getFunctioningState() { return isWorking; }
    }
}
