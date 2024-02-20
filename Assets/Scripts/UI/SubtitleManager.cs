using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup subtitlePanel;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private List<string> subtitles;
    [SerializeField] private List<string> voicelineNames;

    int currLine;

    void Awake()
    {
        subtitlePanel.gameObject.SetActive(false);
        subtitlePanel.alpha = 0;
        subtitleText.text = string.Empty;
        currLine = 0;
    }

    public void NextLine()
    {
        if (subtitles.Count >= currLine)
        {
            subtitleText.text = subtitles[currLine];

            AudioManager.Instance.PlayVL(voicelineNames[currLine]);

            currLine++;
        }
    }
}
