using System;
using System.Collections.Generic;
using DoubleTrouble.Managers;
using DoubleTrouble.Utilities;
using UnityEngine;
using UnityEngine.Rendering;

namespace SpongeScene.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private List<SerializableTuple<SoundName, AudioClip>> gameSounds;
        
        [SerializeField] private AudioSource src;
        [SerializeField] private AudioSource src1;
        
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private AudioClip endMusic;
        [SerializeField] private AudioClip cutSceneMusic;

        public void Init()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.StartGame, (object o) => PlayMusic(gameMusic));
            CoreManager.Instance.EventsManager.AddListener(EventNames.GameOver, (object o) => PlayMusic(endMusic));
            CoreManager.Instance.EventsManager.AddListener(EventNames.StartEndCutScene, (object o) => PlayMusic(cutSceneMusic));
            CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, (object o) => PlayMusic(gameMusic));
            CoreManager.Instance.EventsManager.AddListener(EventNames.ToMainMenu, (object o) => PlayMusic(menuMusic));
        }

        private void Start()
        {
            src1.clip = menuMusic;
            src1.Play();
        }

        private AudioClip GetSoundBySoundName(SoundName name)
        {
            foreach (var kvp in gameSounds)
            {
                if (kvp.first == name)
                {
                    return kvp.second;
                }
            }

            return default;
        }

        public void PlaySoundByName(SoundName soundName)
        {
            var sound = GetSoundBySoundName(soundName);

            src.PlayOneShot(sound);
        }

        public void PlaySound(AudioClip sound)
        {
            src.PlayOneShot(sound);
        }

        public void PlaySoundBySource(AudioSource source, SoundName soundName)
        {
            source.Stop();
            source.clip = gameSounds.Find(pair => pair.first == soundName).second;
            source.Play();
        }

        public void PlayMusic(AudioClip c)
        {
            src1.Stop();
            print($"playing : {c.name}");
            src1.clip = c;
            src1.Play();
        }
        
        
    }
    //dsa

    public enum SoundName
    {
        None = 0,
        WaterSplash = 1,
        GateOpenSlow = 2,
        GateOpenFast = 3,
        ButtonPressed = 4,
        ObjectGrow = 5,
        BoilingWater = 6,
        Gayser = 7,
        Shuriken = 8,
        Die=9,
        WallBreak=10,
        Token=11,
        Upgrade=12,
        ObjectDecrease=13,
        WaterButton = 14,
        Pop = 15,
        CheckPoint=16,
        
    }
}