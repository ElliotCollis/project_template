using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    // used for running game options
    public class PlayerSettings
    {
        bool initialized = false;

        public PlayerSettings  ()
        {
            if (!initialized)
                Initialize();
        }

        void Initialize ()
        {
            // Load all saved values or set default

            // Default values
            CurrentLanguage = Localization.SupportedLanguages.English;
            MasterVolume = 0.5f;
            MusicVolume = 0.5f;
            SfxVolume = 0.5f;
            MuteAllAduio = false;
            MuteMusicAduio = false;
            MuteSfxAduio = false; 

            initialized = true;
        }

        Localization.SupportedLanguages currentLanguage = Localization.SupportedLanguages.English;

        public Localization.SupportedLanguages CurrentLanguage
        {
            get => currentLanguage;            
            set
            {
                currentLanguage = value;
                GameManager.instance.localization.SetLanguage(currentLanguage);
            }
        }

        /// <summary>
        /// Audio Options
        /// </summary>

        [Range(0, 1)]
        float masterVolume = 0.5f;

        public float MasterVolume
        {
            get => masterVolume;            
            set
            {
                masterVolume = value;
                muteAllAduio = false;
                GameManager.instance.audioManager.SetMasterVolume(masterVolume);
            }
        }

        [Range (0,1)]
        float musicVolume = 0.5f;

        public float MusicVolume
        {
            get =>musicVolume;            
            set
            {
                musicVolume = value;
                muteMusicAduio = false;
                GameManager.instance.audioManager.SetMusicVolume(musicVolume);
            }
        }

        [Range(0, 1)]
        float sfxVolume = 0.5f;

        public float SfxVolume
        {
            get => sfxVolume;            
            set
            {
                sfxVolume = value;
                muteSfxAduio = false;
                GameManager.instance.audioManager.SetSfxVolume(sfxVolume);
            }
        }

        bool muteAllAduio = false;

        public bool MuteAllAduio
        {
            get => muteAllAduio;         
            set
            {
                muteAllAduio = value;
                GameManager.instance.audioManager.SetMasterVolume(muteAllAduio ? 0 : masterVolume);
            }
        }

        bool muteMusicAduio = false;

        public bool MuteMusicAduio
        {
            get => muteMusicAduio;            
            set
            {
                muteMusicAduio = value;
                GameManager.instance.audioManager.SetMusicVolume(muteMusicAduio ? 0 : musicVolume);
            }
        }

        bool muteSfxAduio = false;

        public bool MuteSfxAduio
        {
            get => muteSfxAduio;            
            set
            {
                muteSfxAduio = value;
                GameManager.instance.audioManager.SetSfxVolume(muteSfxAduio ? 0 : sfxVolume);
            }
        }
    }
}
