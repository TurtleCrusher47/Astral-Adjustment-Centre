using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] List<AudioSource> audSources = new List<AudioSource>();
    [SerializeField] AudioSource bgmSource;
    [SerializeField] List<AudioSource> sfxSources = new List<AudioSource>();

    [SerializeField] List<AudioClip> bgmClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxClips = new List<AudioClip>();

    void Awake()
    {
        gameObject.GetComponents(audSources);

        bgmSource = audSources[0];

        for (int i = 1; i < audSources.Count; i++)
        {
            sfxSources.Add(audSources[i]);
        }
    }

    public void PlayBGM(string name)
    {
        AudioClip bgmToPlay = null;

        for (int i = 0; i < bgmClips.Count; i++)
        {
            if (bgmClips[i].name == name)
            {
                bgmToPlay = bgmClips[i];
            }
        }

        bgmSource.clip = bgmToPlay;
        bgmSource.Play();
        bgmSource.loop = true;
    }

    public void PlaySFX(string name)
    {
        AudioClip clipToPlay = null;

        for (int i = 0; i < sfxClips.Count; i++)
        {
            if (sfxClips[i].name == name)
            {
                clipToPlay = sfxClips[i];
            }
        }

        for (int i = 0; i < sfxSources.Count; i++)
        {
            if (!sfxSources[i].isPlaying)
            {
                sfxSources[i].clip = clipToPlay;
                sfxSources[i].Play();
                break;
            }
        }
    }
}
