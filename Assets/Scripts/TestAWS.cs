using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.Polly;
using Amazon.Polly.Model;
using System.IO;
using System;
using System.Text;
using MoveToCode;
using UnityEditor;

public class TestAWS : MonoBehaviour
{
    private AmazonPollyClient apc;
    private SynthesizeSpeechResponse sres;
    public String whatToSay;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        
        try {
            apc = new AmazonPollyClient("AKIAUTHIP3ELD5TBOOHO", "TZdPzUStylOlbqSI10YidqMRSta+bInm2Z69Wx7t", Amazon.RegionEndpoint.USWest1);
        } catch (Exception e) {
            Debug.Log("Error Generating Polly client with credentials: " + e.Message);
        }

        SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest() {
            TextType = TextType.Ssml,
            Text = "<speak><prosody pitch=\"high\" rate=\"120 % \">" + whatToSay + "</prosody></speak>",
            VoiceId = VoiceId.Kimberly,
            OutputFormat = OutputFormat.Mp3
        };
        //TODO simplify try catches
        try {
            sres = apc.SynthesizeSpeech(sreq);
        } catch(Exception e) {
            Debug.Log("Error getting speech response from client: " + e.Message);
        }

        StringBuilder sb = new StringBuilder();
        foreach (char c in sreq.Text) {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_') {
                sb.Append(c);
            }
        }

        try {
            using (var fileStream = File.Create(@"Assets\Resources\Audio\Speech\" + sb.ToString() + ".mp3")) {
                sres.AudioStream.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
                Debug.Log("File created succesfully");
            }
        } catch(Exception e) {
            Debug.Log("Error writing speech to file: " + e.Message);
        }

        
        StartCoroutine(Ffff(audioSource, sb.ToString()));

    }

    IEnumerator Ffff(AudioSource aaa, String name) {
        yield return new WaitForSeconds(1);
        AssetDatabase.Refresh(); //IMPORTANT
        Debug.Log("Process start");
        Debug.Log(ResourcePathConstants.SpeechFolder + name);
        AudioClip audioClip = Resources.Load<AudioClip>(ResourcePathConstants.SpeechFolder + name);
        aaa.clip = audioClip;
        Debug.Log(audioClip == null);
        aaa.Play();
        Debug.Log("speech sound played");
    }

}
