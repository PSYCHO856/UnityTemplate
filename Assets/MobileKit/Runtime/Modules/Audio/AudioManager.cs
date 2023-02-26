using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public class AudioManager : Singleton<AudioManager>
    {
        public static bool IsMusicEnabled
        {
            get => GameSettingsPrefs.GetBool("IsMusicEnabled", true);
            set
            {
                GameSettingsPrefs.SetBool("IsMusicEnabled", value);
                if (value)
                {
                    PlayMusic(curMusicClip, musicVolumePercentage, 1f);
                }
                else
                {
                    ReleaseMusic();
                }
            }
        }

        public static bool IsAudioEnabled
        {
            get => GameSettingsPrefs.GetBool("IsAudioEnabled", true);
            set => GameSettingsPrefs.SetBool("IsAudioEnabled", value);
        }

        public static float Volume
        {
            get => GameSettingsPrefs.GetFloat("Volume", 1);
            set
            {
                GameSettingsPrefs.SetFloat("Volume", value);
                SetVolumeForAudioSources();
            }
        }
        
        public static float MusicVolume
        {
            get => GameSettingsPrefs.GetFloat("MusicVolume", 1);
            set
            {
                GameSettingsPrefs.SetFloat("MusicVolume", value);
                SetVolumeForMusicSources();
            }
        }
        

        private static List<AudioSource> audioSources = new List<AudioSource>();
        private static List<AudioSource> activeSounds = new List<AudioSource>();
        private static List<AudioSource> activeMusic = new List<AudioSource>();
        private static List<AudioSource> customSources = new List<AudioSource>();
        private static List<AudioCaseCustom> activeCustomSourcesCases = new List<AudioCaseCustom>();
        private static AudioClip curMusicClip;
        private static float musicVolumePercentage = 1;
        
        public static void Init()
        {
            //Create audio source objects
            for (int i = 0; i < 4; i++)
            {
                audioSources.Add(CreateAudioSourceObject());
            }
        }
        
        /// <summary>
        /// Stop all active streams
        /// </summary>
        public static void ReleaseStreams()
        {
            ReleaseMusic();
            ReleaseSounds();
            ReleaseCustomStreams();
        }

        /// <summary>
        /// Releasing all active music.
        /// </summary>
        public static void ReleaseMusic()
        {
            int activeMusicCount = activeMusic.Count - 1;
            for (int i = activeMusicCount; i >= 0; i--)
            {
                activeMusic[i].Stop();
                activeMusic[i].clip = null;
                activeMusic.RemoveAt(i);
            }
        }

        /// <summary>
        /// Releasing all active sounds.
        /// </summary>
        public static void ReleaseSounds()
        {
            int activeStreamsCount = activeSounds.Count - 1;
            for (int i = activeStreamsCount; i >= 0; i--)
            {
                activeSounds[i].Stop();
                activeSounds[i].clip = null;
                activeSounds.RemoveAt(i);
            }
        }

        /// <summary>
        /// Releasing all active custom sources.
        /// </summary>
        public static void ReleaseCustomStreams()
        {
            int activeStreamsCount = activeCustomSourcesCases.Count - 1;
            for (int i = activeStreamsCount; i >= 0; i--)
            {
                if (activeCustomSourcesCases[i].autoRelease)
                {
                    AudioSource source = activeCustomSourcesCases[i].source;
                    activeCustomSourcesCases[i].source.Stop();
                    activeCustomSourcesCases[i].source.clip = null;
                    activeCustomSourcesCases.RemoveAt(i);
                    customSources.Add(source);
                }
            }
        }

        public static void StopStream(AudioCase audioCase, float fadeTime = 0)
        {
            if (audioCase.type == AudioType.Sound)
            {
                StopSound(audioCase.source, fadeTime);
            }
            else
            {
                StopMusic(audioCase.source, fadeTime);
            }
        }
        

        public static void StopStream(AudioCaseCustom audioCase, float fadeTime = 0)
        {
            ReleaseCustomSource(audioCase, fadeTime);
        }

        private static AudioSource currentMusicSource;
        public static void PlayMusic(AudioClip clip, float volumePercentage = 1.0f, float fadeTime = 0)
        {
            if (clip == null)
            {
                Debug.LogWarning("Music is null.");
                return ;
            }
            StopCurrentMusic(1f);
            curMusicClip = clip;
            musicVolumePercentage = volumePercentage;
            if (!IsMusicEnabled) return;

            AudioSource source = GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Music);

            source.clip = clip;
            source.volume = volumePercentage * MusicVolume;
            source.Play();
            currentMusicSource = source;
            AddMusic(source);
            
            if (fadeTime > 0)
            {
                float volume = source.volume;
                source.volume = 0;
                source.MDOVolume(volume, fadeTime).SetEasing(Ease.Type.Linear);
            }
        }

        public static void StopCurrentMusic(float fadeTime = 0)
        {
            if (currentMusicSource != null)
            {
                StopMusic(currentMusicSource, fadeTime);
            }
        }

        public static AudioCase PlaySmartMusic(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            if (!IsMusicEnabled) return null;

            if (clip == null)
                Debug.LogError("[AudioManager]: Audio clip is null");

            AudioSource source = GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Music);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;

            AudioCase audioCase = new AudioCase(clip, source, AudioType.Music);

            audioCase.Play();

            AddMusic(source);

            return audioCase;
        }


        public static AudioSource PlaySound(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            if (!IsAudioEnabled) return null;

            if (clip == null)
                Debug.LogWarning("[AudioManager]: Audio clip is null");

            AudioSource source = GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Sound);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;
            source.Play();

            AddSound(source);
            return source;
        }

        public static AudioCase PlaySmartSound(AudioClip clip, float volumePercentage = 1.0f, float pitch = 1.0f)
        {
            if (!IsAudioEnabled) return null;

            if (clip == null)
                Debug.LogWarning("[AudioManager]: Audio clip is null");

            AudioSource source = GetAudioSource();

            SetSourceDefaultSettings(source, AudioType.Sound);

            source.clip = clip;
            source.volume *= volumePercentage;
            source.pitch = pitch;

            AudioCase audioCase = new AudioCase(clip, source, AudioType.Sound);
            audioCase.Play();

            AddSound(source);

            return audioCase;
        }

        public static AudioCaseCustom GetCustomSource(bool autoRelease, AudioType audioType)
        {
            AudioSource source = null;

            if (!customSources.IsNullOrEmpty())
            {
                source = customSources[0];
                customSources.RemoveAt(0);
            }
            else
            {
                source = CreateAudioSourceObject();
            }

            SetSourceDefaultSettings(source, audioType);

            AudioCaseCustom audioCase = new AudioCaseCustom(null, source, audioType, autoRelease);

            activeCustomSourcesCases.Add(audioCase);

            return audioCase;
        }

        public static void ReleaseCustomSource(AudioCaseCustom audioCase, float fadeTime = 0)
        {
            int streamID = activeCustomSourcesCases.FindIndex(x => x.source == audioCase.source);
            if (streamID != -1)
            {
                if (fadeTime == 0)
                {
                    activeCustomSourcesCases[streamID].source.Stop();
                    activeCustomSourcesCases[streamID].source.clip = null;
                    activeCustomSourcesCases.RemoveAt(streamID);
                    customSources.Add(audioCase.source);
                }
                else
                {
                    activeCustomSourcesCases[streamID].source.MDOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        activeCustomSourcesCases.Remove(audioCase);
                        audioCase.source.Stop();
                        customSources.Add(audioCase.source);
                    });
                }
            }
        }

        public static void StopSound(AudioSource source, float fadeTime = 0)
        {
            if (source == null) return;
            int streamID = activeSounds.FindIndex(x => x == source);
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
                    activeSounds[streamID].MDOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        activeSounds.Remove(source);
                        source.Stop();
                        source.volume = 1f;
                    });
                }
            }
        }

        public static void StopMusic(AudioSource source, float fadeTime = 0)
        {
            int streamID = activeMusic.FindIndex(x => x == source);
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
                    activeMusic[streamID].MDOVolume(0f, fadeTime).OnComplete(() =>
                    {
                        activeMusic.Remove(source);
                        source.Stop();
                    });
                }
            }
        }
        
        private static AudioSource CreateAudioSourceObject()
        {
            AudioSource audioSource = Instance.gameObject.AddComponent<AudioSource>();
            SetSourceDefaultSettings(audioSource);
            return audioSource;
        }
        
        private static void AddMusic(AudioSource source)
        {
            if (!activeMusic.Contains(source))
            {
                activeMusic.Add(source);
            }
        }

        private static void AddSound(AudioSource source)
        {
            if (!activeSounds.Contains(source))
            {
                activeSounds.Add(source);
            }
        }

        private static AudioSource GetAudioSource()
        {
            int sourcesAmount = audioSources.Count;
            for (int i = 0; i < sourcesAmount; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    return audioSources[i];
                }
            }

            AudioSource createdSource = CreateAudioSourceObject();
            audioSources.Add(createdSource);
            return createdSource;
        }


        private static void SetVolumeForAudioSources()
        {
            // setuping all active sound sources
            int activeSoundSourcesCount = activeSounds.Count;
            for (int i = 0; i < activeSoundSourcesCount; i++)
            {
                activeSounds[i].volume = Volume;
            }

            // setuping all custom sound sources
            activeSoundSourcesCount = activeCustomSourcesCases.Count;
            for (int i = 0; i < activeSoundSourcesCount; i++)
            {
                activeCustomSourcesCases[i].source.volume = Volume;
            }
        }
        
        private static void SetVolumeForMusicSources()
        {
            int activeSoundSourcesCount = activeMusic.Count;
            for (int i = 0; i < activeSoundSourcesCount; i++)
            {
                activeMusic[i].volume = MusicVolume * musicVolumePercentage;
            }
        }

        public static void SetSourceDefaultSettings(AudioSource source, AudioType type = AudioType.Sound)
        {
            source.loop = type switch
            {
                AudioType.Sound => false,
                AudioType.Music => true,
                _ => source.loop
            };
            source.clip = null;
            source.volume = Volume;
            source.pitch = 1.0f;
            source.spatialBlend = 0;
            source.mute = false;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = null;
        }

        public enum AudioType
        {
            Music = 0,
            Sound = 1
        }
    }
}