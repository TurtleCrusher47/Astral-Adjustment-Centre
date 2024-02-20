using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : Singleton<TimelineManager>
{
    [SerializeField] private GameObject subtitlePanel;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private List<TimelineAsset> timelines = new List<TimelineAsset>();
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
}
