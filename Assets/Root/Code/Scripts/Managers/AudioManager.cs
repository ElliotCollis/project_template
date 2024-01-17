using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace HowlingMan
{
    public class AudioManager : MonoBehaviour
    {
        private FMOD.Studio.EventInstance bgmInstance;

        public void PlaySFX(string sfxPath)
        {
            if (EventExists("event:/SFX/" + sfxPath))
            {
                RuntimeManager.PlayOneShot("event:/SFX/" + sfxPath);
            }
            else
            {
                Debug.LogError("SFX path not found: " + "event:/SFX/" + sfxPath);
            }
        }

        public void PlayBGM(string bgmPath, float fadeDuration = 1.0f)
        {
            if (EventExists("event:/Music/" + bgmPath))
            {
                if (bgmInstance.isValid())
                {
                    bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    bgmInstance.release();
                }

                bgmInstance = RuntimeManager.CreateInstance("event:/Music/" + bgmPath);
                bgmInstance.start();

                // Implement fading logic if necessary
            }
            else
            {
                Debug.LogError("BGM path not found: " + "event:/Music/" + bgmPath);
            }
        }

        private bool EventExists(string path)
        {
            FMOD.Studio.EventDescription eventDescription;
            FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out eventDescription);

            if (result != FMOD.RESULT.OK)
            {
                return false;
            }

            return eventDescription.isValid();
        }

        // Optional: Coroutine for fading BGM
        // IEnumerator FadeBGM(FMOD.Studio.EventInstance instance, float targetVolume, float duration) { ... }

        // Example usage
        // audioManager.PlaySFX("event:/SFX/YourSFXEvent");
        // audioManager.PlayBGM("event:/Music/YourBGMEvent");

    }
}
