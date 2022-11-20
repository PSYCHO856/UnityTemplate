using System;
using UnityEngine;

namespace Watermelon
{
    [SetupTab("Audio", texture = "icon_audio")]
    [CreateAssetMenu(fileName = "Audio Settings", menuName = "Settings/Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        public bool IsMusicEnabled = true;
        public bool IsAudioEnabled = true;
        public bool IsVibrationEnabled = true;

        public AudioClip[] MusicAudioClips;

        public Sounds Sounds;
        public Vibrations Vibrations;
    }

    [Serializable]
    public class Sounds
    {
        public AudioClip Bottle;
        public AudioClip[] Pour;
        public AudioClip FillUp;
        public AudioClip Victory;
        public AudioClip Click;
        public AudioClip Open;
        public AudioClip Close;
        public AudioClip Random;
        public AudioClip RandomResult;
        public AudioClip Equip;
        public AudioClip PopUp;
        public AudioClip Withdrawal;
        public AudioClip PourNegeative;
        public AudioClip Fail;
    }

    [Serializable]
    public class Vibrations
    {
        public int ShortVibration;
        public int LongVibration;
    }
}

// -----------------
// Audio Controller v 0.3
// -----------------