using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


//[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    List<AudioSource> currentAudioSources = new List<AudioSource>();
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup musicGroup;

    // Start is called before the first frame update
    void Start()
    {
        currentAudioSources.Add(gameObject.GetComponent<AudioSource>());
    }

    // Update is called once per frame
    public void Play(AudioClip clip, bool isMusic)
    {
        foreach (AudioSource source in currentAudioSources)
        {
            if (source.isPlaying)
                continue;

            source.PlayOneShot(clip);
            source.outputAudioMixerGroup = isMusic ? musicGroup : sfxGroup;
            return;
        }

        AudioSource temp = gameObject.AddComponent<AudioSource>();
        currentAudioSources.Add(temp);
        temp.PlayOneShot(clip);
        temp.outputAudioMixerGroup = isMusic ? musicGroup : sfxGroup;
    }


}