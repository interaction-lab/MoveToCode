using UnityEngine;

namespace MoveToCode {
    public class AudioManager : Singleton<AudioManager> {
        public static AudioClip correctAudioClip, incorrectAudioClip, poofAudioClip, popAudioClip, snapAudioClip, spwanAudioClip;
        public AudioClip MRTKButtonPress, MRTKButtonUnpress;
        static string audioLogCol = "AudioPlayed";

        void Awake() {
            correctAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.CorrectSound);
            incorrectAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.IncorrectSound);
            poofAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.PoofSound);
            popAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.PopSound);
            snapAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.SnapSound);
            spwanAudioClip = Resources.Load<AudioClip>(ResourcePathConstants.SpawnSound);
            LoggingManager.instance.AddLogColumn(audioLogCol, "");
        }

        public void PlaySoundAtObject(Transform tran, AudioClip ac) {
            PlaySoundAtObject(tran.gameObject, ac);
        }

        public void PlayButtonClick() {
            PlaySoundAtObject(ScreenMenuManager.instance.transform, MRTKButtonPress);
        }

        public void PlayReleaseClick() {
            PlaySoundAtObject(ScreenMenuManager.instance.transform, MRTKButtonUnpress);
        }

        public void PlaySoundAtObject(GameObject go, AudioClip ac) {
            AudioSource aos = go.GetComponent<AudioSource>();
            if (aos == null) {
                aos = go.AddComponent<AudioSource>();
                aos.spatialize = true;
                aos.spatialBlend = 1.0f;
            }
            aos.PlayOneShot(ac, 0.5f);
            LoggingManager.instance.UpdateLogColumn(audioLogCol, ac.name);
        }
    }
}

