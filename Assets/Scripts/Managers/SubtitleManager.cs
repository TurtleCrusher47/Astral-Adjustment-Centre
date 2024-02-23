using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup subtitlePanel;
    [SerializeField] private TMP_Text playerSubtitleText, enemySubtitleText;
    [SerializeField] private List<string> playerSubtitles, enemySubtitles;
    [SerializeField] private List<string> playerVoicelines, enemyVoicelines;

    private int playerCurrLine;
    private int enemyCurrLine;

    void Awake()
    {
        subtitlePanel.gameObject.SetActive(false);
        subtitlePanel.alpha = 0;
        playerSubtitleText.text = string.Empty;
        enemySubtitleText.text = string.Empty;
        playerCurrLine = 0;
    }

    public void ResetText()
    {
        playerSubtitleText.text = string.Empty;
        enemySubtitleText.text = string.Empty;
    }

    public void PlayerNextLine()
    {
        if (playerSubtitles.Count >= playerCurrLine)
        {
            playerSubtitleText.text = playerSubtitles[playerCurrLine];

            AudioManager.Instance.PlayVL(playerVoicelines[playerCurrLine]);

            playerCurrLine++;
        }
        else
        {
            playerCurrLine = 0;

            playerSubtitleText.text = playerSubtitles[playerCurrLine];

            AudioManager.Instance.PlayVL(playerVoicelines[playerCurrLine]);

            playerCurrLine++;
        }
    }

    public void EnemyNextLine()
    {
        if (enemySubtitles.Count >= enemyCurrLine)
        {
            enemySubtitleText.text = enemySubtitles[enemyCurrLine];

            AudioManager.Instance.PlayVL(enemyVoicelines[enemyCurrLine]);

            enemyCurrLine++;
        }
        else
        {
            enemyCurrLine = 0;

            enemySubtitleText.text = enemySubtitles[enemyCurrLine];

            AudioManager.Instance.PlayVL(enemyVoicelines[enemyCurrLine]);

            enemyCurrLine++;
        }
    }
}
