using UnityEngine;

namespace MobileKit
{
    [System.Serializable]
    public class AudioCaseCustom : AudioCase
    {
        public bool autoRelease = false;

        public AudioCaseCustom(AudioClip clip, AudioSource source, AudioManager.AudioType type, bool autoRelease) : base(clip, source, type)
        {
            this.autoRelease = autoRelease;
        }

        public void SetSourceTypeDefaultSettings(AudioManager.AudioType type)
        {
            AudioManager.SetSourceDefaultSettings(source, type);
        }

        public void ReleaseCustomSource(float fadeTime = 0)
        {
            AudioManager.ReleaseCustomSource(this, fadeTime);
        }

        public override void Play()
        {
            if (source.clip != null)
            {
                source.Play();
            }
            else
            {
                Debug.Log("Audio clip is null");
            }
        }

        public void Play(AudioClip audioClip)
        {
            source.clip = audioClip;
            source.Play();
        }
    }
}

// -----------------
// Audio Controller v 0.3.1
// -----------------