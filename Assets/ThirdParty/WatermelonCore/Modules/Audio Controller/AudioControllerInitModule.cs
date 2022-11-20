#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class AudioControllerInitModule : InitModule
    {
        [SerializeField] private AudioSettings audioSettings;

        public AudioControllerInitModule()
        {
            moduleName = "Audio Controller";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            var audioController = new AudioController();
            audioController.Init(audioSettings, Initialiser.gameObject);
        }
    }
}

// -----------------
// Audio Controller v 0.3
// -----------------