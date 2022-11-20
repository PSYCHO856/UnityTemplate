﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Watermelon
{
    public class AudioController
    {
        public enum AudioType
        {
            Music = 0,
            Sound = 1
        }

        private const int AUDIO_SOURCES_AMOUNT = 4;

        private static AudioController instance;
        private readonly List<AudioCaseCustom> activeCustomSourcesCases = new();
        private readonly List<AudioSource> activeMusic = new();

        private readonly List<AudioSource> activeSounds = new();

        private AudioSettings audioSettings;

        private readonly List<AudioSource> audioSources = new();

        private readonly List<AudioSource> customSources = new();
        private int lastSourceID;

        private GameObject targetGameObject;

        public static AudioSettings AudioSettings
        {
            get
            {
#if UNITY_EDITOR
                return RuntimeEditorUtils.GetAssetByName<AudioSettings>("Audio Settings");
#else
                return instance.audioSettings;
#endif
            }
        }

        public void Init(AudioSettings audioSettings, GameObject targetGameObject)
        {
            if (instance != null)
            {
                Debug.Log("[Audio Controller]: Module already exists!");

                return;
            }

            instance = this;

            this.audioSettings = audioSettings;
            this.targetGameObject = targetGameObject;

            if (audioSettings == null)
            {
                Debug.LogError(
                    "[AudioController]: Audio Settings is NULL! Please assign audio settings scriptable on Audio Controller script.");

                return;
            }

            //Create audio source objects
            for (var i = 0; i < AUDIO_SOURCES_AMOUNT; i++) audioSources.Add(CreateAudioSourceObject(false));
        }

        public static void PlayRandomMusic()
        {
            PlayMusic(instance.audioSettings.MusicAudioClips.GetRandomItem());
        }

        /// <summary>
        ///     Stop all active streams
        /// </summary>
        public static void ReleaseStreams()
        {
            ReleaseMusic();
            ReleaseSounds();
            ReleaseCustomStreams();
        }

        /// <summary>
        ///     Releasing all active music.
        /// </summary>
        public static void ReleaseMusic()
        {
            var activeMusicCount = instance.activeMusic.Count - 1;
            for (var i = activeMusicCount; i >= 0; i--)
            {
                instance.activeMusic[i].Stop();
                instance.activeMusic[i].clip = null;
                instance.activeMusic.RemoveAt(i);
            }
        }

        /// <summary>
        ///     Releasing all active sounds.
        /// </summary>
        public static void ReleaseSounds()
        {
            var activeStreamsCount = instance.activeSounds.Count - 1;
            for (var i = activeStreamsCount; i >= 0; i--)
            {
                instance.activeSounds[i].Stop();
                instance.activeSounds[i].clip = null;
                instance.activeSounds.RemoveAt(i);
            }
        }

        /// <summary>
        ///     Releasing all active custom sources.
        /// </summary>
        public static void ReleaseCustomStreams()
        {
            var activeStreamsCount = instance.activeCustomSourcesCases.Count - 1;
            for (var i = activeStreamsCount; i >= 0; i--)
                if (instance.activeCustomSourcesCases[i].autoRelease)
                {
                    var source = instance.activeCustomSourcesCases[i].source;
                    instance.activeCustomSourcesCases[i].source.Stop();
                    instance.activeCustomSourcesCases[i].source.clip = null;
                    instance.activeCustomSourcesCases.RemoveAt(i);
                    instance.customSources.Add(source);
                }
        }

        public static void StopStream(AudioCase audioCase, float fadeTime = 0)
        {
            if (audioCase.type == AudioType.Sound)
                instance.StopSound(audioCase.source, fadeTime);
            else
                instance.StopMusic(audioCase.source, fadeTime);
        }

        public static void StopStream(AudioCaseCustom audioCase, float fadeTime = 0)
        {
            ReleaseCustomSource(audioCase, fadeTime);
        }

        private void StopSound(AudioSource source, float fadeTime = 0)
        {
            var streamID = activeSounds.FindIndex(x => x == source);
            if (streamID != -1)
            {
                if (fadeTime == 0)
                {
                    activeSounds[streamID].Stop();
                    activeSounds[streamID].clip = null;
                    activeSounds.RemoveAt(streamID);
                }
                else
                {
                    activeSounds[streamID].DOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        activeSounds.Remove(source);
                        source.Stop();
                    });
                }
            }
        }

        private void StopMusic(AudioSource source, float fadeTime = 0)
        {
            var streamID = activeMusic.FindIndex(x => x == source);
            if (streamID != -1)
            {
                if (fadeTime == 0)
                {
                    activeMusic[streamID].Stop();
                    activeMusic[streamID].clip = null;
                    activeMusic.RemoveAt(streamID);
                }
                else
                {
                    activeMusic[streamID].DOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        activeMusic.Remove(source);
                        source.Stop();
                    });
                }
            }
        }

        private static void AddMusic(AudioSource source)
        {
            if (!instance.activeMusic.Contains(source)) instance.activeMusic.Add(source);
        }

        private static void AddSound(AudioSource source)
        {
            if (!instance.activeSounds.Contains(source)) instance.activeSounds.Add(source);
        }


        public static void PlayMusic(AudioClip clip, float volumePercentage = 1.0f)
        {
            // if (!instance.audioSettings.IsMusicEnabled)
            // return;

            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            var source = instance.GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Music);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.Play();

            AddMusic(source);
        }

        public static AudioCase PlaySmartMusic(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            // if (!instance.audioSettings.IsMusicEnabled)
            // return null;

            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            var source = instance.GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Music);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;

            var audioCase = new AudioCase(clip, source, AudioType.Music);

            audioCase.Play();

            AddMusic(source);

            return audioCase;
        }


        public static void PlaySound(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            // if (!instance.audioSettings.IsAudioEnabled)
            // return;

            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            var source = instance.GetAudioSource();

            SetSourceDefaultSettings(source);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;
            source.Play();

            AddSound(source);
        }

        public static AudioCase PlaySmartSound(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            // if (!instance.audioSettings.IsAudioEnabled)
            // return null;

            if (clip == null)
                Debug.LogError("[AudioController]: Audio clip is null");

            var source = instance.GetAudioSource();

            SetSourceDefaultSettings(source);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;

            var audioCase = new AudioCase(clip, source, AudioType.Sound);
            audioCase.Play();

            AddSound(source);

            return audioCase;
        }

        public static AudioCaseCustom GetCustomSource(bool autoRelease, AudioType audioType)
        {
            AudioSource source = null;

            if (!instance.customSources.IsNullOrEmpty())
            {
                source = instance.customSources[0];
                instance.customSources.RemoveAt(0);
            }
            else
            {
                source = instance.CreateAudioSourceObject(true);
            }

            SetSourceDefaultSettings(source, audioType);

            var audioCase = new AudioCaseCustom(null, source, audioType, autoRelease);

            instance.activeCustomSourcesCases.Add(audioCase);

            return audioCase;
        }

        public static void ReleaseCustomSource(AudioCaseCustom audioCase, float fadeTime = 0)
        {
            var streamID = instance.activeCustomSourcesCases.FindIndex(x => x.source == audioCase.source);
            if (streamID != -1)
            {
                if (fadeTime == 0)
                {
                    instance.activeCustomSourcesCases[streamID].source.Stop();
                    instance.activeCustomSourcesCases[streamID].source.clip = null;
                    instance.activeCustomSourcesCases.RemoveAt(streamID);
                    instance.customSources.Add(audioCase.source);
                }
                else
                {
                    instance.activeCustomSourcesCases[streamID].source.DOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        instance.activeCustomSourcesCases.Remove(audioCase);
                        audioCase.source.Stop();
                        instance.customSources.Add(audioCase.source);
                    });
                }
            }
        }

        private AudioSource GetAudioSource()
        {
            var sourcesAmount = audioSources.Count;
            for (var i = 0; i < sourcesAmount; i++)
                if (!audioSources[i].isPlaying)
                    return audioSources[i];

            var createdSource = CreateAudioSourceObject(false);
            audioSources.Add(createdSource);

            return createdSource;
        }

        private AudioSource CreateAudioSourceObject(bool isCustom)
        {
            lastSourceID++;

            var audioSource = targetGameObject.AddComponent<AudioSource>();
            SetSourceDefaultSettings(audioSource);

            return audioSource;
        }

        private void SetVolumeForAudioSources(float volume)
        {
            // setuping all active sound sources
            var activeSoundSourcesCount = activeSounds.Count;
            for (var i = 0; i < activeSoundSourcesCount; i++) activeSounds[i].volume = volume;

            activeSoundSourcesCount = activeMusic.Count;
            for (var i = 0; i < activeSoundSourcesCount; i++) activeMusic[i].volume = volume;

            // setuping all custom sound sources
            activeSoundSourcesCount = activeCustomSourcesCases.Count;
            for (var i = 0; i < activeSoundSourcesCount; i++) activeCustomSourcesCases[i].source.volume = volume;
        }

        public static void SetVolume(float volume)
        {
            GameSettingsPrefs.Set("volume", volume);

            instance.SetVolumeForAudioSources(volume);
        }

        public static float GetVolume()
        {
            return GameSettingsPrefs.Get<float>("volume");
        }

        public static bool IsAudioModuleEnabled()
        {
            var audioSettings = AudioSettings;
            if (audioSettings == null)
            {
                Debug.Log("[Audio Controller]: Audio settings are missing");

                return false;
            }

            // return audioSettings.IsAudioEnabled;
            return true;
        }

        public static bool IsVibrationModuleEnabled()
        {
            var audioSettings = AudioSettings;
            if (audioSettings == null)
            {
                Debug.Log("[Audio Controller]: Audio settings are missing");

                return false;
            }

            // return audioSettings.IsVibrationEnabled;
            return true;
        }

        public static bool IsVibrationEnabled()
        {
            // if (!instance.audioSettings.IsVibrationEnabled)
            // return false;

            return GameSettingsPrefs.Get<bool>("vibration");
        }

        public static void SetSourceDefaultSettings(AudioSource source, AudioType type = AudioType.Sound)
        {
            // float volume = GameSettingsPrefs.Get<float>("volume");

            if (type == AudioType.Sound)
                source.loop = false;
            else if (type == AudioType.Music) source.loop = true;

            source.clip = null;

            source.volume = 1.0f;
            source.pitch = 1.0f;
            source.spatialBlend = 0; // 2D Sound
            source.mute = false;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = null;
        }

        public static void LoadAudioClipFromStreamingAssets(string fileName, Action<AudioClip> OnAudioLoaded)
        {
            Tween.InvokeCoroutine(instance.LoadAudioClipFromSACoroutine(fileName, OnAudioLoaded));
        }

        private IEnumerator LoadAudioClipFromSACoroutine(string fileName, Action<AudioClip> OnAudioLoaded)
        {
            var directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
            var allFiles = directoryInfo.GetFiles("*.wav");

            foreach (var file in allFiles)
                if (file.Name.Contains(fileName))
                {
                    if (file.Name.Contains("meta"))
                    {
                        yield break;
                    }

                    var musicFilePath = file.FullName;
                    var url = string.Format("file://{0}", musicFilePath);

                    using (var www = UnityWebRequestMultimedia.GetAudioClip(url, UnityEngine.AudioType.WAV))
                    {
                        www.SendWebRequest();

                        while (!www.isDone) yield return null;

                        if (www.result == UnityWebRequest.Result.ConnectionError)
                        {
                            Debug.Log(www.error);
                        }
                        else
                        {
                            var clip = DownloadHandlerAudioClip.GetContent(www);
                            clip.name = fileName;

                            OnAudioLoaded?.Invoke(clip);
                        }
                    }
                }
        }
    }
}

// -----------------
// Audio Controller v 0.3
// -----------------

// Changelog
// v 0.3
// • Added IsAudioModuleEnabled method
// • Added IsVibrationModuleEnabled method
// • Removed VibrationToggleButton class
// v 0.2
// • Removed MODULE_VIBRATION
// v 0.1
// • Added basic version
// • Added support of new initialization
// • Music and Sound volume is combined