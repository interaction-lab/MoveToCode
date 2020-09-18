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
        private string APIPath = "Assets/Resources/" + ResourcePathConstants.AWSPollyAPI + ".txt";

        private const string speakstyle = "pitch=\"x-high\" rate=\"100%\"";
        private VoiceId speakvoice = VoiceId.Joanna;

        private AmazonPollyClient apc;
        private bool isServerWorking = true;
        private bool onlineStateIn;

        public AWSPollyGetter(bool state) {
            onlineStateIn = state;
            if (onlineStateIn) { 
                StreamReader reader = new StreamReader(APIPath);
                try {
                    apc = new AmazonPollyClient(reader.ReadLine(), reader.ReadLine(), regions[reader.ReadLine()]);
                }
                catch (Exception e) {
                    Debug.LogWarning("Error Generating Polly client with credentials: " + e.Message);
                    Debug.Log("Please make sure the API key is correctly placed and formatted");
                    Debug.Log("Please make sure you have internet connection if you want to use online API");
                    isServerWorking = false;
                }
            }
            else {
                isServerWorking = false;
            }
        }

        public string PullPhrase(string lyric) {
            if (isServerWorking) {
                SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest() {
                    TextType = TextType.Ssml,
                    Text = "<speak><prosody " + speakstyle + ">" + lyric + "</prosody></speak>",
                    VoiceId = speakvoice,
                    OutputFormat = OutputFormat.Mp3
                };

                string filename = getFilename(lyric);

                try {
                    OnlineRequestPolly(sreq, filename);
                }
                catch (Exception e) {
                    throw new FileLoadException();
                }

                return ResourcePathConstants.SpeechCacheFolder + filename;
            }
            else {
                throw new FileLoadException();
            }
        }

        private void OnlineRequestPolly(SynthesizeSpeechRequest sreq, string name) {
            SynthesizeSpeechResponse sres = apc.SynthesizeSpeech(sreq);
            using (FileStream fileStream = File.Create("Assets/Resources/" + ResourcePathConstants.SpeechCacheFolder + name + ".mp3")) {
                sres.AudioStream.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
                Debug.Log("New speech file created succesfully");
            }
        }

        public bool GetFunctioningState() { return isServerWorking; }

        private static string getFilename(string lyric) {
            StringBuilder sb = new StringBuilder();
            foreach (char c in lyric) {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private Dictionary<string, Amazon.RegionEndpoint> regions = new Dictionary<string, Amazon.RegionEndpoint>() {
            {"USWest1", Amazon.RegionEndpoint.USWest1},
            {"USWest2", Amazon.RegionEndpoint.USWest2},
            {"USEast1", Amazon.RegionEndpoint.USEast1},
            {"USEast2", Amazon.RegionEndpoint.USEast2},
            {"CACentral1", Amazon.RegionEndpoint.CACentral1},
            {"EUCentral1", Amazon.RegionEndpoint.EUCentral1},
            {"EUWest1", Amazon.RegionEndpoint.EUWest1 }
        };

    }
}
