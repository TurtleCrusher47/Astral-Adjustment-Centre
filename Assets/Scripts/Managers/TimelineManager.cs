using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelineManager : Singleton<TimelineManager>
{
    public Button skipButton;
    [SerializeField] private GameObject subtitlePanel;
    [SerializeField] private GameObject topPanel, btmPanel;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private List<TimelineAsset> timelines = new List<TimelineAsset>();
    public string nextSceneName;
    public int cutsceneIndex = 0;

    void Awake()
    {
        cutsceneIndex = 0;

        director = GetComponent<PlayableDirector>();
        director.playableAsset = timelines[cutsceneIndex];

        subtitlePanel.SetActive(false);
    }

    public IEnumerator PlayCutscene(string cutsceneName, string nextScene)
    {
        nextSceneName = nextScene;

        switch (cutsceneName)
        {
            case "PostIntro":
                skipButton.gameObject.SetActive(false);
                topPanel.SetActive(false);
                btmPanel.SetActive(false);
                break;
            default:
                skipButton.gameObject.SetActive(true);
                topPanel.SetActive(true);
                btmPanel.SetActive(true);
                break;
        }

        gameObject.GetComponent<SubtitleManager>().ResetText();

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.SetBGMSourcesVol(0.2f));

        for (int i = 0; i < timelines.Count; i++)
        {
            if (timelines[i].name == (cutsceneName + "Cutscene"))
            {
                cutsceneIndex = i;
            }
        }

        director.playableAsset = timelines[cutsceneIndex];
        director.Play();

        yield return new WaitUntil(() => director.state == PlayState.Paused);

        yield return new WaitForSeconds(0.75f);

        if (nextScene != null)
        {
            GameManager.Instance.ChangeScene(nextScene);
        }
        else
        {
            subtitlePanel.SetActive(false);
            yield return null;
        }

        yield return new WaitForSeconds (0.25f);

        subtitlePanel.SetActive(false);
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();

        skipButton.gameObject.SetActive(false);

        if (director.state == PlayState.Playing)
        {
            director.Stop();
            cutsceneIndex++;
        }

        GameManager.Instance.ChangeScene(nextSceneName);

        subtitlePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            GameManager.Instance.ChangeScene("MenuScene");
        }
    }
}
