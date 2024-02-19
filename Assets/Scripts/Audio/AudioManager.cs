using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<AudioSource> sfxSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> vlSources = new List<AudioSource>();

    [SerializeField] private List<AudioClip> bgmClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> vlClips = new List<AudioClip>();

    void Awake()
    {
        bgmSource = GameManager.Instance.FindChildWithTag(gameObject, "BGM").GetComponent<AudioSource>();
        sfxSources = GameManager.Instance.FindChildWithTag(gameObject, "SFX").GetComponents<AudioSource>().ToList();
        vlSources = GameManager.Instance.FindChildWithTag(gameObject, "VL").GetComponents<AudioSource>().ToList();
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

    public void PlayVL(string name)
    {
        AudioClip clipToPlay = null;

        for (int i = 0; i < vlClips.Count; i++)
        {
            if (vlClips[i].name == name)
            {
                clipToPlay = vlClips[i];
            }
        }

        for (int i = 0; i < vlSources.Count; i++)
        {
            if (!vlSources[i].isPlaying)
            {
                vlSources[i].clip = clipToPlay;
                vlSources[i].Play();
                break;
            }
        }
    }
}
