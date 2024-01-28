using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace HowlingMan
{
    public enum MusicList
    {
        ActandContemplate,
        EternalExplorer,
        Largo,
        Oldskool,
        Timed,
        Voltage
    }

    public enum SFXList
    {
        back,
        click,
        close,
        confirm,
        drop,
        error,
        open,
        rollover,
        sswitch,
        toggle
    }

    public enum SFXFolder
    {
        UI,
        player
    }

    public class AudioManager : MonoBehaviour
    {
        FMOD.Studio.EventInstance bgmInstance;

        public void PlaySFX(SFXList sfxPath, SFXFolder sfxFolder)
        {
            if (EventExists("event:/SFX/" + sfxFolder + "/" + sfxPath))
            {
                RuntimeManager.PlayOneShot("event:/SFX/" + sfxFolder + "/" + sfxPath);
            }
            else
            {
                Debug.LogError("SFX path not found: " + "event:/SFX/" + sfxFolder + "/" + sfxPath);
            }
        }

        public void PlayBGM(MusicList bgmPath, float fadeDuration = 1.0f)
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

        bool EventExists(string path)
        {
            FMOD.Studio.EventDescription eventDescription;
            FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out eventDescription);

            if (result != FMOD.RESULT.OK)
            {
                return false;
            }

            return eventDescription.isValid();
        }

        /// <summary>
        /// Options
        /// </summary>

        public void SetMasterVolume(float newValue)
        {
            FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/");

            if (masterBus.isValid())
            {
                masterBus.setVolume(newValue);
                Debug.Log("set master bus");
                return;
            }

            Debug.LogError("Failed to set volume: Master bus not found.");
        }

        public void SetMusicVolume(float newValue)
        {
            FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/Music Bus");

            if (masterBus.isValid())
            {
                masterBus.setVolume(newValue);
                return;
            }

            Debug.LogError("Failed to set volume: Master bus not found.");
        }

        public void SetSfxVolume(float newValue)
        {
            FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/Sfx Bus");

            if (masterBus.isValid())
            {
                masterBus.setVolume(newValue);
                return;
            }

            Debug.LogError("Failed to set volume: Master bus not found.");
        }

        // Optional: Coroutine for fading BGM
        // IEnumerator FadeBGM(FMOD.Studio.EventInstance instance, float targetVolume, float duration) { ... }

        // Example usage
        // audioManager.PlaySFX("event:/SFX/YourSFXEvent");
        // audioManager.PlayBGM("event:/Music/YourBGMEvent");

    }
}
