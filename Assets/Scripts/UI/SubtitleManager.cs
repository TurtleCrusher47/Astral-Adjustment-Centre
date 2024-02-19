using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private List<string> subtitles;
    [SerializeField] private List<string> voicelineNames;

    int currLine;

    void Awake()
    {
        subtitleText.text = string.Empty;
        currLine = 0;
    }

    public void NextLine()
    {
        subtitleText.text = subtitles[currLine];

        AudioManager.Instance.PlayVL(voicelineNames[currLine]);

        currLine++;
    }
}
