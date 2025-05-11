using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
    public AudioSource audioSource; 
    public List<AudioClip> soundClips; 
    
    private Dictionary<string, AudioClip> sounds;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        sounds = new Dictionary<string, AudioClip>();
        sounds.Add("suction", soundClips[0]); 
        sounds.Add("end  of suction - full", soundClips[1]); 
        sounds.Add("first jump", soundClips[2]); 
        sounds.Add("squeeze", soundClips[3]); 
        sounds.Add("landing", soundClips[4]); 
        sounds.Add("second jump", soundClips[5]);

    }

    public void PlaySound(string action)
    {
        if (sounds.ContainsKey(action))
        {
            audioSource.clip = sounds[action];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound not found for action: " + action);
        }
    }
}
