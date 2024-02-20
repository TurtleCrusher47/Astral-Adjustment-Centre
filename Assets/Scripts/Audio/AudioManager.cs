using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private List<AudioSource> bgmSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> sfxSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> vlSources = new List<AudioSource>();

    [SerializeField] private List<AudioClip> bgmClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> vlClips = new List<AudioClip>();

    void Awake()
    {
        bgmSources = GameManager.Instance.FindChildWithTag(gameObject, "BGM").GetComponents<AudioSource>().ToList();
        sfxSources = GameManager.Instance.FindChildWithTag(gameObject, "SFX").GetComponents<AudioSource>().ToList();
        vlSources = GameManager.Instance.FindChildWithTag(gameObject, "VL").GetComponents<AudioSource>().ToList();
    }

    public IEnumerator PlayBGM(string name)
    {
        AudioSource currSource = new AudioSource();
        AudioSource newSource = new AudioSource();
        AudioClip bgmToPlay = null;
        int sourcesNotPlaying = 0;

        for (int i = 0; i < bgmClips.Count; i++)
        {
            if (bgmClips[i].name == name)
            {
                bgmToPlay = bgmClips[i];
            }
        }

        for (int i = 0; i < bgmSources.Count; i++)
        {
            if (bgmSources[i].isPlaying)
            {
                currSource = bgmSources[i];
            }
            else
            {
                newSource = bgmSources[i];
                sourcesNotPlaying++;
            }
        }

        newSource.volume = 0;
        newSource.clip = bgmToPlay;
        newSource.loop = true;
        newSource.Play();

        if (sourcesNotPlaying >= bgmSources.Count)
        {
            while (newSource.volume < 1)
            {
                newSource.volume += Time.deltaTime / 2;

                yield return null;
            }
        }
        else if (currSource.isPlaying)
        {
            while (currSource.volume > 0 || newSource.volume < 1)
            {
                currSource.volume -= Time.deltaTime / 2;
                newSource.volume += Time.deltaTime / 2;

                yield return null;
            }

            currSource.volume = 0;
            newSource.volume = 1;
        }
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

    public IEnumerator SetBGMSourcesVol(float vol)
    {
        bool fadeIn = vol > bgmSources[0].volume;

        if (fadeIn)
        {
            while (bgmSources[0].volume < vol)
            {
                foreach (AudioSource source in bgmSources)
                {
                    source.volume += Time.deltaTime / 2;
                }

                yield return null;
            }
        }
        else
        {
            while (bgmSources[0].volume > vol)
            {
                foreach (AudioSource source in bgmSources)
                {
                    source.volume -= Time.deltaTime / 2;
                }

                yield return null;
            }
        }
    }
}
