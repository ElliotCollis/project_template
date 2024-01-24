using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    // used for running game options
    public class PlayerSettings
    {
        Localization.SupportedLanguages currentLanguage = Localization.SupportedLanguages.English;

        public Localization.SupportedLanguages CurrentLanguage
        {
            get
            {
                return currentLanguage;
            }
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
            get
            {
                return masterVolume;
            }
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
            get
            {
                return musicVolume;
            }
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
            get
            {
                return sfxVolume;
            }
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
            get
            {
                return muteAllAduio;
            }
            set
            {
                muteAllAduio = value;
                GameManager.instance.audioManager.SetMasterVolume(muteAllAduio ? 0 : masterVolume);
            }
        }

        bool muteMusicAduio = false;

        public bool MuteMusicAduio
        {
            get
            {
                return muteMusicAduio;
            }
            set
            {
                muteMusicAduio = value;
                GameManager.instance.audioManager.SetMusicVolume(muteMusicAduio ? 0 : musicVolume);
            }
        }

        bool muteSfxAduio = false;

        public bool MuteSfxAduio
        {
            get
            {
                return muteSfxAduio;
            }
            set
            {
                muteSfxAduio = value;
                GameManager.instance.audioManager.SetSfxVolume(muteSfxAduio ? 0 : sfxVolume);
            }
        }
    }
}
