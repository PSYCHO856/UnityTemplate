using System;
using System.Collections;
using UnityEngine;

namespace Watermelon
{
    [Serializable]
    public class AudioCase
    {
        public delegate void AudioCallback();

        public AudioSource source;

        public AudioController.AudioType type;
        private Coroutine endCoroutine;

        public AudioCallback onAudioEnded;

        public AudioCase(AudioClip clip, AudioSource source, AudioController.AudioType type,
            AudioCallback callback = null)
        {
            this.source = source;
            this.type = type;

            this.source.clip = clip;
        }

        public AudioCase OnComplete(AudioCallback callback)
        {
            onAudioEnded = callback;

            endCoroutine = Tween.InvokeCoroutine(OnAudioEndCoroutine(source.clip.length));

            return this;
        }

        public virtual void Play()
        {
            source.Play();
        }

        public void Stop()
        {
            source.Stop();

            if (endCoroutine != null)
                Tween.StopCustomCoroutine(endCoroutine);
        }

        public void FadeOut(float value, float time, bool stop = false)
        {
            var tweenCase = source.DOVolume(value, time);

            if (stop)
                tweenCase.OnComplete(delegate { source.Stop(); });
        }

        public void FadeIn(float value, float time)
        {
            source.DOVolume(value, time);
        }

        private IEnumerator OnAudioEndCoroutine(float clipDuration)
        {
            yield return new WaitForSeconds(clipDuration);

            onAudioEnded.Invoke();
        }
    }
}

// -----------------
// Audio Controller v 0.3
// -----------------