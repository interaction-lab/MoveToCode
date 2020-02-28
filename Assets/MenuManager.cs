using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MenuManager : Singleton<MenuManager> {
        PressableButtonHoloLens2 playButton;

        static Material fakeButtonPressParticleMaterial;

        private void Awake() {
            fakeButtonPressParticleMaterial = Resources.Load<Material>(ResourcePathConstants.FakeButtonPressParticleMaterial);
        }

        public PressableButtonHoloLens2 GetPlayButton() {
            if (playButton == null) {
                playButton = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<PressableButtonHoloLens2>();
            }
            return playButton;
        }

        public void FakePressPlay() {
            GetPlayButton().FakePressHololens2Button();
            ParticleSystem ps = GetPlayButton().GetComponent<ParticleSystem>();
            if (ps == null) {
                ps = GetPlayButton().gameObject.AddComponent<ParticleSystem>();
                ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
                psr.material = fakeButtonPressParticleMaterial;
                var main = ps.main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }
            StartCoroutine(FakeButtonPress(GetPlayButton().gameObject, ps));
        }

        IEnumerator FakeButtonPress(GameObject gameObject, ParticleSystem ps) {
            ps.Play();
            AudioManager.instance.PlaySoundAtObject(GetPlayButton().gameObject, AudioManager.instance.MRTKButtonPress);
            yield return new WaitForSeconds(AudioManager.instance.MRTKButtonPress.length);
            AudioManager.instance.PlaySoundAtObject(GetPlayButton().gameObject, AudioManager.instance.MRTKButtonUnpress);
            yield return new WaitForSeconds(AudioManager.instance.MRTKButtonUnpress.length);
            yield return new WaitForSeconds(1f); //extra padded time for ps
            ps.Stop();
        }
    }
}
