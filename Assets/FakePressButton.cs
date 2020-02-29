using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode
{
    public class FakePressButton : MonoBehaviour
    {
        float scaleFloat = 0.05f;
        AnimationCurve animationCurve;
        Vector3 origLocalPos;

        public void PressButton()
        {
            GetComponent<Interactable>().TriggerOnClick();
            PushIn();
            PlayButtonSound();
        }

        public void PushIn()
        {
            if(animationCurve == null)
            {
                animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.4f, 1), new Keyframe(1, 0));
            }
            StartCoroutine(Push());
        }

        public void PlayButtonSound()
        {
            StartCoroutine(PlayPressSound());
        }

        IEnumerator Push()
        {
            origLocalPos = transform.localPosition;
            float curTime = 0, totalTime = 1f;
            Vector3 backGoal = origLocalPos - Vector3.back * scaleFloat;
            while (curTime < totalTime)
            {
                transform.localPosition = Vector3.Lerp(
                    origLocalPos,
                    backGoal,
                    animationCurve.Evaluate(curTime / totalTime));
                yield return new WaitForSeconds(Time.deltaTime);
                curTime += Time.deltaTime;
            }
            transform.localPosition = origLocalPos;
        }

        IEnumerator PlayPressSound()
        {
            AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.instance.MRTKButtonPress);
            yield return new WaitForSeconds(AudioManager.instance.MRTKButtonPress.length);
            AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.instance.MRTKButtonUnpress);
            yield return new WaitForSeconds(AudioManager.instance.MRTKButtonUnpress.length);
        }
       
    }
}